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
    
    [JsonObject("index")]
	public class TemplateIndexSetting
	{
		[JsonProperty("index.number_of_shards")]
		public int NumberOfShards { set; get; }

        [JsonProperty("index.number_of_replicas")]
		public int NumberOfReplicas { set; get; }

        public TemplateIndexSetting(int shards, int replicas)
		{
			NumberOfReplicas = replicas;
			NumberOfShards = shards;
		}
	}
}