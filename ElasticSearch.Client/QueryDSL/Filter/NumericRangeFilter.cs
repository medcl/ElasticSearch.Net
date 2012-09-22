using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents with fields that have values within a certain numeric range. Similar to range filter, except that it works only with numeric values, and the filter execution works differently.
    /// </summary>
    [JsonObject("numeric_range")]
    [JsonConverter(typeof(NumericRangeFilterConverter))]
    public class NumericRangeFilter:IFilter
    {
        public string Field;
        public int From;
        public int To;
        public bool IncludeLower;
        public bool IncludeUpper;
        public bool Cache;

        public NumericRangeFilter(string fileld,int from,int to,bool includeLower,bool includeUpper)
        {
            Field = fileld;
            From = from;
            To = to;
            IncludeLower = includeLower;
            IncludeUpper = includeUpper;
        }

        public NumericRangeFilter SetCache(bool cache)
        {
            Cache = cache;
            return this;
        }

    }
}