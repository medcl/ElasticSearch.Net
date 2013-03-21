using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents that include only hits that exists within a specific distance from a geo point. 
    /// </summary>
    [JsonObject("geo_distance")]
    [JsonConverter(typeof(GeoDistanceFilterConverter))]
    public class GeoDistanceFilter:IFilter
    {
        /// <summary>
        /// The distance to include hits in the filter. The distance can be a numeric value, and then the distance_unit (either mi/miles or km can be set) controlling the unit. Or a single string with the unit as well.
        /// </summary>
        public string Distance;

        /// <summary>
        /// How to compute the distance. Can either be arc (better precision) or plane (faster). Defaults to arc.
        /// </summary>
        public string DistanceType;

        /// <summary>
        /// Will an optimization of using first a bounding box check will be used. Defaults to memory which will do in memory checks. Can also have values of indexed to use indexed value check (make sure the geo_point type index lat lon in this case), or none which disables bounding box optimization.
        /// </summary>
        public string OptimizeBbox;

        /// <summary>
        /// The field to be queried
        /// </summary>
        public string Field;

        /// <summary>
        /// Lat Lon As String:"40,-70"   OR  Geohash:"drm3btev3e86"
        /// </summary>
        public string Location;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance">The distance to include hits in the filter. The distance can be a numeric value, and then the distance_unit (either mi/miles or km can be set) controlling the unit. Or a single string with the unit as well.</param>
        /// <param name="locatoin">can be set with Lat Lon As String:"40,-70"   OR  Geohash:"drm3btev3e86"</param>
        public GeoDistanceFilter(string filed, string locatoin, string distance)
        {
            this.Field = filed;
            this.Distance = distance;
            this.Location = locatoin;
        }
    }

}