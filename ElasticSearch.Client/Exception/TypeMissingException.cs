namespace ElasticSearch.Client.Exception
{
	public class TypeMissingException : System.Exception
	{
		public TypeMissingException(string formatedMessage)
			: base(formatedMessage)
		{

		}
	}
}