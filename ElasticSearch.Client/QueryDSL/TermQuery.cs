using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Matches documents that have fields that contain a term (not analyzed). The term query maps to Lucene TermQuery. 
	/// </summary>
	[JsonObject("term")]
	public class TermQuery:IQuery
	{
		public string Field { get; set; }
		public object Value { get; set; }
		public double Boost { get; set; }
	}
}