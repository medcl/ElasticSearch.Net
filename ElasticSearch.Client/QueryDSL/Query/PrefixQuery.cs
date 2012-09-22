using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Matches documents that have fields containing terms with a specified prefix (not analyzed). The prefix query maps to Lucene PrefixQuery
	/// </summary>
	[JsonObject("prefix")]
	[JsonConverter(typeof(PrefixQueryConverter))]
	public class PrefixQuery:IQuery
	{
		public float Boost;
		public string Field;
		public string Value;

		public PrefixQuery(string field,string value)
		{
			Field = field;
			Value = value;
		}
	}
}
