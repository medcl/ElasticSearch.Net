using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents that include only hits that exists within a specific distance from a geo point. 
    /// </summary>
    [JsonObject("geo_distance")]
    [JsonConverter(typeof(GeoDistanceFilterConvert))]
    public class GeoDistanceFilter:IFilter
    {
        //TODO
    }
}