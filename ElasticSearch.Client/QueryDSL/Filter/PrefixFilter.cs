using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents that have fields containing terms with a specified prefix (not analyzed). Similar to phrase query, except that it acts as a filter. Can be placed within queries that accept a filter.
    /// </summary>
    [JsonObject("prefix")]
    [JsonConverter(typeof(PrefixFilterConverter))]
    public class PrefixFilter:IFilter
    {
        public string Field;
        public string Prefix;
        public bool Cache;
        public PrefixFilter(string field,string prefix)
        {
            Field = field;
            Prefix = prefix;
        }
        public PrefixFilter SetCache(bool cache)
        {
            Cache = cache;
            return this;
        }
    }
}