using System;
using System.Net.Sockets;
using System.Threading;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Transport.Connection;
using ElasticSearch.Client.Utils;
using Thrift.Transport;

namespace ElasticSearch.Client.Transport
{
	//TODO reload config
	internal class ESNode
	{
		private static readonly LogWrapper logger = LogWrapper.GetLogger();

		public ESNode(NodeDefinition nodeDefinition)
		{
			Enabled = nodeDefinition.Enabled;
			Host = nodeDefinition.Host;
			Port = nodeDefinition.Port;
//			Framed = nodeDefinition.IsFramed;
			DangerZoneThreshold = nodeDefinition.DangerZoneThreshold;
			DangerZoneSeconds = nodeDefinition.DangerZoneSeconds;
			ConnectionProvider = new ConnectionPool(nodeDefinition.Host, nodeDefinition.Port, nodeDefinition.ConnectionPool??ElasticSearchConfig.Instance.ConnectionPool,nodeDefinition.IsFramed);
		}
		public ESNode(string ip,int port,bool framed=false)
		{
			Host = ip;
			Port = port;
			Enabled = true;
//			Framed = framed;
			ConnectionProvider = new ConnectionPool(Host, Port, ElasticSearchConfig.Instance.ConnectionPool,framed);
		}

		public bool Enabled { get; private set; }
		public string Host { get; private set; }
		public int Port { get; private set; }
//		public bool Framed { get; private set; }
		public int DangerZoneThreshold { get; private set; }
		public int DangerZoneSeconds { get; private set; }
		public IConnectionProvider ConnectionProvider { get; private set; }

		public bool Unreachable
		{
			get
			{
				return (_serverUnreachableErrors > 0
				        && _lastServerUnreachable.AddSeconds(_serverUnreachableWaitSeconds) > DateTime.Now);
			}
		}

		public bool InDangerZone
		{
			get
			{
				if (Unreachable) return true;

				if (_serverDownErrors > 0
				    && _serverDownErrorsLast30Seconds > DangerZoneThreshold
				    && _lastServerDownTime.AddSeconds(DangerZoneSeconds) > DateTime.Now)
				{
					return true;
				}

				return false;
			}
		}

		#region server status track

		private const int ServerUnreachableBaseWaitSeconds = 30;
		private const int ServerUnreachableMaxWaitSeconds = 60*5;
		private readonly AggregateCounter _serverDownCounter = new AggregateCounter(60); // 30 seconds, tricked every 500 ms
		private DateTime _lastServerDownTime;
		private DateTime _lastServerUnreachable;
		private int _serverDownErrors;
		private int _serverDownErrorsLast30Seconds;
		private AggregateCounter _serverUnreachableCounter = new AggregateCounter(120); // 60 seconds, ticked every 500 ms

		private int _serverUnreachableErrors;
		private int _serverUnreachableErrorsLast2WaitPeriods;
		private int _serverUnreachableWaitSeconds = 60; // initial value, will be increased as errors are encountered

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
			Interlocked.Increment(ref _serverDownErrors);
			_serverDownCounter.IncrementCounter();
			_lastServerDownTime = DateTime.Now;
		}

		private void IncrementServerUnreachable()
		{
			// If we've gotten too many in the last 2 wait period, then increase the wait time
			if (_serverUnreachableErrorsLast2WaitPeriods >= 2)
			{
				if (_serverUnreachableWaitSeconds <= ServerUnreachableMaxWaitSeconds)
				{
					_serverUnreachableWaitSeconds = (int) (_serverUnreachableWaitSeconds*1.5);
					// want twice the wait period, and then twice that many seconds because it's ticked every 500ms	
					_serverUnreachableCounter = new AggregateCounter(_serverUnreachableWaitSeconds*4);
				}
			}
			else if (_serverUnreachableErrorsLast2WaitPeriods == 0 && _serverUnreachableWaitSeconds != ServerUnreachableBaseWaitSeconds)
			{
				_serverUnreachableWaitSeconds = ServerUnreachableBaseWaitSeconds;
				_serverUnreachableCounter = new AggregateCounter(_serverUnreachableWaitSeconds*4);
			}

			Interlocked.Increment(ref _serverUnreachableErrors);
			_serverUnreachableCounter.IncrementCounter();
			_lastServerUnreachable = DateTime.Now;
		}

		internal void AggregateCounterTicker()
		{
			int count = _serverDownCounter.Tick();
			if (count != -1)
			{
				_serverDownErrorsLast30Seconds = _serverDownCounter.Tick();
			}
			else
			{
				if (logger.IsDebugEnabled)
					logger.DebugFormat("Node {0} tried to tick its aggregate counters simultaneously!", this);
			}

			count = _serverUnreachableCounter.Tick();
			if (count != -1)
			{
				_serverUnreachableErrorsLast2WaitPeriods = _serverUnreachableCounter.Tick();
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