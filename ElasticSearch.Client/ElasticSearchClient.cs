using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Web;
using ElasticSearch.Client.Admin;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Domain;
using ElasticSearch.Client.Mapping;
using ElasticSearch.Client.QueryDSL;
using ElasticSearch.Client.QueryString;
using ElasticSearch.Client.Transport;
using ElasticSearch.Client.Transport.IDL;
using ElasticSearch.Client.Utils;
using Newtonsoft.Json.Linq;

namespace ElasticSearch.Client
{
	/// <summary>
	/// ElasticSearch.Client
	/// license:Apache2
	/// author:medcl
	/// url:http://log.medcl.net
	/// </summary>
	public class ElasticSearchClient
	{
		private readonly LogWrapper _logger = LogWrapper.GetLogger();
		private readonly RestProvider _provider;

		public ElasticSearchClient(string clusterName)
		{
			_provider = new RestProvider(clusterName);
		}

		public ElasticSearchClient(string host, int port, TransportType transportType,bool isframed=false)
		{
			string cluster = string.Format("{0}:{1}", host, port);
			ESNodeManager.Instance.BuildCustomNodes(cluster, host, port, transportType,isframed);
			_provider = new RestProvider(cluster);
		}

		#region index

		public OperateResult Index(string index, IndexItem indexItem)
		{
			return Index(index, indexItem.IndexType, indexItem.IndexKey, indexItem.FieldsToJson(), indexItem.ParentKey);
		}

		public OperateResult Index(string index, IEnumerable<IndexItem> indexItems)
		{
			IList<BulkObject> bulkObject = new List<BulkObject>();
			foreach (IndexItem indexItem in indexItems)
			{
				bulkObject.Add(new BulkObject(index, indexItem.IndexType, indexItem.IndexKey, indexItem.FieldsToJson(),
				                              indexItem.ParentKey));
			}
			return Bulk(bulkObject);
		}

		public OperateResult Index(string index, string type, string indexKey, Dictionary<string, object> dictionary,
		                           string parentKey = null)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(type));
			Contract.Assert(!string.IsNullOrEmpty(indexKey));
			string jsonData = JsonSerializer.Get(dictionary);
			Contract.Assert(!string.IsNullOrEmpty(jsonData));

			return Index(index, type, indexKey, jsonData, parentKey);
		}

		public OperateResult Index(string index, string type, string indexKey, string jsonData, string parentKey = null)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(type));
			Contract.Assert(!string.IsNullOrEmpty(jsonData));
//			Contract.Assert(!string.IsNullOrEmpty(indexKey));

			string url = "/{0}/{1}/{2}".Fill(index.Trim().ToLower(), type.Trim(), indexKey);
			//set parent-child relation
			if (!string.IsNullOrEmpty(parentKey))
			{
				url = url + string.Format("?parent={0}", parentKey);
			}
			RestResponse result = _provider.Post(url, jsonData);
			return GetOperationResult(result);
		}


		public OperateResult Bulk(IList<BulkObject> bulkObjects)
		{
			Contract.Assert(bulkObjects != null);
			Contract.Assert(bulkObjects.Count > 0);

			const string url = "/_bulk";
			string jsonData = bulkObjects.GetJson();
			RestResponse result = _provider.Post(url, jsonData);
			OperateResult result1 = GetOperationResult(result);
			result1.Success = result.Status == Transport.IDL.Status.OK;
			return result1;
		}

		#endregion


        public bool PartialUpdate(string index, string type, string indexKey, string jsonData, string routing = null)
        {
            Contract.Assert(!string.IsNullOrEmpty(index));
            Contract.Assert(!string.IsNullOrEmpty(type));
            Contract.Assert(!string.IsNullOrEmpty(jsonData));
            Contract.Assert(!string.IsNullOrEmpty(indexKey));

            var url = "/{0}/{1}/{2}/_partial_update".Fill(index.Trim().ToLower(), type.Trim(), indexKey);

            //set parent-child relation
            if (!string.IsNullOrEmpty(routing))
            {
                url = url + string.Format("?routing={0}", routing);
            }

            RestResponse result = _provider.Post(url, jsonData);
            return result.Status == Transport.IDL.Status.OK || result.Status == Transport.IDL.Status.CREATED;
        }


		public Document Get(string index, string type, string indexKey, string routing = null)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(type));
			Contract.Assert(!string.IsNullOrEmpty(indexKey));

			string url = "/{0}/{1}/{2}".Fill(index.ToLower(), type, indexKey);
			if (!string.IsNullOrEmpty(routing))
			{
				url = url + string.Format("?routing={0}", routing);
			}

			RestResponse result = _provider.Get(url);
			
			if (result.Body != null&&(int)result.Status<400)
			{
				var document = new Document();
				document.JsonString = result.GetBody();
				try
				{
					var hitResult = JsonSerializer.Get<Hits>(result.GetBody());
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

		public OperateResult Delete(string indexName, string indexType, string[] objectKeys, string routing = null)
		{
			Contract.Assert(!string.IsNullOrEmpty(indexName));
			Contract.Assert(!string.IsNullOrEmpty(indexType));
			Contract.Assert(objectKeys != null);
			Contract.Assert(objectKeys.Length > 0);

			string url = "/_bulk";
			var stringBuilder = new StringBuilder(objectKeys.Length);
			foreach (string variable in objectKeys)
			{
				stringBuilder.Append(
					"{{ \"delete\" : {{ \"_index\" : \"{0}\", \"_type\" : \"{1}\", \"_id\" : \"{2}\"".Fill(indexName.ToLower(),
																											  indexType, variable));
				if (!string.IsNullOrEmpty(routing))
				{
					stringBuilder.Append(string.Format(", \"routing\" : \"{0}\"", routing));
				}
				stringBuilder.Append(" }}");
				stringBuilder.Append("\n");
			}
			string jsonData = stringBuilder.ToString();
			RestResponse result = _provider.Post(url, jsonData);
			OperateResult result1 = GetOperationResult(result);
			result1.Success = result.Status == Transport.IDL.Status.OK;
			return result1;
		}


		public OperateResult Delete(string indexName, string indexType, List<KeyValuePair<string, string>> keyParentPairs)
		{
			Contract.Assert(!string.IsNullOrEmpty(indexName));
			Contract.Assert(!string.IsNullOrEmpty(indexType));
			Contract.Assert(keyParentPairs != null);
			Contract.Assert(keyParentPairs.Count > 0);

			string url = "/_bulk";
			var stringBuilder = new StringBuilder(keyParentPairs.Count);
			foreach (var variable in keyParentPairs)
			{

				stringBuilder.Append(
					"{{ \"delete\" : {{ \"_index\" : \"{0}\", \"_type\" : \"{1}\", \"_id\" : \"{2}\", \"routing\" : \"{3}\" }} }}".Fill(indexName.ToLower(),
																											  indexType, variable.Key, variable.Value));
				stringBuilder.Append("\n");
			}
			string jsonData = stringBuilder.ToString();
			RestResponse result = _provider.Post(url, jsonData);
			OperateResult result1 = GetOperationResult(result);
			result1.Success = result.Status == Transport.IDL.Status.OK;
			return result1;
		}

		public List<string> GetIndices()
		{
			ClusterIndexStatus status = Status("");
			var result = new List<string>();
			Dictionary<string, IndexStatus>.KeyCollection e = status.IndexStatus.Keys;
			foreach (string variable in e)
			{
				result.Add(variable);
			}
			return result;
		}

		public DocStatus GetIndexDocStatus(string index)
		{
			string url = "/{0}/_status".Fill(index);
			RestResponse response = _provider.Get(url);
			JObject jObject = JObject.Parse(response.GetBody());
			var indexDocStatus = JsonSerializer.Get<DocStatus>(jObject["indices"][index]["docs"].ToString());
			return indexDocStatus;
		}

		public SearchResult Search(string index, string[] type, Conditional conditional, int from, int size)
		{
			return Search(index, type, conditional.Query, from, size);
		}

		public SearchResult Search(string index, string[] type, ExpressionEx expression, int from, int size)
		{
			return Search(index, type, Conditional.Get(expression), from, size);
		}

		public int Count(string index, string type, Conditional conditional)
		{
			return Count(index, type, conditional.Query);
		}

		public int Count(string index, string type, ExpressionEx expression)
		{
			return Count(index, type, Conditional.Get(expression).Query);
		}

		public string Analyze(string indexName, string analyzer, string text, string format)
		{
			string url = "/{0}/_analyze?analyzer={1}&format={2}".Fill(indexName.ToLower(), analyzer, format);
			string data = text;
			RestResponse result = _provider.Post(url, data);
			return result.GetBody();
		}

		#region search

		public SearchResult Search(string index, string[] type, string queryString, int size)
		{
			return Search(index, type, queryString, 0, size);
		}

		public SearchResult Search(string index, string[] type, string queryString, int from, int size)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(queryString));
			Contract.Assert(from >= 0);
			Contract.Assert(size > 0);
			Contract.Assert(queryString != null, "queryString != null");

			queryString = HttpUtility.UrlEncode(queryString.Trim());

			string url = string.Empty;

			if (type == null || type.Length == 0)
			{
				url = "/{0}/_search?q={1}&from={2}&size={3}".Fill(index.ToLower(), queryString, from,
				                                                  size);
			}
			else
			{
				url = "/{0}/{1}/_search?q={2}&from={3}&size={4}".Fill(index.ToLower(), string.Join(",", type), queryString, from,
				                                                      size);
			}
			RestResponse result = _provider.Get(url);
			var hitResult = new SearchResult(url,result.GetBody());
			return hitResult;
		}

		/// <summary>
		/// 搜索index下所有type
		/// </summary>
		/// <param name="index"></param>
		/// <param name="queryString"></param>
		/// <param name="from"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public SearchResult Search(string index, string queryString, int from, int size)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(queryString));
			Contract.Assert(from >= 0);
			Contract.Assert(size > 0);

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = "/{0}/_search?q={1}&from={2}&size={3}".Fill(index.ToLower(), queryString, from, size);
			RestResponse result = _provider.Get(url);

			var hitResult = new SearchResult(url,result.GetBody());
			return hitResult;
		}
        
        //TODO,remove this method
        public SearchResult Search(string url)
        {
            RestResponse result = _provider.Get(url);

            var hitResult = new SearchResult(url, result.GetBody());
            return hitResult; 
        }

		public SearchResult Search(string index, string type, string queryString)
		{
			string[] types = null;
			if (!string.IsNullOrEmpty(type))
			{
				types = new[] {type};
			}

			return Search(index, types, queryString, 10);
		}

		public SearchResult Search(string index, string type, string queryString, int from, int size)
		{
			string[] types = null;
			if (!string.IsNullOrEmpty(type))
			{
				types = new[] {type};
			}

			return Search(index, types, queryString, from, size);
		}

		public SearchResult Search(string index, string queryString, int from, int size, string sortString)
		{
			return Search(index, new string[] {}, queryString, sortString, from, size);
		}

		public SearchResult Search(string index, string type, string queryString, string sortString, int from, int size)
		{
			string[] types = null;
			if (!string.IsNullOrEmpty(type))
			{
				types = new[] {type};
			}
			return Search(index, types, queryString, sortString, from, size);
		}

		public SearchResult Search(string index, string[] type, string queryString, string sortString, int from, int size)
		{
			return Search(index, type, queryString, sortString, null, from, size);
		}

		public SearchResult Search(string index, string[] type, string queryString, string sortString, string[] fields,
		                           int from, int size)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = string.Empty;

			if (type == null || type.Length == 0)
			{
				url = "/{0}/_search?q={1}&from={2}&size={3}".Fill(index.ToLower(), queryString, from,
				                                                  size);
			}
			else
			{
				url = "/{0}/{1}/_search?q={2}&from={3}&size={4}".Fill(index.ToLower(), string.Join(",", type), queryString, from,
				                                                      size);
			}

			if (!string.IsNullOrEmpty(sortString))
			{
				url += "&sort=" + sortString;
			}

			if (fields != null && fields.Length > 0)
			{
				url += "&fields=" + string.Join(",", fields);
			}

			RestResponse result = _provider.Get(url);

			var hitResult = new SearchResult(url,result.GetBody());
			return hitResult;
		}

		public List<string> SearchIds(string index, string type, string queryString, string sortString, int from, int size)
		{
			string[] types = null;
			if (!string.IsNullOrEmpty(type))
			{
				types = new[] {type};
			}
			return SearchIds(index, types, queryString, sortString, from, size);
		}

		public List<string> SearchIds(string index, string[] type, string queryString, string sortString, int from, int size)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());

			string url = string.Empty;

			if (type == null || type.Length == 0)
			{
				url = "/{0}/_search?q={1}&from={2}&size={3}".Fill(index.ToLower(), queryString, from,
				                                                  size);
			}
			else
			{
				url = "/{0}/{1}/_search?q={2}&fields=_id&from={3}&size={4}".Fill(index.ToLower(), string.Join(",", type),
				                                                                 queryString, from, size);
			}

			if (!string.IsNullOrEmpty(sortString))
			{
				url += "&sort=" + sortString;
			}

			RestResponse result = _provider.Get(url);

			var hitResult = new SearchResult(url,result.GetBody());
			return hitResult.GetHitIds();
		}

		public List<string> SearchIds(string index, string[] type, Conditional conditional, string sortString, int from,
		                              int size)
		{
			return SearchIds(index, type, conditional.Query, sortString, from, size);
		}

		public SearchResult Search(string index, string type, string queryString, int size)
		{
			string[] types = null;
			if (!string.IsNullOrEmpty(type))
			{
				types = new[] {type};
			}
			return Search(index, types, queryString, size);
		}

		#endregion

		#region admin

		public OperateResult Refresh(params string[] index)
		{
			string indexs = string.Empty;
			if (index != null && index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs.ToLower() + "/_refresh";

			RestResponse result = _provider.Get(url);
			return GetOperationResult(result);
		}

		public OperateResult Flush(params string[] index)
		{
			string indexs = string.Empty;
			if (index != null && index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs.ToLower() + "/_flush";

			RestResponse result = _provider.Get(url);
			return GetOperationResult(result);
		}

		public OperateResult Optimize(params string[] index)
		{
			string indexs = string.Empty;
			if (index != null && index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs.ToLower() + "/_optimize";

			RestResponse result = _provider.Get(url);
			return GetOperationResult(result);
		}

		public ClusterIndexStatus Status(params string[] index)
		{
			string indexs = string.Empty;
			if (index != null && index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs.ToLower() + "/_status";

			string json = _provider.Get(url).GetBody();

			return JsonSerializer.Get<ClusterIndexStatus>(json);
		}

		#endregion

		#region mapping

		public OperateResult PutMapping(string index, TypeSetting typeSetting)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(typeSetting != null);
			string url = "/{0}/{1}/_mapping".Fill(index.ToLower(), typeSetting.TypeName);

			var mappings = new Dictionary<string, TypeSetting>();
			mappings.Add(typeSetting.TypeName, typeSetting);

			string data = JsonSerializer.Get(mappings);

			RestResponse response = _provider.Put(url, data);

			if (response != null)
			{
				try
				{
					if (response.Status == Transport.IDL.Status.INTERNAL_SERVER_ERROR ||
					    response.Status == Transport.IDL.Status.BAD_REQUEST)
					{
						//auto create index
						CreateIndex(index, new IndexSetting(5, 1));
						//try again
						response = _provider.Put(url, data);

						return GetOperationResult(response);
					}
				}
				catch (System.Exception e)
				{
					_logger.Error(e);
				}
			}
			return GetOperationResult(response);
		}

		public OperateResult PutMapping(string index, string type, string typeJson)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(typeJson != null);
			string url = "/{0}/{1}/_mapping".Fill(index.ToLower(), type);

			RestResponse response = _provider.Put(url, typeJson);

			if (response != null)
			{
				try
				{
					if (response.Status == Transport.IDL.Status.INTERNAL_SERVER_ERROR ||
					    response.Status == Transport.IDL.Status.BAD_REQUEST)
					{
						//auto create index
						CreateIndex(index, new IndexSetting(5, 1));
						//try again
						response = _provider.Put(url, typeJson);

						return GetOperationResult(response);
					}
				}
				catch (System.Exception e)
				{
					_logger.Error(e);
				}
			}
			return GetOperationResult(response);
		}


		public string GetMapping(string index, params string[] types)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			string url;

			if (types == null || types.Length == 0)
			{
				url = "/{0}/_mapping".Fill(index.ToLower());
			}
			else
			{
				url = "/{0}/{1}/_mapping".Fill(index.ToLower(), string.Join(",", types));
			}
			RestResponse result = _provider.Get(url);
			return result.GetBody();
		}

		#endregion

		#region indexAdmin

		public OperateResult CreateIndex(string index)
		{
			return CreateIndex(index, new IndexSetting(5, 1));
		}

		public OperateResult CreateIndex(string index, IndexSetting indexSetting)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(indexSetting != null);

			string url = "/" + index.ToLower() + "/";

			string json = JsonSerializer.Get(indexSetting);
			json = "{    index : " + json + " }";

			RestResponse result = _provider.Post(url, json);
			return GetOperationResult(result);
		}

		public OperateResult ModifyIndex(string index, IndexSetting indexSetting)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(indexSetting != null);

			string url = "/" + index.ToLower() + "/_settings";

			string json = "{{\"number_of_replicas\" : {0}}}".Fill(indexSetting.NumberOfReplicas);

			RestResponse result = _provider.Put(url, json);
			return GetOperationResult(result);
		}

		public OperateResult DeleteIndex(string index)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));

			string url = "/{0}".Fill(index.ToLower());

			RestResponse result = _provider.Delete(url);

			return GetOperationResult(result);
		}

		public OperateResult CreateTemplate(string templateName, TemplateSetting template)
		{
			Contract.Assert(template != null);

			string url = "/_template/{0}".Fill(templateName);

			string json = JsonSerializer.Get(template);
			RestResponse result = _provider.Post(url, json);

			return GetOperationResult(result);
		}

		public Dictionary<string, TemplateSetting> GetTemplate(string templateName)
		{
			Contract.Assert(!string.IsNullOrEmpty(templateName));

			string url = "/_template/{0}".Fill(templateName);

			RestResponse result = _provider.Get(url);

			if (result.Body != null)
			{
				var document = new Document();
				document.JsonString = result.GetBody();
				try
				{
					return JsonSerializer.Get<Dictionary<string, TemplateSetting>>(result.GetBody());
				}
				catch (System.Exception e)
				{
					_logger.Error(e);
				}
			}
			return null;
		}

		public OperateResult DeleteTemplate(string templateName)
		{
			Contract.Assert(!string.IsNullOrEmpty(templateName));
			Contract.Assert(templateName != null);

			string url = "/_template/{0}".Fill(templateName);
			RestResponse result = _provider.Delete(url);

			return GetOperationResult(result);
		}

		public OperateResult OpenIndex(string index)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			string url = "/{0}/_open".Fill(index);
			RestResponse result = _provider.Post(url,null);
			return GetOperationResult(result);
		}

		public OperateResult CloseIndex(string index)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			string url = "/{0}/_close".Fill(index);
			RestResponse result = _provider.Post(url, null);
			return GetOperationResult(result);
		}

		#endregion

		#region delete

		public OperateResult Delete(string index, string type, string indexKey, string routing)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(type));
			Contract.Assert(!string.IsNullOrEmpty(indexKey));


			string url = "/{0}/{1}/{2}?routing={3}".Fill(index.ToLower(), type, indexKey, routing);
			RestResponse result = _provider.Delete(url);
			return GetOperationResult(result);
		}

		public OperateResult Delete(string index, string type, string indexKey)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(type));
			Contract.Assert(!string.IsNullOrEmpty(indexKey));


			string url = "/{0}/{1}/{2}/".Fill(index.ToLower(), type, indexKey);
			RestResponse result = _provider.Delete(url);
			return GetOperationResult(result);
		}

		/// <summary>
		/// DeleteByQuery
		/// </summary>
		/// <param name="index"></param>
		/// <param name="type"></param>
		/// <param name="queryString">user:kimchy</param>
		/// <example>DeleteByQueryString('multenant','resume','user:kimchy');</example>
		public OperateResult DeleteByQueryString(string index, string type, string queryString)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(type));
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = "/{0}/{1}/_query?q={2}".Fill(index.ToLower(), type, queryString);
			RestResponse result = _provider.Delete(url);

			return GetOperationResult(result);
		}

		private OperateResult GetOperationResult(RestResponse result)
		{
			string jsonString = result.GetBody();
			if (!string.IsNullOrEmpty(jsonString))
			{
				var hitResult = JsonSerializer.Get<OperateResult>(jsonString);
				hitResult.JsonString = jsonString;
				return hitResult;
			}
			return new OperateResult();
		}

		public OperateResult DeleteByQueryString(string index, string[] type, string queryString)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url;

			if (type == null || type.Length == 0)
			{
				url = "/{0}/_query?q={1}".Fill(index.ToLower(), queryString);
			}
			else
			{
				url = "/{0}/{1}/_query?q={2}".Fill(index.ToLower(), string.Join(",", type), queryString);
			}
			RestResponse result = _provider.Delete(url);
			string jsonString = result.GetBody();
			if (jsonString != null)
			{
				var hitResult = JsonSerializer.Get<OperateResult>(jsonString);
				hitResult.JsonString = jsonString;
				return hitResult;
			}
			return null;
		}

		public OperateResult DeleteByQueryString(string[] index, string[] type, string queryString)
		{
			Contract.Assert(index != null);
			Contract.Assert(index.Length > 0);
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url;

			if (type == null || type.Length == 0)
			{
				url = "/{0}/_query?q={1}".Fill(string.Join(",", index).ToLower(), queryString);
			}
			else
			{
				url = "/{0}/{1}/_query?q=".Fill(string.Join(",", index).ToLower(), string.Join(",", type), queryString);
			}
			RestResponse result = _provider.Delete(url);
			string jsonString = result.GetBody();
			if (jsonString != null)
			{
				var hitResult = JsonSerializer.Get<OperateResult>(jsonString);
				hitResult.JsonString = jsonString;
				return hitResult;
			}
			return null;
		}

		public OperateResult DeleteByQueryString(string index, string queryString)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = "/{0}/_query?q=".Fill(index.ToLower(), queryString);
			RestResponse result = _provider.Delete(url);
			string jsonString = result.GetBody();
			if (jsonString != null)
			{
				var hitResult = JsonSerializer.Get<OperateResult>(jsonString);
				hitResult.JsonString = jsonString;
				return hitResult;
			}
			return null;
		}

		public OperateResult DeleteByQueryString(string[] index, string queryString)
		{
			Contract.Assert(index != null);
			Contract.Assert(index.Length > 0);
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = "/{0}/_query?q={1}".Fill(string.Join(",", index).ToLower(), queryString);
			RestResponse result = _provider.Delete(url);
			string jsonString = result.GetBody();
			if (jsonString != null)
			{
				var hitResult = JsonSerializer.Get<OperateResult>(jsonString);
				hitResult.JsonString = jsonString;
				return hitResult;
			}
			return null;
		}

		/// <summary>
		///be careful,influence all indicies
		/// </summary>
		/// <param name="queryString"></param>
		internal OperateResult DeleteByQueryString(string queryString)
		{
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = "/_all/_query?q={0}".Fill(queryString);
			RestResponse result = _provider.Delete(url);
			string jsonString = result.GetBody();
			if (jsonString != null)
			{
				var hitResult = JsonSerializer.Get<OperateResult>(jsonString);
				hitResult.JsonString = jsonString;
				return hitResult;
			}
			return null;
		}

		#endregion

		#region count

		public int Count(string index, string[] type, string queryString)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url;

			if (type == null || type.Length == 0)
			{
				url = "/{0}/_count?q={1}".Fill(index.ToLower(), queryString);
			}
			else
			{
				url = "/{0}/{1}/_count?q={2}".Fill(index.ToLower(), string.Join(",", type), queryString);
			}
			RestResponse result = _provider.Get(url);

			string restr = result.GetBody();

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
			string[] types = null;
			if (!string.IsNullOrEmpty(type))
			{
				types = new[] {type};
			}
			return Count(index, types, queryString);
		}

		public int Count(string index, string queryString)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = "/{0}/_count?q={1}".Fill(index.ToLower(), queryString);
			RestResponse result = _provider.Get(url);

			string restr = result.GetBody();

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

		public int Count(string queryString)
		{
			Contract.Assert(!string.IsNullOrEmpty(queryString));

			queryString = HttpUtility.UrlEncode(queryString.Trim());
			string url = "/_count?q={0}".Fill(queryString);
			RestResponse result = _provider.Get(url);

			string restr = result.GetBody();

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

		#endregion

		#region queryDSL

		public SearchResult Search(string index, string[] type, ElasticQuery elasticQuery)
		{
			string jsonstr = JsonSerializer.Get(elasticQuery);

			string url;

			if (type == null || type.Length == 0)
			{
				url = "/{0}/_search".Fill(index.ToLower());
			}
			else
			{
				url = "/{0}/{1}/_search".Fill(index.ToLower(), string.Join(",", type));
			}
			RestResponse result = _provider.Post(url, jsonstr);
			var hitResult = new SearchResult(url,jsonstr,result.GetBody());
			return hitResult;
		}

        public SearchResult Search(string index, ElasticQuery elasticQuery)
        {
            return Search(index, new string[]{}, elasticQuery);
        }
		public SearchResult Search(string index, string type, ElasticQuery elasticQuery)
		{
			string[] temp = null;
			if (type != null) temp = new[] { type };
			return Search(index, temp, elasticQuery);
		}

		public SearchResult Search(string index, string type, IQuery query,SortItem sortItem, int from = 0, int size = 5, string[] fields = null)
		{
			string[] temp = null;
			if (type != null) temp = new[] { type };
			return Search(index, temp, query,sortItem, from, size, fields);
		}

		public SearchResult Search(string index, string type, IQuery query,int from = 0, int size = 5)
		{
			string[] temp = null;
			if (type != null) temp = new[] {type};
			return Search(index, temp, query, null, from, size, null);
		}

		public SearchResult Search(string index, string[] type, IQuery query,SortItem sortItem, int from, int size, string[] fields = null)
		{
			Contract.Assert(!string.IsNullOrEmpty(index));
			Contract.Assert(query != null);
			Contract.Assert(from >= 0);
			Contract.Assert(size > 0);

			var elasticQuery = new ElasticQuery(from, size);
			elasticQuery.SetQuery(query);
			if (sortItem != null)
			{
				elasticQuery.AddSortItem(sortItem);
			}
			if (fields != null) elasticQuery.AddFields(fields);

			return Search(index, type, elasticQuery);
		}

		#endregion
	}
}