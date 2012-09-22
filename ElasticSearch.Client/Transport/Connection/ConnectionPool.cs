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

		private readonly ConnectionPoolConfig _config;
		private readonly Semaphore _connectionLimiter;

		private readonly Queue<IConnection> _connections = new Queue<IConnection>();
		private readonly object _padlock = new object();
		private readonly TSocketSettings _socketSettings;
		private readonly Server _target;
		private readonly bool _useLimiter;
		public string ClusterName { get; set; }

		public ConnectionBuilder Builder
		{
			get { return null; }
		}

		#endregion

		#region ctor

		public ConnectionPool(string host, int port, ConnectionPoolConfig config,bool isframed=false)
		{
			if (config == null) throw new ArgumentNullException("config");

			_target = new Server(host, port,isframed);
			_config = config;
			_socketSettings = config.SocketSettings ?? TSocketSettings.DefaultSettings;

			if (config.PoolSize > 0)
			{
				_useLimiter = true;
				_connectionLimiter = new Semaphore(config.PoolSize, config.PoolSize);
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
					lock (_padlock)
					{
						while (_connections.Count > 0)
						{
							IConnection connection = _connections.Dequeue();
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

			throw new ElasticSearchException("Too many connections to " + _target);
		}

		public bool Close(IConnection connection)
		{
			lock (_padlock)
			{
				if (IsAlive(connection))
					_connections.Enqueue(connection);
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
			if (_config.ConnectionLifetimeMinutes > 0
			    && connection.Created.AddMinutes(_config.ConnectionLifetimeMinutes) < DateTime.Now)
				return false;

			return connection.IsOpen;
		}

		private bool EnterLimiter()
		{
			if (_useLimiter)
			{
				if (_connectionLimiter.WaitOne(_socketSettings.ConnectTimeout, false))
					return true;
				return false;
			}
			return true;
		}

		private void ExitLimiter()
		{
			if (_useLimiter)
			{
				try
				{
					_connectionLimiter.Release();
				}
				catch (SemaphoreFullException)
				{
					logger.ErrorFormat("Connection pool for {0} released a connection too many times", _target);
				}
			}
		}

		private void ReleaseConnection(IConnection connection)
		{
			connection.Dispose();
		}

		public void ReleaseAll()
		{
			lock (_padlock)
			{
				foreach (IConnection connection in _connections)
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
			var connection = new Connection(_target, _socketSettings);
			connection.Open();
			return connection;
		}
	}
}