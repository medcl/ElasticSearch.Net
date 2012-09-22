using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A limit filter limits the number of documents (per shard) to execute on.
    /// </summary>
    [JsonObject("limit")]
    [JsonConverter(typeof(LimitFilterConverter))]
    public class LimitFilter:IFilter
    {
        public int Limit;
        public LimitFilter(int limit)
        {
            Limit = limit;
        }
    }
}