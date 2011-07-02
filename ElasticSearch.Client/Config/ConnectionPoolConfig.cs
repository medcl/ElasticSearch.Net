using Exortech.NetReflector;
using Thrift.Transport;

namespace ElasticSearch.Client.Config
{
	[ReflectorType("ConnectionPool")]
	public class ConnectionPoolConfig
	{
		public ConnectionPoolConfig()
		{
			PoolSize = 30;
			ConnectionLifetimeMinutes = 60;
			SocketSettings = TSocketSettings.DefaultSettings;
		}

		[ReflectorProperty("PoolSize", InstanceType = typeof(int), Required = true)]
		public int PoolSize { get; set; }

		[ReflectorProperty("LifetimeMinutes", InstanceType = typeof(int), Required = true)]
		public int ConnectionLifetimeMinutes { get; set; }

		[ReflectorProperty("SocketSettings", InstanceType = typeof(TSocketSettings), Required = false)]
		public TSocketSettings SocketSettings { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as ConnectionPoolConfig;
			return (other != null
			        && other.PoolSize == PoolSize
			        && other.ConnectionLifetimeMinutes == ConnectionLifetimeMinutes);
		}
	}
}