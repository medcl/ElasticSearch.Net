using System.Collections.Generic;
using System.Text;
using System.Web;
using ElasticSearch.Client.Transport;
using ElasticSearch.DSL;
using ElasticSearch.Mapping;
using ElasticSearch.Thrift;
using ElasticSearch.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = ElasticSearch.Utils.JsonSerializer;

namespace ElasticSearch.Client
{
	/// <summary>
	/// ElasticSearchClient
	/// </summary>
	public class ElasticSearchClient
	{
		private static readonly ElasticSearchClient _client = new ElasticSearchClient();
		LogWrapper _logger= LogWrapper.GetLogger();
		private ElasticSearchClient()
		{
		}

		public static ElasticSearchClient Instance
		{
			get { return _client; }
		}

		public OperateResult Index(string index, IndexItem indexItem)
		{
			return Index(index, indexItem.IndexType, indexItem.IndexKey, indexItem.ToJson());
		}

		public OperateResult Index(string index, string type, string indexKey, string jsonData)
		{
			string url = "/" + index + "/" + type + "/" + indexKey;
			RestResponse result = RestProvider.Instance.Post(url, jsonData);

			return GetOperateResult(result);
		}

		public OperateResult Bulk(IList<BulkObject> bulkObjects)
		{
			string url = "/_bulk";
			var jsonData = bulkObjects.GetJson();
			RestResponse result = RestProvider.Instance.Post(url, jsonData);
			var operateResult= GetOperateResult(result);
			operateResult.Success = result.Status == Thrift.Status.OK;
			return operateResult;
		}

		public OperateResult Delete(string index, string type, string indexKey)
		{
			string url = "/" + index + "/" + type + "/" + indexKey;
			RestResponse result = RestProvider.Instance.Delete(url);

			return GetOperateResult(result);
		}

		public OperateResult Delete(string indexName, string indexType, string[] objectKeys)
		{
			string url = "/_bulk";
			var stringBuilder = new StringBuilder(objectKeys.Length);
			foreach (var variable in objectKeys)
			{
				stringBuilder.AppendLine("{{ \"delete\" : {{ \"_index\" : \"{0}\", \"_type\" : \"{1}\", \"_id\" : \"{2}\" }} }}".Fill(indexName.ToLower(), indexType, variable));
			}
			var jsonData = stringBuilder.ToString();
			RestResponse result = RestProvider.Instance.Post(url, jsonData);
			var operateResult = GetOperateResult(result);
			operateResult.Success = result.Status == Thrift.Status.OK;
			return operateResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="type"></param>
		/// <param name="queryString">user:kimchy</param>
		/// <example>DeleteByQueryString('multenant','resume','user:kimchy');</example>
		public OperateResult DeleteByQueryString(string index, string type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/" + type + "/_query?q=" + queryString;
			RestResponse result = RestProvider.Instance.Delete(url);

			return GetOperateResult(result);
		}

		public OperateResult DeleteByQueryString(string index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/" + string.Join(",", type) + "/_query?q=" + queryString;
			RestResponse result = RestProvider.Instance.Delete(url);

			return GetOperateResult(result);
		}

		private OperateResult GetOperateResult(RestResponse result)
		{
			var json = result.GetBody();
			OperateResult operateResult = JsonSerializer.Get<OperateResult>(json);
			operateResult.JsonString = json;
			return operateResult;
		}

		public OperateResult DeleteByQueryString(string[] index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + string.Join(",", index) + "/" + string.Join(",", type) + "/_query?q=" + queryString;
			RestResponse result = RestProvider.Instance.Delete(url);
			
			return GetOperateResult(result);
		}

		public OperateResult DeleteByQueryString(string index, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/_query?q=" + queryString;
			RestResponse result = RestProvider.Instance.Delete(url);

			return GetOperateResult(result);
		}

		public OperateResult DeleteByQueryString(string[] index, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + string.Join(",", index) + "/_query?q=" + queryString;
			RestResponse result = RestProvider.Instance.Delete(url);

			return GetOperateResult(result);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="queryString"></param>
		internal OperateResult DeleteByQueryString(string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/_all/_query?q=" + queryString;
			RestResponse result = RestProvider.Instance.Delete(url);

			return GetOperateResult(result);
		}

		public int Count(string index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/" + string.Join(",", type) + "/_count?q=" + queryString;
			RestResponse result = RestProvider.Instance.Get(url);
			
			var restr = result.GetBody();
			
			if (!string.IsNullOrEmpty(restr))
			{
				try
				{
					JObject o = JObject.Parse(restr);
					var count = (int) o["count"];

					return count;
				}
				catch (System.Exception e)
				{
					_logger.Error(e);
				}
			}
			return 0;
		}

		public int Count(string index, string type, string queryString)
		{
			return Count(index, new[] {type}, queryString);
		}

		public int Count(string index, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/_count?q=" + queryString;
			RestResponse result = RestProvider.Instance.Get(url);

			var restr = result.GetBody();

			if (!string.IsNullOrEmpty(restr))
			{
				try
				{
					JObject o = JObject.Parse(restr);
					var count = (int)o["count"];

					return count;
				}
				catch (System.Exception e)
				{
					_logger.Error(e);
				}
			}
			return 0;
		}

		public int Count(string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/_count?q=" + queryString;
			RestResponse result = RestProvider.Instance.Get(url);

			var restr = result.GetBody();

			if (!string.IsNullOrEmpty(restr))
			{
				try
				{
					JObject o = JObject.Parse(restr);
					var count = (int)o["count"];

					return count;
				}
				catch (System.Exception e)
				{
					_logger.Error(e);
				}
			}
			return 0;
		}

		public Document Get(string index, string type, string indexKey)
		{
			string url = "/" + index + "/" + type + "/" + indexKey;
			RestResponse result = RestProvider.Instance.Get(url);

			if (result.Body != null)
			{
				var document = new Document();
				document.JsonString = result.GetBody();
				try
				{
				var hitResult = JsonConvert.DeserializeObject<Hits>(result.GetBody());
				document.Hits = hitResult;
				}
				catch (System.Exception e)
				{
					_logger.Error(e);
				}
				
				return document;
			}
			return null;
		}
		
		public SearchResult Search(string index, string[] type, string queryString, int size)
		{
			return Search(index, type, queryString, 0, size);
		}

		public SearchResult Search(string index, string[] type, string queryString,int from, int size)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/" + string.Join(",", type) + "/_search?q=" + queryString +"&from="+from+ "&size=" + size;
			RestResponse result = RestProvider.Instance.Get(url);

			var hitResult = new SearchResult(result.GetBody());
			return hitResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="queryString"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public SearchResult Search(string index,string queryString,int from,int size)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/_search?q=" + queryString + "&from=" + from + "&size=" + size;
			RestResponse result = RestProvider.Instance.Get(url);

			var hitResult = new SearchResult(result.GetBody());
			return hitResult;
		}

		public SearchResult Search(string index, string type, string queryString)
		{
			return Search(index, new[] {type}, queryString, 10);
		}

		public SearchResult Search(string index, string type, string queryString,int from, int size)
		{
			return Search(index, new[] {type}, queryString,from,size);
		}

		public List<string> SearchIds(string index, string[] type, string queryString, string sortString, int from, int size)
		{
			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = "/{0}/{1}/_search?q={2}&fields=_id&from={3}&size={4}".Fill(index.ToLower(), string.Join(",", type),
																				 queryString, from, size);

			if (!string.IsNullOrEmpty(sortString))
			{
				url += "&sort=" + sortString;
			}

			RestResponse result = RestProvider.Instance.Get(url);

			var hitResult = new SearchResult(result.GetBody());
			return hitResult.GetHitIds();
		}

		public SearchResult Search(string index, string type, string queryString, int size)
		{
			return Search(index, new[] { type }, queryString, size);
		}

		public SearchResult SearchByDSL(string index, string[] type, string queryString,int from, int size)
		{
			QueryString query=new QueryString(queryString);
			
			ElasticQuery  elasticQuery=new ElasticQuery(from,size);
			elasticQuery.AddQuery(query);

			var jsonstr = JsonSerializer.Get(elasticQuery);
			
			string url = "/" + index + "/" + string.Join(",", type) + "/_search";
			RestResponse result = RestProvider.Instance.Post(url,jsonstr);
			var hitResult = new SearchResult(result.GetBody());
			return hitResult;
		}
		
		public OperateResult Refresh(params string[] index)
		{
			string indexs = string.Empty;
			if (index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs + "/_refresh";

			RestResponse result = RestProvider.Instance.Get(url);

			return GetOperateResult(result);
		}

		public OperateResult Flush(params string[] index)
		{
			string indexs = string.Empty;
			if (index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs + "/_flush";

			RestResponse result = RestProvider.Instance.Get(url);

			return GetOperateResult(result);
		}

		public OperateResult Optimize(params string[] index)
		{
			string indexs = string.Empty;
			if (index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs + "/_optimize";

			RestResponse result = RestProvider.Instance.Get(url);

			return GetOperateResult(result);
		}

		public string Status(params string[] index)
		{
			string indexs = string.Empty;
			if (index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs + "/_status";

			return RestProvider.Instance.Get(url).GetBody();
		}

		#region mapping operate
		public OperateResult PutMapping(string index,TypeSetting typeSetting)
		{
			var url = "/" + index +"/_mapping";

			var mappings = new Dictionary<string, TypeSetting>();
			mappings.Add(typeSetting.Type, typeSetting);

			var data = JsonSerializer.Get(mappings);

			var response = RestProvider.Instance.Put(url, data);

			if (response != null)
			{
				try
				{
				var operateResult = JsonConvert.DeserializeObject<OperateResult>(response.GetBody());
					
					if(response.Status==Thrift.Status.INTERNAL_SERVER_ERROR)
					{
						//auto create index
						CreateIndex(index, new IndexSetting(5, 1));
						//try again
						response = RestProvider.Instance.Put(url, data);

						return GetOperateResult(response);
					}
				}
				catch (System.Exception e)
				{
					_logger.Error(e);
				}
				
			}
			return GetOperateResult(response);
		}
		#endregion

		public OperateResult CreateIndex(string index, IndexSetting indexSetting)
		{
			string url = "/" + index  + "/";

			var json = JsonSerializer.Get(indexSetting);
			json ="{    index : "+json+" }";

			RestResponse result = RestProvider.Instance.Post(url,json);

			var jsonString = result.GetBody();
			var hitResult = JsonConvert.DeserializeObject<OperateResult>(jsonString);
			hitResult.JsonString = jsonString;

			return hitResult; 
		}
		public OperateResult ModifyIndex(string index, IndexSetting indexSetting)
		{
			string url = "/" + index + "/_settings";

			var json = JsonSerializer.Get(indexSetting);
			json = "{    index : " + json + " }";

			RestResponse result = RestProvider.Instance.Put(url, json);
			var jsonString = result.GetBody();
			var hitResult  = JsonConvert.DeserializeObject<OperateResult>(jsonString);
			hitResult.JsonString = result.GetBody();

			return hitResult; 
		}
	
		public OperateResult DeleteIndex(string index)
		{
			string url = "/" + index ;

			RestResponse result = RestProvider.Instance.Delete(url);

			var jsonString = result.GetBody();
			var hitResult = JsonConvert.DeserializeObject<OperateResult>(jsonString);
			hitResult.JsonString = jsonString;
			return hitResult; 
		}
	
	}
}