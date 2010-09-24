using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ElasticSearchClient
{
	/// <summary>
	/// ElasticSearchClient
	/// </summary>
	public class ElasticSearchClient
	{
		private static readonly ElasticSearchClient _client = new ElasticSearchClient();

		private ElasticSearchClient()
		{
		}

		public static ElasticSearchClient Instance
		{
			get { return _client; }
		}

		/// <summary>
		/// AddIndex
		/// </summary>
		/// <example>Index('multenant','resume','9E86AF9F-B108-47F1-8F24-FCBAFCE727F6','{
		///   "__typeid": "2ed7f3b3-c1fc-4386-8866-86ccd2061475",   
		///  "__tenant": "100002",
		///  "__time": "202009201316",
		///  "Name": "test",
		///  "HasWorkExperience": true,
		/// "Age": 45
		/// }');</example>
		public void Index(string index, string type, string indexKey, string jsonData)
		{
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + index + "/" + type + "/" + indexKey;
			RESTfulHelper.Post(url, jsonData);
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
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + index + "/" + type + "/" + indexKey;
			RESTfulHelper.Delete(url);
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
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + index + "/" + type + "/_query?q=" + queryString;
			RESTfulHelper.Delete(url);
		}

		public void DeleteByQueryString(string index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + index + "/" + string.Join(",", type) + "/_query?q=" +
			             queryString;
			RESTfulHelper.Delete(url);
		}

		public void DeleteByQueryString(string[] index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + string.Join(",", index) + "/" + string.Join(",", type) +
			             "/_query?q=" + queryString;
			RESTfulHelper.Delete(url);
		}

		public void DeleteByQueryString(string index, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + index + "/_query?q=" + queryString;
			RESTfulHelper.Delete(url);
		}

		public void DeleteByQueryString(string[] index, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + string.Join(",", index) + "/_query?q=" + queryString;
			RESTfulHelper.Delete(url);
		}

		/// <summary>
		/// 谨慎使用，影响所有索引
		/// </summary>
		/// <param name="queryString"></param>
		internal void DeleteByQueryString(string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = ElasticSearchConfig.ElasticSearchHost + "/_all/_query?q=" + queryString;
			RESTfulHelper.Delete(url);
		}

		public int Count(string index, string[] type, string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + index + "/" + string.Join(",", type) + "/_count?q=" +
			             queryString;
			string result = RESTfulHelper.Get(url);

			JObject o = JObject.Parse(result);
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
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + index + "/_count?q=" + queryString;
			string result = RESTfulHelper.Get(url);

			JObject o = JObject.Parse(result);
			var count = (int) o["count"];

			return count;
		}

		public int Count(string queryString)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = ElasticSearchConfig.ElasticSearchHost + "/_count?q=" + queryString;
			string result = RESTfulHelper.Get(url);

			JObject o = JObject.Parse(result);
			var count = (int) o["count"];

			return count;
		}

		public void DeleteByQueryDSL(string index, string type, string jsonData)
		{
		}

		public SearchResult Search(string index, string[] type, string queryString, int limit)
		{
			queryString = HttpUtility.UrlEncode(queryString).Trim();
			string url = ElasticSearchConfig.ElasticSearchHost + "/" + index + "/" + string.Join(",", type) + "/_search?q=" +
			             queryString + "&size=" + limit;
			string result = RESTfulHelper.Get(url);

			var hitResult = JsonConvert.DeserializeObject<SearchResult>(result);
			if (hitResult == null)
			{
				hitResult = new SearchResult();
			}
			hitResult.JsonString = result;
			return hitResult;
		}

		public SearchResult Search(string index, string type, string queryString)
		{
			return Search(index, new[] {type}, queryString, 10);
		}

		public SearchResult Search(string index, string type, string queryString, int limit)
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
			string url = ElasticSearchConfig.ElasticSearchHost + indexs + "/_refresh";

			RESTfulHelper.Get(url);
		}
	}
}