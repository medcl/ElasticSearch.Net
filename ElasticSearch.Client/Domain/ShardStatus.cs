using Newtonsoft.Json;

namespace ElasticSearch.Client.Domain
{
	public class ShardStatus
	{
		[JsonProperty("failed")] 
		public int Failed;
		[JsonProperty("successful")] 
		public int Successful;
		[JsonProperty("total")] 
		public int Total;
	}
}