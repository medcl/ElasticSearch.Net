using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Nested query allows to query nested objects / docs (see nested mapping). The query is executed against the nested objects / docs as if they were indexed as separate docs (they are, internally) and resulting in the root parent doc (or parent nested mapping).
	/// </summary>
	[JsonConverter(typeof(NestedQueryConverterer))]
	[JsonObject("wildcard")]
	public class NestedQuery:IQuery
	{
		/// <summary>
		/// The score_mode allows to set how inner children matching affects scoring of parent. It defaults to avg, but can be total, max and none
		/// </summary>
		public string ScoreMode;
		/// <summary>
		/// The query path points to the nested object path, and the query (or filter) includes the query that will run on the nested docs matching the direct path, and joining with the root parent docs.
		/// </summary>
		public string Path;
		public IQuery Query;
		public IFilter Filter;

		public NestedQuery(string path, IQuery query)
		{
			Path = path;
			Query = query;
		}

		public NestedQuery(string path, IFilter filter)
        {
            Path = path;
            Filter = filter;
        }

	}
}