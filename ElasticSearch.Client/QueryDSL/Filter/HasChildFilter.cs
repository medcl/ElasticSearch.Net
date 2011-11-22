using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// The has_child filter accepts a query and the child type to run against, and results in parent documents that have child docs matching the query.
    /// With the current implementation, all _id values are loaded to memory (heap) in order to support fast lookups, so make sure there is enough mem for it.
    ///  </summary>
    [JsonObject("has_child")]
    [JsonConverter(typeof(HasChildFilterConverter))]
    public class HasChildFilter:IFilter
    {
        /// <summary>
        /// A _scope can be defined on the filter allowing to run facets on the same scope name that will work against the child documents
        /// </summary>
        public string Scope;
        public string Type;
        public IQuery Query;

        public HasChildFilter(string type,IQuery query,string scope=null)
        {
            Type = type;
            Query = query;
            Scope = scope;
        }


    }
}