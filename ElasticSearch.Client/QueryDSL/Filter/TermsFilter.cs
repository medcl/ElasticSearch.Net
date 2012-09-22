using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents that have fields that match any of the provided terms (not analyzed)
    /// </summary>
    [JsonObject("terms")]
    [JsonConverter(typeof(TermsFilterConverter))]
    public class TermsFilter:IFilter
    {
        public string Field { get; set; }
        public object[] Values { get; set; }
        public bool Cache;

        public TermsFilter(string field,params object[] terms)
        {
            Field = field;
            Values = terms;
        }
        public TermsFilter SetCache(bool cache)
        {
            Cache = cache;
            return this;
        }
    }
}