using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.EMO
{
	public class Hits
	{
		[JsonProperty("_source")] 
		public Dictionary<string, object> Fields = new Dictionary<string, object>();
		[JsonProperty("_id")] 
		public string Id;
		[JsonProperty("_index")] 
		public string Index;
		[JsonProperty("_score")] 
		public string Score;
		[JsonProperty("_type")] 
		public string Type;
		[JsonProperty("_version")]
		public int Version;
	}
}