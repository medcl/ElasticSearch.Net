using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// The has_child query works the same as the has_child filter, by automatically wrapping the filter with a constant_score. 
	/// </summary>
	[JsonObject("fuzzy")]
	[JsonConverter(typeof(HasChildQueryConverter))]
	public class HasChildQuery:IQuery
	{
		public string Type;
		public IQuery Query;
		/// <summary>
		/// A _scope can be defined on the filter allowing to run facets on the same scope name that will work against the child documents
		/// </summary>
		public string Scope;

		public HasChildQuery(string type, IQuery query, string scope = null)
		{
			Type = type;
			Query = query;
			Scope = scope;
		}
	}
}