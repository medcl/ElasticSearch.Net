using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A query that wraps a filter or another query and simply returns a constant score equal to the query boost for every document in the filter. Maps to Lucene ConstantScoreQuery.
    /// </summary>
    [JsonObject("constant_score")]
    [JsonConverter(typeof(ConstantScoreQueryConvert))]
    public class ConstantScoreQuery:IQuery
    {
        /// <summary>
        /// A query can also be wrapped in a constant_score query:
        /// </summary>
        public IQuery Query { get; set; }
        /// <summary>
        /// The filter object can hold only filter elements, not queries. Filters can be much faster compared to queries since they don¡¯t perform any scoring, especially when they are cached.
        /// </summary>
        public IFilter Filter { get; set; }
        [DefaultValue(1.0)]
        public double Boost { get; set; }

        public ConstantScoreQuery(IQuery query,double boost=1.0)
        {
            Query = query;
            Boost = boost;
        } 
        
        public ConstantScoreQuery(IFilter filter,double boost=1.0)
        {
            Filter = filter;
            Boost = boost;
        }
    }
}