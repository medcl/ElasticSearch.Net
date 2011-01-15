using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Exception;
using ElasticSearch.Client.Utils;
using Thrift.Transport;

namespace ElasticSearch.Client.Transport.Connection
{
	internal class ConnectionPool : IConnectionProvider
	{
		#region fields

		private static readonly LogWrapper logger = LogWrapper.GetLogger();

		private readonly ConnectionPoolConfig config;
		private readonly Semaphore connectionLimiter;

		private readonly Queue<IConnection> connections = new Queue<IConnection>();
		private readonly object padlock = new object();
		private readonly TSocketSettings socketSettings;
		private readonly Server target;
		private readonly bool useLimiter;

		public ConnectionBuilder Builder
		{
			get { return null; }
		}

		#endregion

		#region ctor

		public ConnectionPool(string host, int port, ConnectionPoolConfig config)
		{
			if (config == null) throw new ArgumentNullException("config");

			target = new Server(host, port);
			this.config = config;
			socketSettings = config.SocketSettings ?? TSocketSettings.DefaultSettings;

			if (config.PoolSize > 0)
			{
				useLimiter = true;
				connectionLimiter = new Semaphore(config.PoolSize, config.PoolSize);
			}
		}

		#endregion

		#region IConnectionProvider Members

		public IConnection Open()
		{
			if (EnterLimiter())
			{
				try
				{
					lock (padlock)
					{
						while (connections.Count > 0)
						{
							IConnection connection = connections.Dequeue();
							if (!IsAlive(connection))
							{
								ReleaseConnection(connection);
								continue;
							}
							return connection;
						}

						return NewConnection();
					}
				}
				catch
				{
					ExitLimiter();
					throw;
				}
			}

			throw new ElasticSearchException("Too many connections to " + target);
		}

		public bool Close(IConnection connection)
		{
			lock (padlock)
			{
				if (IsAlive(connection))
					connections.Enqueue(connection);
				else
					ReleaseConnection(connection);

				ExitLimiter();
			}

			return true;
		}

		public IConnection CreateConnection()
		{
			throw new NotImplementedException();
		}

		#endregion

		private bool IsAlive(IConnection connection)
		{
			if (config.ConnectionLifetimeMinutes > 0
			    && connection.Created.AddMinutes(config.ConnectionLifetimeMinutes) < DateTime.Now)
				return false;

			return connection.IsOpen;
		}

		private bool EnterLimiter()
		{
			if (useLimiter)
			{
				if (connectionLimiter.WaitOne(socketSettings.ConnectTimeout, false))
					return true;
				return false;
			}
			return true;
		}

		private void ExitLimiter()
		{
			if (useLimiter)
			{
				try
				{
					connectionLimiter.Release();
				}
				catch (SemaphoreFullException)
				{
					logger.ErrorFormat("Connection pool for {0} released a connection too many times", target);
				}
			}
		}

		private void ReleaseConnection(IConnection connection)
		{
			connection.Dispose();
		}

		public void ReleaseAll()
		{
			lock (padlock)
			{
				foreach (IConnection connection in connections)
				{
					try
					{
						connection.Dispose();
					}
					catch (SocketException)
					{
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}
		}

		private IConnection NewConnection()
		{
			var connection = new Connection(target, socketSettings);
			connection.Open();
			return connection;
		}
	}
}