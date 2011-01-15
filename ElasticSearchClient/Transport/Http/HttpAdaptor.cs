using System;
using System.IO;
using System.Net;
using System.Text;
using ElasticSearch.Client.Transport.IDL;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Transport.Http
{
	internal class HttpAdaptor : IRestProvider
	{
		public static readonly HttpAdaptor Instance = new HttpAdaptor();

		private static readonly LogWrapper _logger = LogWrapper.GetLogger();

		private HttpAdaptor()
		{
		}

		#region IRestProvider Members

		public RestResponse Process(string strUrl, string reqdata, string encoding, Method method)
		{
			DateTime start = DateTime.Now;

			var result = new RestResponse();
			string responseStr = string.Empty;
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
				responseStr = reader.ReadToEnd();
				result.SetBody(responseStr);
				result.Status = Status.OK;
				reader.Close();
				reader.Dispose();
				response.Close();
			}
			catch (WebException e)
			{
				result.Status = Status.INTERNAL_SERVER_ERROR;
				DateTime endtime = DateTime.Now;
				_logger.ErrorFormat("REQUEST> url: {0},body:{1},method,{2},encoding:{3},time:{5},\n\r RESPONSE>:{4}", strUrl, reqdata,
				                    method, encoding, responseStr, endtime - start);
				_logger.ErrorFormat(e, "Error happend while process the request");
			}
			return result;
		}

		#endregion
	}
}