using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Matches documents that have fields that contain a term (not analyzed). The term query maps to Lucene TermQuery. 
	/// </summary>
	[JsonObject("term")]
	[JsonConverter(typeof(TermQueryConverter))]
	public class TermQuery:IQuery
	{
		public string Field { get; set; }
		public object Value { get; set; }
		[JsonProperty("boost")]
		[DefaultValue(1.0)]
		public double Boost { get; set; }

		public TermQuery(string field, object value,double boost=1.0)
		{
			Field = field;
			Value = value;
			Boost = boost;
		}

		public TermQuery SetBoost(double boost)
		{
			Boost = boost;
			return this;
		}
	

    }
}