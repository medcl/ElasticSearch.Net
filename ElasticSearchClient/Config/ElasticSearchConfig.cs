using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using ElasticSearch.Client.Exception;
using Exortech.NetReflector;

namespace ElasticSearch.Client.Config
{
	[ReflectorType("ElasticSearchConfig")]
	public class ElasticSearchConfig
	{
		#region static method

		private static readonly string ConfigName;

		private static ElasticSearchConfig _instance;

		static ElasticSearchConfig()
		{
			#region get configFile

			ConfigName = "Config/ElasticSearch.config";
			string configFileValue = ConfigurationManager.AppSettings["ElasticSearchConfigFile"];

			if (!string.IsNullOrEmpty(configFileValue))
				ConfigName = configFileValue;

			#endregion

			#region load Config

			_instance = LoadConfig<ElasticSearchConfig>(ConfigName);

			#endregion
		}

		public static ElasticSearchConfig Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ElasticSearchConfig();
				}
				return _instance;
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
			ConnectionPool = new ConnectionPoolConfig();
		}

		[ReflectorCollection("Clusters", InstanceType = typeof(ClusterDefinition[]), Required = true)]
		public ClusterDefinition[] Clusters { get; set; }

		[ReflectorCollection("ConnectionPool", InstanceType = typeof(ConnectionPoolConfig), Required = false)]
		public ConnectionPoolConfig ConnectionPool { set; get; }
		private static T LoadConfig<T>(string xmlPath) where T : class
		{
			try
			{
				using (var xml = new XmlTextReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlPath)))
				{
					var config = NetReflector.Read(xml) as T;
					xml.Close();
					return config;
				}
			}
			catch (System.Exception exp)
			{
				throw new ElasticSearchException("Failed on loading config !", exp);
			}
		}


		public ClusterDefinition GetCluster(string clusterName)
		{
			if (Clusters != null) return Clusters.Where(var => var.ClusterName == clusterName).First();
			throw new ElasticSearchException("cluster not found");
		}
	}
}