using System;
using System.Collections.Generic;
using ElasticSearch.Utils;
using Newtonsoft.Json;

namespace ElasticSearch.Client
{
	/// <summary>
	/// search result
	/// </summary>
	public class SearchResult
	{
		private SearchHits _hits;
		
		[JsonIgnore]
		private bool _isNotcalled = true;
		
		[JsonIgnore]
		public string JsonString { set; get; }

		[JsonIgnore] 
		private LogWrapper _logger = LogWrapper.GetLogger();

		public HitStatus GetHits()
		{
			if (_hits == null && _isNotcalled)
			{
				_isNotcalled = false;
				try
				{
				var temp = JsonConvert.DeserializeObject<SearchHits>(JsonString);
				if (temp != null)
				{
					_hits = new SearchHits();
					_hits.Hits = temp.Hits;
					return _hits.Hits;
				}
				}
				catch (System.Exception e)
				{
					_logger.ErrorFormat(e,"Json:{0}",JsonString);
				}
				
			}
			if (_hits != null) return _hits.Hits;
			return new HitStatus();
		}

		public int GetTotalCount()
		{
			return GetHits().Total;
		}

		public SortedList<string, Dictionary<string, object>> GetFields()
		{
			var result = new SortedList<string, Dictionary<string, object>>();
			HitStatus hitStatus = GetHits();
			if (hitStatus != null)
			{
				foreach (Hits hit in hitStatus.Hits)
				{
					var dict = new Dictionary<string, object>();
					foreach (var fileItem in hit.Fields)
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
						result.Add(hit.Id, dict);
					}
				}
			}

			return result;
		}
	}
}