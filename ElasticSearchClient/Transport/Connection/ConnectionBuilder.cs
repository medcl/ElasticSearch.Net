using System;
using System.Collections.Generic;
using System.Text;
using Thrift.Transport;

namespace ElasticSearch.Client
{
	internal class ConnectionBuilder
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="indexCatalog"></param>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="timeout">Milliseconds</param>
		public ConnectionBuilder(TSocketSettings socketSettings, string host, int port = 9200, bool isframed = false,
		                         int timeout = 0, bool pooled = false, int poolSize = 25, int lifetime = 0)
		{
			Servers = new List<Server> {new Server(host, port, isframed)};
			Timeout = timeout;
			Pooled = pooled;
			PoolSize = poolSize;
			Lifetime = lifetime;
			ConnectionString = GetConnectionString();
			TSocketSetting = socketSettings;
		}

		/// <summary>
		/// 
		/// </summary>
		public int Timeout { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public int PoolSize { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public int Lifetime { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public bool Pooled { get; private set; }

		public TSocketSettings TSocketSetting { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public IList<Server> Servers { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public string ConnectionString { get; private set; }

		private string GetConnectionString()
		{
			var b = new StringBuilder();
			string format = "{0}={1};";
			string serverStr = string.Empty;
			foreach (var server in Servers)
			{
				serverStr += server;
			}
			b.AppendFormat(format, "Server", serverStr);
			b.AppendFormat(format, "Timeout", Timeout);
			b.AppendFormat(format, "Pooled", Pooled);
			b.AppendFormat(format, "PoolSize", PoolSize);
			b.AppendFormat(format, "Lifetime", Lifetime);
			return b.ToString();
		}
	}
}