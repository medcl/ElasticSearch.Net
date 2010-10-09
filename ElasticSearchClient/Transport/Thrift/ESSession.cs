using System;
using ElasticSearch.Exception;
using ElasticSearch.Thrift;

namespace ElasticSearch
{
	/// <summary>
	/// ElasticSearch session
	/// </summary>
	internal class ESSession : IDisposable
	{
		[ThreadStatic] private static ESSession _current;

		private IConnection _connection;
		private bool _disposed;

		public ESSession(ConnectionBuilder connectionBuilder)
			: this(ConnectionProviderFactory.Get(connectionBuilder))
		{
		}

		public ESSession(IConnectionProvider connectionProvider)
		{
			if (Current != null)
				throw new ElasticSearchException("Cannot create a new session while there is one already active.");
			ConnectionProvider = connectionProvider;
			Current = this;
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