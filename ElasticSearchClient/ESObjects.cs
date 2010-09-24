using System.Collections.Generic;

namespace ElasticSearchClient
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

	public class SearchResult
	{
		public string JsonString;
		public ShardStatus _shards;
		public HitStatus hits;

		public SortedList<string, Dictionary<string, object>> GetFields()
		{
			var result = new SortedList<string, Dictionary<string, object>>();

			if (hits != null)
			{
				foreach (Hits hit in hits.hits)
				{
					var dict = new Dictionary<string, object>();
					foreach (var fileItem in hit._source)
					{
						if (dict.ContainsKey(fileItem.Key))
						{
							object value = dict[fileItem.Key];
							value = value + "," + fileItem.Value;
							dict[fileItem.Key] = value;
						}
						else
						{
							dict.Add(fileItem.Key, fileItem.Value);
						}
					}
					if (dict.Count > 0)
					{
						result.Add(hit._id, dict);
					}
				}
			}

			return result;
		}
	}
}