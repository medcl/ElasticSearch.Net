using System;
using System.Configuration;
using System.IO;
using System.Xml;
using ElasticSearch.Exception;
using Exortech.NetReflector;
using Thrift.Transport;

namespace ElasticSearch.Config
{
	[ReflectorType("ElasticSearchConfig")]
	public class ElasticSearchConfig
	{
		#region static method

		private static readonly string ConfigName;

		private static ElasticSearchConfig instance;

		static ElasticSearchConfig()
		{
			#region get configFile

			ConfigName = "Config/ElasticSearchConfig.config";
			string configFileValue = ConfigurationManager.AppSettings["ElasticSearchConfigFile"];

			if (!string.IsNullOrEmpty(configFileValue))
				ConfigName = configFileValue;

			#endregion

			#region load Config

			instance = LoadConfig<ElasticSearchConfig>(ConfigName);

			#endregion
		}

		public static ElasticSearchConfig Instance
		{
			get
			{
				if (instance == null)
				{
					instance = LoadConfig<ElasticSearchConfig>(ConfigName);
				}
				return instance;
			}
		}

		public static event EventHandler ConfigChanged;

		public static void OnConfigChanged()
		{
			if (ConfigChanged != null)
				ConfigChanged(Instance, EventArgs.Empty);
		}

		#endregion

		public ElasticSearchConfig()
		{
			TransportType = TransportType.thrift;
		}

		/// <summary>
		/// http or thrift
		/// </summary>
		[ReflectorProperty("TransportType", InstanceType = typeof (string), Required = true)]
		public TransportType TransportType { set; get; }

		[ReflectorCollection("ThriftNodes", InstanceType = typeof (NodeDefinition[]), Required = false)]
		public NodeDefinition[] ThriftNodes { get; set; }

		[ReflectorCollection("HttpNodes", InstanceType = typeof (NodeDefinition[]), Required = false)]
		public NodeDefinition[] HttpNodes { get; set; }

		private static T LoadConfig<T>(string xmlPath) where T : class
		{
			try
			{
				using (var xml = new XmlTextReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlPath)))
				{
					var actionUnionIcon = NetReflector.Read(xml) as T;
					xml.Close();
					return actionUnionIcon;
				}
			}
			catch (System.Exception exp)
			{
				throw new ElasticSearchException("Failed to loading config !", exp);
			}
		}

		public bool ContainsNode(NodeDefinition[] nodes, string host, int port)
		{
			if (nodes == null || nodes.Length == 0)
				return false;

			foreach (NodeDefinition node in nodes)
			{
				if (node.Host == host && node.Port == port)
					return true;
			}
			return false;
		}
	}

	public enum TransportType
	{
		http,
		thrift
	}

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
			ConnectionPool = new ConnectionPoolConfig();
			SocketSettings = TSocketSettings.DefaultSettings;
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

		[ReflectorProperty("SocketSettings", InstanceType = typeof(TSocketSettings), Required = false)]
		public TSocketSettings SocketSettings { get; set; }

		public override string ToString()
		{
			return Host + ":" + Port;
		}
	}

	[ReflectorType("ConnectionPool")]
	public class ConnectionPoolConfig
	{
		public ConnectionPoolConfig()
		{
			PoolSize = 10;
			ConnectionLifetimeMinutes = 60;
		}

		[ReflectorProperty("PoolSize", InstanceType = typeof(int), Required = true)]
		public int PoolSize { get; set; }

		[ReflectorProperty("LifetimeMinutes", InstanceType = typeof(int), Required = true)]
		public int ConnectionLifetimeMinutes { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as ConnectionPoolConfig;
			return (other != null
					&& other.PoolSize == PoolSize
					&& other.ConnectionLifetimeMinutes == ConnectionLifetimeMinutes);
		}
	}
}