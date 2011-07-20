using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Matches documents that have fields matching a wildcard expression (not analyzed). Supported wildcards are *, which matches any character sequence (including the empty one), and ?, which matches any single character. 
	/// N ote this query can be slow, as it needs to iterate over many terms. In order to prevent extremely slow wildcard queries, a wildcard term should not start with one of the wildcards * or ?. The wildcard query maps to Lucene WildcardQuery
	/// {"wildcard" : { "user" : { "value" : "ki*y", "boost" : 2.0 } }} 
	/// </summary>
[JsonConverter(typeof(WildcardQueryConverter))]
	public class WildcardQuery:IQuery
	{
		public string Field { get; set; }
		public string WildCardPattern { get; set; }
		[DefaultValue(1.0)]
		public double Boost { get; set; }

		public WildcardQuery(string filed,string pattern,double boost=1.0)
		{
			Field = filed;
			WildCardPattern = pattern;
			Boost = boost;
		}

		public WildcardQuery SetBoost(double boost)
		{
			Boost = boost;
			return this;
		}
	}
}