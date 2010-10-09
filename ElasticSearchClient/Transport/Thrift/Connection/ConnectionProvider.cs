namespace ElasticSearch
{
	internal abstract class ConnectionProvider : IConnectionProvider
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		protected ConnectionProvider(ConnectionBuilder builder)
		{
			Builder = builder;
		}

		#region IConnectionProvider Members

		/// <summary>
		/// 
		/// </summary>
		public ConnectionBuilder Builder { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public abstract IConnection CreateConnection();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual IConnection Open()
		{
			IConnection conn = CreateConnection();
			conn.Open();
			return conn;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		public virtual bool Close(IConnection connection)
		{
			if (connection.IsOpen)
				connection.Dispose();

			return true;
		}

		#endregion
	}
}