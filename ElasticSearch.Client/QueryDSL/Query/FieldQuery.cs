using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// A query that executes a query string against a specific field. It is a simplified version of query_string query (by setting the default_field to the field this query executed against)
	/// </summary>
	[JsonObject("field")]
	[JsonConverter(typeof(FieldQueryConverter))]
	public class FieldQuery :IQuery
	{
		public string Field;
		public string QueryString;
		public float Boost;
		public bool EnablePositionIncrements;

		public FieldQuery(string field,string queryString)
		{
			Field = field;
			QueryString = queryString;
		}
	}
}