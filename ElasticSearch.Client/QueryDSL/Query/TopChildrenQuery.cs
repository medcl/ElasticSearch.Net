using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// The top_children query runs the child query with an estimated hits size, and out of the hit docs, aggregates it into parent docs. If there aren¡¯t enough parent docs matching the requested from/size search request, then it is run again with a wider (more hits) search.
	/// </summary>
	[JsonObject("top_children")]
	[JsonConverter(typeof(TopChildrenQueryConverterer))]
	public class TopChildrenQuery:IQuery
	{
		public string Type;
		public IQuery Query;
		/// <summary>
		/// The top_children also provide scoring capabilities, with the ability to specify max, sum or avg as the score type
		/// </summary>
		public string Score;
		/// <summary>
		/// How many hits are asked for in the first child query run is controlled using the factor parameter (defaults to 5). For example, when asking for 10 docs with from 0, then the child query will execute with 50 hits expected. If not enough parents are found (in our example, 10), and there are still more child docs to query, then the search hits are expanded my multiplying by the incremental_factor (defaults to 2).
		/// </summary>
		public int Factor=5;
		public int IncrementalFactor=2;
		public string Scope;

		public TopChildrenQuery(string type, IQuery query, string scope = null)
		{
			Type = type;
			Query = query;
			Scope = scope;
		}

	}
}