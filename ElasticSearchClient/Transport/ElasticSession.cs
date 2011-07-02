using System;
using ElasticSearch.Client.Exception;
using ElasticSearch.Client.Transport.Connection;
using ElasticSearch.Client.Transport.IDL;

namespace ElasticSearch.Client.Transport
{
	/// <summary>
	/// ElasticSearch session
	/// </summary>
	internal class ESSession : IDisposable
	{
		[ThreadStatic] private static ESSession _current;

		private IConnection _connection;
		private bool _disposed;

		public ESSession(IConnectionProvider connectionProvider)
		{
			if (Current != null)
				throw new ElasticSearchException("Cannot create a new session while there is one already active.");
			ConnectionProvider = connectionProvider;
			Current = this;
		}
		public Server CurrentServer
		{
			get { return _connection.Server; }
		}
		public static ESSession Current
		{
			get { return _current; }
			internal set { _current = value; }
		}

		/// <summary>
		/// Gets ConnectionProvider.
		/// </summary>
		internal IConnectionProvider ConnectionProvider { get; private set; }

		#region IDisposable Members

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		public Rest.Client GetClient()
		{
			if (_connection == null || !_connection.IsOpen)
				_connection = ConnectionProvider.Open();

			return _connection.Client;
		}

		/// <summary>
		/// The dispose.
		/// </summary>
		/// <param name="disposing">
		/// The disposing.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				if (_connection != null)
					ConnectionProvider.Close(_connection);

				if (Current == this)
					Current = null;
			}

			_disposed = true;
		}

		/// <summary>
		/// Finalizes an instance of the ESSession
		/// </summary>
		~ESSession()
		{
			Dispose(false);
		}
	}
}