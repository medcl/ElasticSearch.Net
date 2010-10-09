using System;
using System.IO;
using System.Net;
using System.Text;
using ElasticSearch.Client.Utils;
using ElasticSearch.Thrift;
using ElasticSearch.Transport;

namespace ElasticSearch
{
	internal class HttpAdaptor : IRestProvider
	{
		public static readonly HttpAdaptor Instance = new HttpAdaptor();

		private static readonly LogWrapper _logger = LogWrapper.GetLogger();

		private HttpAdaptor()
		{
		}

		#region IRestProvider Members

		public Response Process(string strUrl, string reqdata, string encoding, Method method)
		{
			DateTime start = DateTime.Now;

			var result = new Response();

			try
			{
				if (!strUrl.StartsWith("/"))
				{
					strUrl = "/" + strUrl;
				}
				WebRequest request = WebRequest.Create(ESNodeManager.Instance.GetHttpNode() + strUrl);
				request.Method = method.ToString();
				byte[] buf = Encoding.GetEncoding(encoding).GetBytes(reqdata);
				request.ContentType = "application/json; charset=" + encoding;
				request.ContentLength = buf.Length;

				if (method != Method.GET || reqdata.Length > 0)
				{
					Stream s = request.GetRequestStream();
					s.Write(buf, 0, buf.Length);
					s.Close();
				}

				WebResponse response = request.GetResponse();
				var reader = new StreamReader(response.GetResponseStream(),
				                              Encoding.GetEncoding(encoding));

				result.Result = reader.ReadToEnd();
				result.Success = true;
				reader.Close();
				reader.Dispose();
				response.Close();
			}
			catch (WebException e)
			{
				DateTime endtime = DateTime.Now;
				_logger.ErrorFormat("REQUEST> url: {0},body:{1},method,{2},encoding:{3},time:{5},\nRESPONSE>{4}", strUrl, reqdata,
				                    method, encoding, result.Result, endtime - start);
				_logger.HandleException(e, "Error happend while process the request");
			}
			return result;
		}

		#endregion
	}
}