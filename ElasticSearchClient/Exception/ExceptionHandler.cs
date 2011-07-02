using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Exception
{
	internal static class ExceptionHandler
	{
		static LogWrapper _logger = new LogWrapper("Exception");
		public static void HandleExceptionResponse(string response, string formatedMessage)
		{
			if (response != null)
			{
				if (response.IndexOf("IndexMissingException") > -1)
				{
					throw new IndexMissingException(formatedMessage);
				}
				if (response.IndexOf("TypeMissingException") > -1)
				{
					throw new TypeMissingException(formatedMessage);
				}
				if (response.IndexOf("SearchPhaseExecutionException") > -1)
				{
					throw new SearchPhaseExecutionException(formatedMessage);
				}
				_logger.HandleException(new ElasticSearchException(formatedMessage), "ElasticSearchException");
			}
			_logger.InfoFormat("{0}\r\n{1}", response, formatedMessage);
		}
	}
}