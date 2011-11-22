using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// A query that match on any (configurable) of the provided terms. This is a simpler syntax query for using a bool query with several term queries in the should clauses. 
	/// { "terms" : {"tags" : [ "blue", "pill" ], "minimum_match" : 1  }}
	/// The terms query is also aliased with in as the query name for simpler usage.
	/// </summary>
	[JsonObject("terms")]
	[JsonConverter(typeof(TermsQueryConverter))]
	public class TermsQuery:IQuery
	{
		public string Field { get; set; }
		public object[] Values { get; set; }
		[JsonProperty("minimum_match")]
		[DefaultValue(1)]
		public int MinimumMatch { get; set; }

		public TermsQuery(string field,params object[] values)
		{
			Field = field;
			Values = values;
			MinimumMatch = 1;
		}
		public TermsQuery(string field,int  minimunMatch=1,params object[] values):this(field,values)
		{
			MinimumMatch = minimunMatch;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="minimunMatch">default 1</param>
		/// <returns></returns>
		public TermsQuery SetMinimumMatch(int minimunMatch)
		{
			MinimumMatch = minimunMatch;
			return this;
		}
	}
}