using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Transport.Http;
using ElasticSearch.Client.Transport.IDL;
using ElasticSearch.Client.Transport.Thrift;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Transport
{
	internal interface IRestProvider
	{
		RestResponse Process(string cluster,string strUrl, string reqdata, string encoding, Method method);
	}

	internal class RestProvider : IRestProvider
	{
		private const string DefaultEncoding = "utf-8";
	    private readonly string _clusterName;

		public RestProvider(string clusterName)
		{
			_clusterName = clusterName;
		}

		#region IRestProvider Members

		public RestResponse Process(string clusterName,string strUrl, string reqdata, string encoding, Method method)
		{
			var transportType= ESNodeManager.Instance.GetClusterType(clusterName);
			switch (transportType)
			{
				case TransportType.Thrift:
					return ThriftAdaptor.Instance.Process(clusterName, strUrl, reqdata, encoding, method);
				default:
					return HttpAdaptor.Instance.Process(clusterName,strUrl, reqdata, encoding, method);
			}
		}

		#endregion

		public RestResponse Post(string strUrl, string reqdata, string encoding)
		{
			return Process(_clusterName,strUrl, reqdata, encoding, Method.POST);
		}

		public RestResponse Post(string strUrl, string reqdata)
		{
			return Process(_clusterName, strUrl, reqdata, DefaultEncoding, Method.POST);
		}

		public RestResponse Put(string strUrl, string reqdata, string encoding)
		{
			return Process(_clusterName, strUrl, reqdata, encoding, Method.PUT);
		}

		public RestResponse Put(string strUrl, string reqdata)
		{
			return Process(_clusterName, strUrl, reqdata, DefaultEncoding, Method.PUT);
		}

		public RestResponse Delete(string url)
		{
			return Process(_clusterName, url, string.Empty, DefaultEncoding, Method.DELETE);
		}

		public RestResponse Get(string url)
		{
			return Process(_clusterName, url, string.Empty, DefaultEncoding, Method.GET);
		}

		public RestResponse Search(string url)
		{
			return Process(_clusterName, url, string.Empty, DefaultEncoding, Method.GET);
		}

		public RestResponse Search(string url, string reqdata)
		{
			return Process(_clusterName, url, reqdata, DefaultEncoding, Method.POST);
		}

		public RestResponse Search(string url, string reqdata, string encoding)
		{
			return Process(_clusterName, url, reqdata, encoding, Method.POST);
		}
	}
}