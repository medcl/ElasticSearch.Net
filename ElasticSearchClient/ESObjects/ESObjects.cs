using System.Collections.Generic;

namespace ElasticSearch
{
	public class ShardStatus
	{
		public int failed;
		public int successful;
		public int total;
	}

	public class HitStatus
	{
		public List<Hits> hits = new List<Hits>();
		public double max_score;
		public int total;
	}

	public class Hits
	{
		public string _id;
		public string _index;
		public string _score;
		public Dictionary<string, object> _source = new Dictionary<string, object>();
		public string _type;
	}
}