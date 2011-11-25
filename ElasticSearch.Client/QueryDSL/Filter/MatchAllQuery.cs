using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{

	/// <summary>
	/// A query that matches all documents. Maps to Lucene MatchAllDocsQuery
	/// </summary>
	[JsonObject("match_all")]
	[JsonConverter(typeof(MatchAllQueryConverter))]
	public class MatchAllQuery : IQuery
	{
		public float Boost;
		/// <summary>
		/// When indexing, a boost value can either be associated on the document level, or per field. The match all query does not take boosting into account by default. In order to take boosting into account, the norms_field needs to be provided in order to explicitly specify which field the boosting will be done on (Note, this will result in slower execution time)
		/// </summary>
		public string NormsField;

	}
}