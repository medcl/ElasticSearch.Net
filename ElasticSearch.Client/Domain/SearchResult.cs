using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json;
using JsonSerializer = ElasticSearch.Client.Utils.JsonSerializer;

namespace ElasticSearch.Client.Domain
{
	/// <summary>
	/// search result
	/// </summary>
	public class SearchResult
	{
		private HitStatus _hits;

		public SearchResult(string jsonResult)
		{
			Response = jsonResult;
		}

        public SearchResult(string uri,string response)
        {
            URI = uri;
            Response = response;
        }
        public SearchResult(string uri,string query, string response)
        {
            URI = uri;
            Query = query;
            Response = response;
        }

		[JsonIgnore]
		private bool _isNotcalled = true;
		
		[JsonIgnore]
		public string Response { set; get; }

        [JsonIgnore]
        public string URI { set; get; }       
        [JsonIgnore]
        public string Query { set; get; }

		[JsonIgnore] 
		private LogWrapper _logger = LogWrapper.GetLogger();

		public HitStatus GetHits()
		{
			if (_hits == null && _isNotcalled)
			{
				_isNotcalled = false;
				try
				{
					if (!string.IsNullOrEmpty(Response))
					{
						var temp = JsonSerializer.Get<SearchHits>(Response);
						if (temp != null && temp.Hits != null)
						{
							_hits = temp.Hits;
						}
					}
				}
				catch (System.Exception e)
				{
					_logger.Error(Response, e);
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
					foreach (var fileItem in hit.Source)
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

	    internal Dictionary<string, Dictionary<string, int>> _facets;
        internal Dictionary<string, string> _hightlights;

	    /// <summary>
        /// Facets统计信息
        /// </summary>
        [IgnoreDataMember]
        public Dictionary<string, Dictionary<string, int>> Facets
	    {
	        get
	        {
                if(_facets==null)
                {
                    this.InitOrGetFacets();
                }
	            return _facets;
	        }
	        set { _facets = value; }
	    } 
        


	}
}