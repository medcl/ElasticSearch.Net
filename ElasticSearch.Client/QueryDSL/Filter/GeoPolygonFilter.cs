using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A filter allowing to include hits that only fall within a polygon of points.
    /// </summary>
    [JsonObject("geo_polygon")]
    [JsonConverter(typeof(GeoPolygonFilterConvert))]
    public class GeoPolygonFilter:IFilter
    {
        //TODO
    }
}