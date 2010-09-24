using System.IO;
using System.Net;
using System.Text;

namespace ElasticSearchClient
{
	internal class RESTfulHelper
	{
//		static LogWrapper  _logger=new LogWrapper();
		private const string DefaultEncoding = "utf-8";

		public static string Post(string strUrl, string reqdata, string encoding)
		{
			return Request(strUrl, reqdata, encoding, Method.POST);
		}

		public static string Post(string strUrl, string reqdata)
		{
			return Request(strUrl, reqdata, DefaultEncoding, Method.POST);
		}

		public static string Put(string strUrl, string reqdata, string encoding)
		{
			return Request(strUrl, reqdata, encoding, Method.PUT);
		}

		public static string Put(string strUrl, string reqdata)
		{
			return Request(strUrl, reqdata, DefaultEncoding, Method.PUT);
		}

		private static string Request(string strUrl, string reqdata, string encoding, Method method)
		{
//			DateTime start = DateTime.Now;
			string val;
			try
			{
				WebRequest request = WebRequest.Create("" + strUrl + "");
				request.Method = method.ToString();
				byte[] buf = Encoding.GetEncoding(encoding).GetBytes(reqdata);
				request.ContentType = "application/json; charset=UTF-8";
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
				val = reader.ReadToEnd();
				reader.Close();
				reader.Dispose();
				response.Close();
			}
			catch (WebException e)
			{
//				_logger.HandleException(e,"请求出现异常");
				val = string.Empty;
			}
//			DateTime endtime = DateTime.Now;
//			_logger.DebugFormat("REQUEST> url: {0},body:{1},method,{2},encoding:{3},time:{5},\nRESPONSE>{4}", strUrl, reqdata, method, encoding,val,endtime-start);
			return val;
		}

		public static string Delete(string url)
		{
			return Request(url, string.Empty, DefaultEncoding, Method.DELETE);
		}

		public static string Get(string url)
		{
			return Request(url, string.Empty, DefaultEncoding, Method.GET);
		}

		public static string Search(string url)
		{
			return Request(url, string.Empty, DefaultEncoding, Method.SEARCH);
		}

		public static string Search(string url, string reqdata)
		{
			return Request(url, reqdata, DefaultEncoding, Method.SEARCH);
		}

		public static string Search(string url, string reqdata, string encoding)
		{
			return Request(url, reqdata, encoding, Method.SEARCH);
		}

		#region Nested type: Method

		private enum Method
		{
			/// <summary>
			///修改
			/// </summary>
			PUT,
			/// <summary>
			/// 创建
			/// </summary>
			POST,
			/// <summary>
			/// 获取
			/// </summary>
			GET,
			/// <summary>
			/// 删除
			/// </summary>
			DELETE,
			/// <summary>
			/// 搜索
			/// </summary>
			SEARCH,
		}

		#endregion
	}
}