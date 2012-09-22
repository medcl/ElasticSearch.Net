using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A query that applies a filter to the results of another query,This query maps to Lucene FilteredQuery.
    /// {
    //    "filtered" : {
    //        "query" : {
    //            "term" : { "tag" : "wow" }
    //        },
    //        "filter" : {
    //            "range" : {
    //                "age" : { "from" : 10, "to" : 20 }
    //            }
    //        }
    //    }
    //}
    /// </summary>
    [JsonObject("filtered")]
    [JsonConverter(typeof(FilteredQueryConverter))]
    public class FilteredQuery:IQuery
    {
        public IQuery  Query { get; set; }
        /// <summary>
        /// The filter object can hold only filter elements, not queries. 
        /// Filters can be much faster compared to queries since they don¡¯t perform any scoring, especially when they are cached.
        /// </summary>
        public IFilter Filter{ get; set; }

        public FilteredQuery(IQuery query,IFilter filter)
        {
            Query = query;
            Filter = filter;
        }
        
    }
}