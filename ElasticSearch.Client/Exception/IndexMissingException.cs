namespace ElasticSearch.Client.Exception
{
	public class IndexMissingException:System.Exception
	{
		public IndexMissingException(string msg):base(msg)
		{
		}
	}
}