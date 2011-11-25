using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A filter that matches on all documents
    /// </summary>
    [JsonObject("match_all")]
    [JsonConverter(typeof(MatchAllFilterConverter))]
    public class MatchAllFilter:IFilter
    {
        
    }
}