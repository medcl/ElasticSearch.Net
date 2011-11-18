using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents where a specific field has no value in them.
    /// The result of the filter is always cached.
    /// </summary>
    [JsonObject("missing")]
    [JsonConverter(typeof(MissingFilterConverter))]
    public class MissingFilter:IFilter
    {
        public string Filed;
        public MissingFilter(string filed)
        {
            Filed = filed;
        }
    }
}