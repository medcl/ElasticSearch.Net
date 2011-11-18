using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Wraps any query to be used as a filter. Can be placed within queries that accept a filter.
    /// </summary>
    [JsonObject("query")]
    [JsonConverter(typeof(QueryFilterConverter))]
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