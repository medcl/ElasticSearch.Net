using System.Collections.Generic;

namespace ElasticSearch
{
	public class ESSearchResult
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