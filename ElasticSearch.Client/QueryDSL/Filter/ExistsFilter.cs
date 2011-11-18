using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents where a specific field has a value in them.
    /// The result of the filter is always cached.
    /// </summary>
    [JsonObject("exists")]
    [JsonConverter(typeof(ExistsFilterConverter))]
    public class ExistsFilter:IFilter
    {
        public string Filed;
        
        public ExistsFilter(string field)
        {
            Filed = field;
        }
    }
}