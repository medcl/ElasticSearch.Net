using Exortech.NetReflector;

namespace ElasticSearch.Client.Config
{
	[ReflectorType("Node")]
	public class NodeDefinition
	{
		public NodeDefinition()
		{
			Enabled = true;
			Port = 9500;
			DangerZoneThreshold = 5;
			DangerZoneSeconds = 30;
			EnablePool = true;
			TimeOut = 120;
			IsFramed = false;
		}

		[ReflectorProperty("Enabled", InstanceType = typeof(bool), Required = false)]
		public bool Enabled { get; set; }

		[ReflectorProperty("Host", InstanceType = typeof(string), Required = true)]
		public string Host { get; set; }

		[ReflectorProperty("Port", InstanceType = typeof(int), Required = true)]
		public int Port { get; set; }

		[ReflectorProperty("TimeOut", InstanceType = typeof(int), Required = false)]
		public int TimeOut { get; set; }

		[ReflectorProperty("IsFramed", InstanceType = typeof(bool), Required = false)]
		public bool IsFramed { get; set; }

		[ReflectorProperty("DangerZoneThreshold", InstanceType = typeof(int), Required = false)]
		public int DangerZoneThreshold { get; set; }

		[ReflectorProperty("DangerZoneSeconds", InstanceType = typeof(int), Required = false)]
		public int DangerZoneSeconds { get; set; }

		[ReflectorProperty("EnablePool", InstanceType = typeof(bool), Required = false)]
		public bool EnablePool { get; set; }

		[ReflectorProperty("ConnectionPool", InstanceType = typeof(ConnectionPoolConfig), Required = false)]
		public ConnectionPoolConfig ConnectionPool { get; set; }

		public override string ToString()
		{
			return Host + ":" + Port;
		}
	}
}