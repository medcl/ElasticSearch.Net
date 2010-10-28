using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client
{
	public class HitStatus
	{
		[JsonProperty("hits")] 
		public List<Hits> Hits = new List<Hits>();
		[JsonProperty("max_score")] 
		public double MaxScore;
		[JsonProperty("total")] 
		public int Total;
	}
}