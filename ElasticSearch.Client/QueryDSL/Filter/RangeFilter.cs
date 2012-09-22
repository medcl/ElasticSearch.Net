using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents with fields that have terms within a certain range. Similar to range query, except that it acts as a filter. Can be placed within queries that accept a filter.
    /// </summary>
    [JsonObject("range")]
    [JsonConverter(typeof(RangeFilterConverter))]
    public class RangeFilter:IFilter
    {
        public string Field;
        public string From;
        public string To;
        public bool IncludeLower;
        public bool IncludeUpper;
        public bool Cache;

        public RangeFilter(string fileld, string from, string to, bool includeLower, bool includeUpper)
        {
            Field = fileld;
            From = from;
            To = to;
            IncludeLower = includeLower;
            IncludeUpper = includeUpper;
        }

        public RangeFilter SetCache(bool cache)
        {
            Cache = cache;
            return this;
        }  
    }
}