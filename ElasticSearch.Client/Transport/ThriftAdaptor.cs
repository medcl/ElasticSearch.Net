using System.Collections.Generic;
using System.Text;
using ElasticSearch.Client.Exception;
using ElasticSearch.Client.Transport.IDL;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Transport.Thrift
{
	internal class ThriftAdaptor : IRestProvider
	{
		public static LogWrapper _logger = LogWrapper.GetLogger();
		public static readonly ThriftAdaptor Instance = new ThriftAdaptor();

		private ThriftAdaptor()
		{
		}

		#region IRestProvider Members

		public RestResponse Process(string clusterName,string strUrl, string reqdata, string encoding, Method method)
		{
			ESNode node = ESNodeManager.Instance.GetThriftNode(clusterName);
			using (var esSession = new ESSession(node.ConnectionProvider))
			{
				var restRequest = new RestRequest();
				restRequest.Method = method;
				restRequest.Uri = strUrl;

				if (!string.IsNullOrEmpty(reqdata))
				{
					restRequest.Body = Encoding.UTF8.GetBytes(reqdata);
				}

				//				restRequest.Parameters = new Dictionary<string, string>();
				//				restRequest.Parameters.Add("pretty", "true");
				restRequest.Headers = new Dictionary<string, string>();
				restRequest.Headers.Add("Content-Type", "application/json");
				//				restRequest.Headers.Add("charset", encoding);

				RestResponse response = esSession.GetClient().execute(restRequest);
				if (response.Status != Status.OK && response.Status != Status.CREATED)
				{
					var formatedMessage = string.Format("Method:{5},Status:{0},Url:{1}{2},ReqData{3},Response:{4}",
								  response.Status, esSession.CurrentServer, strUrl, reqdata,
								  response.GetBody(), method);


					ExceptionHandler.HandleExceptionResponse(response.GetBody(), formatedMessage);



				}
				return response;
			}
		}

		#endregion

		public string DecodeStr(byte[] bytes)
		{
			if (bytes != null && bytes.Length > 0)
			{
				string str = Encoding.UTF8.GetString(bytes);

				if (string.IsNullOrEmpty(str))
				{
					_logger.ErrorFormat("occured when deserialize the response.bytes:{0}", bytes);
				}
				return str;
			}

			return string.Empty;
		}
	}
}