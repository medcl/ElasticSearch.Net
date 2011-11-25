using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A filter that matches documents using OR boolean operator on other queries. This filter is more performant then bool filter. Can be placed within queries that accept a filter
    /// </summary>
    [JsonObject("or")]
    [JsonConverter(typeof(OrFilterConverter))]
    public class OrFilter : IFilter
    {
        /// <summary>
        /// The result of the filter is not cached by default. The `cache` can be set to `true` in order to cache it (tough usually not needed). Since the @cache@ element requires to be set on the and filter itself, the structure then changes a bit to have the filters provided within a filters element:
        /// </summary>
        public bool Cache = false;

        public List<IFilter> Filters;

        public OrFilter(params IFilter[] filters)
        {
            Filters = new List<IFilter>();
            Filters.AddRange(filters);
        }

        public void Add(IFilter filter)
        {
            Filters.Add(filter);
        }

        public void SetCache(bool cache)
        {
            Cache = cache;
        }

    }
}