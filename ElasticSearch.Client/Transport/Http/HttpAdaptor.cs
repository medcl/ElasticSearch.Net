using System;
using System.IO;
using System.Net;
using System.Text;
using ElasticSearch.Client.Exception;
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

		public RestResponse Process(string clusterName, string strUrl, string reqdata, string encoding, Method method)
		{
			DateTime start = DateTime.Now;

			var result = new RestResponse();
			string responseStr = string.Empty;
			string url = null;
			try
			{
				if (!strUrl.StartsWith("/"))
				{
					strUrl = "/" + strUrl;
				}
				url = ESNodeManager.Instance.GetHttpNode(clusterName) + strUrl;
				WebRequest request = WebRequest.Create(url);
				request.Method = method.ToString();
				if (reqdata != null)
				{
					byte[] buf = Encoding.GetEncoding(encoding).GetBytes(reqdata);
					request.ContentType = "application/json; charset=" + encoding;
					request.ContentLength = buf.Length;

					if (method != Method.GET || reqdata.Length > 0)
					{
						Stream s = request.GetRequestStream();
						s.Write(buf, 0, buf.Length);
						s.Close();
					}
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

				DateTime endtime = DateTime.Now;
				_logger.InfoFormat("Request Success,Method:{2},Url:{0},Body:{1},Time:{3}", url, reqdata, method, endtime - start);

			}
			catch (WebException e)
			{
				DateTime endtime = DateTime.Now;
				if (e.Response != null)
				{
					var stream = e.Response.GetResponseStream();
					if (stream != null)
					{
					    try
					    {
					        var reader = new StreamReader(stream);
					        responseStr = reader.ReadToEnd();
					        result.SetBody(responseStr);
					    }
					    catch (System.Exception exception)
					    {
//					        Console.WriteLine(exception);
                            _logger.Error(exception);
					    }
					}
				}
				var msg = string.Format("Method:{2}, Url: {0},Body:{1},Encoding:{3},Time:{5},Response:{4}", url,
										reqdata,
										method, encoding, responseStr, endtime - start);
				result.Status = Status.INTERNAL_SERVER_ERROR;
				ExceptionHandler.HandleExceptionResponse(responseStr, msg);
			}
			return result;
		}

		#endregion
	}
}