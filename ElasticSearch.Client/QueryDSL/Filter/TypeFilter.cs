using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents matching the provided document / mapping type. notice, this filter can work even when the _type field is not indexed (using the _uid field).
    /// </summary>
    [JsonObject("type")]
    [JsonConverter(typeof(TypeFilterConverter))]
    public class TypeFilter:IFilter
    {
        public string Type;
        public TypeFilter(string type)
        {
            Type = type;
        }
    }
}