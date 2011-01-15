using Newtonsoft.Json;

namespace ElasticSearch.Client.Admin
{
	public class ClusterState
	{
		[JsonProperty("cluster_name")]
		public string ClusterName;

		[JsonProperty("master_node")] 
		public string MasterNode;

		
		


	}
}