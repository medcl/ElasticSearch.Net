using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearch.Utils;
using Newtonsoft.Json;
using JsonSerializer = ElasticSearch.Utils.JsonSerializer;

namespace ElasticSearch.Client
{
	/// <summary>
	/// search result
	/// </summary>
	public class SearchResult
	{
		private HitStatus _hits;

		public SearchResult(string jsonResult)
		{
			JsonString = jsonResult;
		}

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
					if (!string.IsNullOrEmpty(JsonString))
					{
						var temp = JsonSerializer.Get<SearchHits>(JsonString);
						if (temp != null && temp.Hits != null)
						{
							_hits = temp.Hits;
						}
					}
				}
				catch (System.Exception e)
				{
					_logger.Error(JsonString, e);
				}

			}
			if (_hits != null) return _hits;
			return new HitStatus();
		}

		public List<string> GetHitIds()
		{
			var temp = GetHits();
			if (temp.Hits.Count > 0)
			{
				List<Hits> hits = temp.Hits;
				IEnumerable<string> dhit = from hit in hits
										   select hit.Id;
				return new List<string>(dhit);
			}
			return new List<string>();
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