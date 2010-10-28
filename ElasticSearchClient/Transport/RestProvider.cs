using ElasticSearch.Client.Transport.Thrift;
using ElasticSearch.Config;
using ElasticSearch.Thrift;
using ElasticSearch.Utils;

namespace ElasticSearch.Client.Transport
{
	internal interface IRestProvider
	{
		RestResponse Process(string strUrl, string reqdata, string encoding, Method method);
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

		public RestResponse Process(string strUrl, string reqdata, string encoding, Method method)
		{
			switch (ElasticSearchConfig.Instance.TransportType)
			{
				case TransportType.Thrift:
					return ThriftAdaptor.Instance.Process(strUrl, reqdata, encoding, method);
				default:
					return HttpAdaptor.Instance.Process(strUrl, reqdata, encoding, method);
			}
		}

		#endregion

		public RestResponse Post(string strUrl, string reqdata, string encoding)
		{
			return Process(strUrl, reqdata, encoding, Method.POST);
		}

		public RestResponse Post(string strUrl, string reqdata)
		{
			return Process(strUrl, reqdata, DefaultEncoding, Method.POST);
		}

		public RestResponse Put(string strUrl, string reqdata, string encoding)
		{
			return Process(strUrl, reqdata, encoding, Method.PUT);
		}

		public RestResponse Put(string strUrl, string reqdata)
		{
			return Process(strUrl, reqdata, DefaultEncoding, Method.PUT);
		}

		public RestResponse Delete(string url)
		{
			return Process(url, string.Empty, DefaultEncoding, Method.DELETE);
		}

		public RestResponse Get(string url)
		{
			return Process(url, string.Empty, DefaultEncoding, Method.GET);
		}

		public RestResponse Search(string url)
		{
			return Process(url, string.Empty, DefaultEncoding, Method.GET);
		}

		public RestResponse Search(string url, string reqdata)
		{
			return Process(url, reqdata, DefaultEncoding, Method.POST);
		}

		public RestResponse Search(string url, string reqdata, string encoding)
		{
			return Process(url, reqdata, encoding, Method.POST);
		}
	}
}