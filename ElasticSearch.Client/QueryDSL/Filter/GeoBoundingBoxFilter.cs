using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A filter allowing to filter hits based on a point location using a bounding box.
    /// </summary>
    [JsonObject("geo_bounding_box")]
    [JsonConverter(typeof(GeoBBoxFilterConverter))]
    public class GeoBoundingBoxFilter:IFilter
    {
        //TODO
    }
}