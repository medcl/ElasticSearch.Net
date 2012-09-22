namespace ElasticSearch.Client.Exception
{
	public class SearchPhaseExecutionException : System.Exception
	{
		public SearchPhaseExecutionException(string formatedMessage)
			: base(formatedMessage)
		{

		}
	}
}