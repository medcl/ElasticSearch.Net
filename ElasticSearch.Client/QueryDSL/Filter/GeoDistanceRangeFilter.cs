using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents that exists within a range from a specific point:
    /// </summary>
    [JsonObject("geo_distance_range")]
    [JsonConverter(typeof(GeoDistanceRangeFilterConverter))]
    public class GeoDistanceRangeFilter:IFilter
    {
        //TODO
    }
}