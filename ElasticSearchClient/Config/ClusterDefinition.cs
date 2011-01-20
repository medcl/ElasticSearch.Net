using Exortech.NetReflector;

namespace ElasticSearch.Client.Config
{
	[ReflectorType("Cluster")]
	public class ClusterDefinition
	{
		public ClusterDefinition()
		{
			TransportType = TransportType.Thrift;
		}

		/// <summary>
		/// http or thrift
		/// </summary>
		[ReflectorProperty("TransportType", InstanceType = typeof(TransportType), Required = true)]
		public TransportType TransportType { get; set; }

		[ReflectorProperty("Name", InstanceType = typeof(string), Required = true)]
		public string ClusterName { get; set; }

		[ReflectorCollection("ThriftNodes", InstanceType = typeof(NodeDefinition[]), Required = false)]
		public NodeDefinition[] ThriftNodes { get; set; }

		[ReflectorCollection("HttpNodes", InstanceType = typeof(NodeDefinition[]), Required = false)]
		public NodeDefinition[] HttpNodes { get; set; }

	}
}