using Newtonsoft.Json;

namespace ElasticSearch.Client.EMO
{
	/// <summary>
	/// 含多个结果的搜索结果
	/// </summary>
	public class SearchHits
	{
		[JsonProperty("hits")] 
		public HitStatus Hits;
		[JsonIgnore] [JsonProperty("_shards")] 
		public ShardStatus ShardStatus;
	}
}