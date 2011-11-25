using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A nested filter, works in a similar fashion to the nested query, except used as a filter. It follows exactly the same structure, but also allows to cache the results (set _cache to true), and have it named (set the _name value). 
    /// </summary>
    [JsonObject("nested")]
    [JsonConverter(typeof(NestedFilterConverter))]
    public class NestedFilter:IFilter
    {
        public bool Cache;
        public string Path;
        public IQuery Query;
        public IFilter Filter;

        public NestedFilter(string path,IFilter filter,bool cache)
        {
            Path = path;
            Filter = filter;
            Cache = cache;
        }

        public NestedFilter(string path, IQuery query, bool cache)
        {
            Path = path;
            Query = query;
            Cache = cache;
        }
    }
}