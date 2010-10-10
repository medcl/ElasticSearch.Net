using System.Collections.Generic;
using System.Text;
using ElasticSearch.Exception;
using ElasticSearch.Thrift;
using ElasticSearch.Transport;

namespace ElasticSearch.Transport.Thrift
{
	internal class ThriftAdaptor : IRestProvider
	{
		public static readonly ThriftAdaptor Instance = new ThriftAdaptor();

		private ThriftAdaptor()
		{
		}

		#region IRestProvider Members

		public Response Process(string strUrl, string reqdata, string encoding, Method method)
		{
			var result = new Response();
			ESNode node = ESNodeManager.Instance.GetNode();
			using (var esSession = new ESSession(node.ConnectionProvider))
			{
				var restRequest = new RestRequest();
				restRequest.Method = method;
				restRequest.Uri = strUrl;
				restRequest.Body = Encoding.UTF8.GetBytes(reqdata);
//				restRequest.Parameters = new Dictionary<string, string>();
//				restRequest.Parameters.Add("pretty", "true");
				restRequest.Headers = new Dictionary<string, string>();
				restRequest.Headers.Add("Content-Type", "application/json");
//				restRequest.Headers.Add("charset", encoding);

				RestResponse response = esSession.GetClient().execute(restRequest);
				string responstr = Encoding.UTF8.GetString(response.Body);

				if (responstr.Length == 0)
				{
					throw new ElasticSearchException("occured when deserialize the response.body");
				}

				result.Result = responstr;
				if (response.Status == Status.OK)
				{
					result.Success = true;
				}
				return result;
			}
		}

		#endregion
	}
}