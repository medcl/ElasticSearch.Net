using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Matches documents that have fields that contain a term (not analyzed). The term query maps to Lucene TermQuery. 
	/// </summary>
	[JsonObject("term")]
	[JsonConverter(typeof(TermQueryConvert))]
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


    /// <summary>
    /// Wraps any query to be used as a filter. Can be placed within queries that accept a filter.
    /// </summary>
    [JsonObject("query")]
    [JsonConverter(typeof(QueryFilterConvert))]
    public class QueryFilter : IFilter
    {
        public bool Cache { get; set; }
        public IQuery Query { get; set; }
        public QueryFilter(IQuery query,bool cache=true)
        {
            Query = query;
            Cache = cache;
        }
    }
}