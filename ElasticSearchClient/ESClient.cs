using System;
using System.Web;
using ElasticSearch.Transport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ElasticSearch
{
	/// <summary>
	/// ElasticSearch客户端
	/// </summary>
	public class ESClient
	{
		private static readonly ESClient _client = new ESClient();

		private ESClient()
		{
		}

		public static ESClient Instance
		{
			get { return _client; }
		}

		/// <summary>
		/// 添加索引
		/// </summary>
		/// <param name="index">索引名称</param>
		/// <param name="type">索引类型</param>
		/// <param name="indexKey">唯一key</param>
		/// <param name="jsonData">json文档数据</param>
		/// <example>Index('multenant','resume','9E86AF9F-B108-47F1-8F24-FCBAFCE727F6','{
		///   "__typeid": "2ed7f3b3-c1fc-4386-8866-86ccd2061475",   
		///  "__tenant": "100002",
		///  "__time": "202009201316",
		///  "Name": "张三",
		///  "HasWorkExperience": true,
		/// "Resume_Age": 45
		/// }');</example>
		public bool Index(string index, string type, string indexKey, string jsonData)
		{
			string url = "/" + index + "/" + type + "/" + indexKey;
			Response result = RestProvider.Instance.Post(url, jsonData);
			return result.Success;
		}

		/// <summary>
		///  删除索引
		/// </summary>
		/// <param name="index">索引名称</param>
		/// <param name="type">索引类型</param>
		/// <param name="indexKey">唯一Key</param>
		/// <example>Delete('multenant','resume','7C2DFA95-1ADA-4D4A-864D-C708F8F3A1E4');</example>
		public void Delete(string index, string type, string indexKey)
		{
			string url = "/" + index + "/" + type + "/" + indexKey;
			RestProvider.Instance.Delete(url);
		}

		/// <summary>
		/// 通过查询进行删除
		/// </summary>
		/// <param name="index"></param>
		/// <param name="type"></param>
		/// <param name="queryString">user:kimchy</param>
		/// <example>DeleteByQueryString('multenant','resume','user:kimchy');</example>
		public void DeleteByQueryString(string index, string type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/" + type + "/_query?q=" + queryString;
			RestProvider.Instance.Delete(url);
		}

		public void DeleteByQueryString(string index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/" + string.Join(",", type) + "/_query?q=" + queryString;
			RestProvider.Instance.Delete(url);
		}

		public void DeleteByQueryString(string[] index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + string.Join(",", index) + "/" + string.Join(",", type) + "/_query?q=" + queryString;
			RestProvider.Instance.Delete(url);
		}

		public void DeleteByQueryString(string index, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/_query?q=" + queryString;
			RestProvider.Instance.Delete(url);
		}

		public void DeleteByQueryString(string[] index, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + string.Join(",", index) + "/_query?q=" + queryString;
			RestProvider.Instance.Delete(url);
		}

		/// <summary>
		/// 谨慎使用，影响所有索引
		/// </summary>
		/// <param name="queryString"></param>
		internal void DeleteByQueryString(string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/_all/_query?q=" + queryString;
			RestProvider.Instance.Delete(url);
		}

		public int Count(string index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/" + string.Join(",", type) + "/_count?q=" + queryString;
			Response result = RestProvider.Instance.Get(url);

			JObject o = JObject.Parse(result.Result);
			var count = (int) o["count"];

			return count;
		}

		public int Count(string index, string type, string queryString)
		{
			return Count(index, new[] {type}, queryString);
		}

		public int Count(string index, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/_count?q=" + queryString;
			Response result = RestProvider.Instance.Get(url);

			JObject o = JObject.Parse(result.Result);
			var count = (int) o["count"];

			return count;
		}

		public int Count(string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/_count?q=" + queryString;
			Response result = RestProvider.Instance.Get(url);

			JObject o = JObject.Parse(result.Result);
			var count = (int) o["count"];

			return count;
		}

		public void DeleteByQueryDSL(string index, string type, string jsonData)
		{
		}


		public ESSearchResult Get(string index, string type, string indexKey)
		{
			string url = "/" + index + "/" + type + "/" + indexKey;
			Response result = RestProvider.Instance.Get(url);

			var hitResult = JsonConvert.DeserializeObject<ESSearchResult>(result.Result);
			if (hitResult == null)
			{
				hitResult = new ESSearchResult();
			}
			hitResult.JsonString = result.Result;
			return hitResult;
		}

		public ESSearchResult Search(string index, string[] type, string queryString, int limit)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = "/" + index + "/" + string.Join(",", type) + "/_search?q=" + queryString + "&size=" + limit;
			Response result = RestProvider.Instance.Get(url);

			ESSearchResult hitResult = null;
			if (result.Result != null)
			{
				hitResult = JsonConvert.DeserializeObject<ESSearchResult>(result.Result);
			}
			if (hitResult == null)
			{
				hitResult = new ESSearchResult();
			}
			hitResult.JsonString = result.Result;
			return hitResult;
		}

		public ESSearchResult Search(string index, string type, string queryString)
		{
			return Search(index, new[] {type}, queryString, 10);
		}

		public ESSearchResult Search(string index, string type, string queryString, int limit)
		{
			return Search(index, new[] {type}, queryString, limit);
		}

		public void Refresh(params string[] index)
		{
			string indexs = string.Empty;
			if (index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs + "/_refresh";

			RestProvider.Instance.Get(url);
		}

		public void Flush(params string[] index)
		{
			string indexs = string.Empty;
			if (index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs + "/_flush";

			RestProvider.Instance.Get(url);
		}

		public void Optimize(params string[] index)
		{
			string indexs = string.Empty;
			if (index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs + "/_optimize";

			RestProvider.Instance.Get(url);
		}

		public string Status(params string[] index)
		{
			string indexs = string.Empty;
			if (index.Length > 0)
			{
				indexs = "/" + string.Join(",", index);
			}
			string url = indexs + "/_status";

			return RestProvider.Instance.Get(url).Result;
		}

		public ESSearchResult SearchByQueryDSL(string indexCatalog, string indexType, string jsondata)
		{
			throw new NotImplementedException();
		}
	}
}