using Newtonsoft.Json;

namespace ElasticSearch.Client.EMO
{
	[JsonObject("index")]
	public class IndexSetting
	{
		[JsonProperty("number_of_shards")]
		public int NumberOfShards { set; get; }

		[JsonProperty("number_of_replicas")]
		public int NumberOfReplicas { set; get; }

		public IndexSetting(int shards,int replicas)
		{
			NumberOfReplicas = replicas;
			NumberOfShards = shards;
		}
	}
}