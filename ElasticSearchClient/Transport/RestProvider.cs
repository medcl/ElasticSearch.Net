using ElasticSearch.Client.Utils;
using ElasticSearch.Config;
using ElasticSearch.Thrift;
using ElasticSearch.Transport.Thrift;

namespace ElasticSearch.Transport
{
	internal class Response
	{
		public string Result;
		public bool Success;
	}

	internal interface IRestProvider
	{
		Response Process(string strUrl, string reqdata, string encoding, Method method);
	}

	internal class RestProvider : IRestProvider
	{
		private const string DefaultEncoding = "utf-8";
		private static LogWrapper _logger = LogWrapper.GetLogger();

		public static readonly RestProvider Instance = new RestProvider();

		private RestProvider()
		{
		}

		#region IRestProvider Members

		public Response Process(string strUrl, string reqdata, string encoding, Method method)
		{
			switch (ElasticSearchConfig.Instance.TransportType)
			{
				case TransportType.thrift:
					return ThriftAdaptor.Instance.Process(strUrl, reqdata, encoding, method);
				default:
					return HttpAdaptor.Instance.Process(strUrl, reqdata, encoding, method);
			}
		}

		#endregion

		public Response Post(string strUrl, string reqdata, string encoding)
		{
			return Process(strUrl, reqdata, encoding, Method.POST);
		}

		public Response Post(string strUrl, string reqdata)
		{
			return Process(strUrl, reqdata, DefaultEncoding, Method.POST);
		}

		public Response Put(string strUrl, string reqdata, string encoding)
		{
			return Process(strUrl, reqdata, encoding, Method.PUT);
		}

		public Response Put(string strUrl, string reqdata)
		{
			return Process(strUrl, reqdata, DefaultEncoding, Method.PUT);
		}

		public Response Delete(string url)
		{
			return Process(url, string.Empty, DefaultEncoding, Method.DELETE);
		}

		public Response Get(string url)
		{
			return Process(url, string.Empty, DefaultEncoding, Method.GET);
		}

		public Response Search(string url)
		{
			return Process(url, string.Empty, DefaultEncoding, Method.GET);
		}

		public Response Search(string url, string reqdata)
		{
			return Process(url, reqdata, DefaultEncoding, Method.POST);
		}

		public Response Search(string url, string reqdata, string encoding)
		{
			return Process(url, reqdata, encoding, Method.POST);
		}
	}
}