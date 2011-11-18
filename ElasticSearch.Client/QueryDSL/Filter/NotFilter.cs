using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A filter that filters out matched documents using a query. This filter is more performant then bool filter. Can be placed within queries that accept a filter.
    /// The result of the filter is not cached by default. The `_cache` can be set to `true` in order to cache it (tough usually not needed).
    /// </summary>
    [JsonObject("not")]
    [JsonConverter(typeof(NotFilterConverter))]
    public class NotFilter:IFilter
    {

        /// <summary>
        /// The result of the filter is not cached by default. The `_cache` can be set to `true` in order to cache it (tough usually not needed)
        /// </summary>
        public bool Cache = false;

        public IFilter Filter;

        public NotFilter(IFilter filter)
        {
          Filter=filter;
        }
        
        public void SetCache(bool cache)
        {
            Cache = cache;
        }
    }
}