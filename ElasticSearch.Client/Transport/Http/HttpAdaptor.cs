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

                using (var webResponse = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        long contentLength = webResponse.ContentLength;
                        var bytes = new byte[contentLength];
                        bytes = ReadFully(stream);
                        stream.Close();
                        responseStr = Encoding.GetEncoding(encoding).GetString(bytes);
                        result.SetBody(responseStr);
                        result.Status = Status.OK;
                    }
                }

                DateTime endtime = DateTime.Now;
                _logger.InfoFormat("Request Success,Method:{2},Url:{0},Body:{1},Time:{3}", url, reqdata, method,
                                   endtime - start);
            }
            catch (WebException e)
            {
                DateTime endtime = DateTime.Now;
                if (e.Response != null)
                {
                    Stream stream = e.Response.GetResponseStream();
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
                            _logger.Error(exception);
                        }
                    }
                }
                string msg = string.Format("Method:{2}, Url: {0},Body:{1},Encoding:{3},Time:{5},Response:{4}", url,
                                           reqdata,
                                           method, encoding, responseStr, endtime - start);
                result.Status = Status.INTERNAL_SERVER_ERROR;
                ExceptionHandler.HandleExceptionResponse(responseStr, msg);
            }
            return result;
        }

        #endregion

        private static byte[] ReadFully(Stream stream)
        {
            var buffer = new byte[128];
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }
    }
}