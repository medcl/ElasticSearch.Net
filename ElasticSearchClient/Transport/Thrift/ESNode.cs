using System;
using System.Net.Sockets;
using System.Threading;
using ElasticSearch.Config;
using ElasticSearch.Utils;

namespace ElasticSearch
{
	internal class ESNode
	{
		private static readonly LogWrapper logger = LogWrapper.GetLogger();

		public ESNode(NodeDefinition nodeDefinition)
		{
			Enabled = nodeDefinition.Enabled;

			Host = nodeDefinition.Host;
			Port = nodeDefinition.Port;

			DangerZoneThreshold = nodeDefinition.DangerZoneThreshold;
			DangerZoneSeconds = nodeDefinition.DangerZoneSeconds;

			ConnectionProvider =
				ConnectionProviderFactory.Get(new ConnectionBuilder(nodeDefinition.SocketSettings, nodeDefinition.Host,
				                                                    nodeDefinition.Port, nodeDefinition.IsFramed,
				                                                    nodeDefinition.TimeOut, nodeDefinition.EnablePool,
				                                                    nodeDefinition.ConnectionPool.PoolSize,
				                                                    nodeDefinition.ConnectionPool.ConnectionLifetimeMinutes*60000));
		}

		public bool Enabled { get; private set; }
		public string Host { get; private set; }
		public int Port { get; private set; }
		public int DangerZoneThreshold { get; private set; }
		public int DangerZoneSeconds { get; private set; }
		public IConnectionProvider ConnectionProvider { get; private set; }

		public bool Unreachable
		{
			get
			{
				return (serverUnreachableErrors > 0
				        && lastServerUnreachable.AddSeconds(serverUnreachableWaitSeconds) > DateTime.Now);
			}
		}

		public bool InDangerZone
		{
			get
			{
				if (Unreachable) return true;

				if (serverDownErrors > 0
				    && serverDownErrorsLast30Seconds > DangerZoneThreshold
				    && lastServerDownTime.AddSeconds(DangerZoneSeconds) > DateTime.Now)
				{
					return true;
				}

				return false;
			}
		}

		#region server status track

		private const int ServerUnreachableBaseWaitSeconds = 30;
		private const int ServerUnreachableMaxWaitSeconds = 60*5;
		private readonly AggregateCounter serverDownCounter = new AggregateCounter(60); // 30 seconds, tricked every 500 ms
		private DateTime lastServerDownTime;
		private DateTime lastServerUnreachable;
		private int serverDownErrors;
		private int serverDownErrorsLast30Seconds;
		private AggregateCounter serverUnreachableCounter = new AggregateCounter(120); // 60 seconds, ticked every 500 ms

		private int serverUnreachableErrors;
		private int serverUnreachableErrorsLast2WaitPeriods;
		private int serverUnreachableWaitSeconds = 60; // initial value, will be increased as errors are encountered

		public void LogException(System.Exception ex)
		{
			if (ex is SocketException)
			{
				SocketError error = ((SocketException) ex).SocketErrorCode;
				if (error == SocketError.HostUnreachable
				    || error == SocketError.HostNotFound
				    || error == SocketError.ConnectionRefused
				    || error == SocketError.ConnectionReset)
				{
					IncrementServerUnreachable();
				}
				else
				{
					IncrementServerDown();
				}
			}
			else if (ex is ThreadAbortException || ex is NullReferenceException)
			{
				IncrementServerDown();
			}
		}

		private void IncrementServerDown()
		{
			Interlocked.Increment(ref serverDownErrors);
			serverDownCounter.IncrementCounter();
			lastServerDownTime = DateTime.Now;
		}

		private void IncrementServerUnreachable()
		{
			// If we've gotten too many in the last 2 wait period, then increase the wait time
			if (serverUnreachableErrorsLast2WaitPeriods >= 2)
			{
				if (serverUnreachableWaitSeconds <= ServerUnreachableMaxWaitSeconds)
				{
					serverUnreachableWaitSeconds = (int) (serverUnreachableWaitSeconds*1.5);
					// want twice the wait period, and then twice that many seconds because it's ticked every 500ms	
					serverUnreachableCounter = new AggregateCounter(serverUnreachableWaitSeconds*4);
				}
			}
			else if (serverUnreachableErrorsLast2WaitPeriods == 0 && serverUnreachableWaitSeconds != ServerUnreachableBaseWaitSeconds)
			{
				serverUnreachableWaitSeconds = ServerUnreachableBaseWaitSeconds;
				serverUnreachableCounter = new AggregateCounter(serverUnreachableWaitSeconds*4);
			}

			Interlocked.Increment(ref serverUnreachableErrors);
			serverUnreachableCounter.IncrementCounter();
			lastServerUnreachable = DateTime.Now;
		}

		internal void AggregateCounterTicker()
		{
			int count = serverDownCounter.Tick();
			if (count != -1)
			{
				serverDownErrorsLast30Seconds = serverDownCounter.Tick();
			}
			else
			{
				if (logger.IsDebugEnabled)
					logger.DebugFormat("Node {0} tried to tick its aggregate counters simultaneously!", this);
			}

			count = serverUnreachableCounter.Tick();
			if (count != -1)
			{
				serverUnreachableErrorsLast2WaitPeriods = serverUnreachableCounter.Tick();
			}
			else
			{
				if (logger.IsDebugEnabled)
					logger.DebugFormat("Node {0} tried to tick its aggregate counters simultaneously!", this);
			}
		}

		#endregion
	}
}