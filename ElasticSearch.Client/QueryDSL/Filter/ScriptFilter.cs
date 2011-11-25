using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// A filter allowing to define scripts as filters.
    /// </summary>
    [JsonObject("script")]
    [JsonConverter(typeof(ScriptFilterConverter))]
    public class ScriptFilter:IFilter
    {
        public string Script;
        public ScriptFilter(string script, Dictionary<string, object> param=null)
        {
            Script = script;
            if(param!=null)
            {
                Params = param;
            }
        }

        public Dictionary<string, object> Params;
        public bool Cache;

        public ScriptFilter SetCache(bool cache)
        {
            Cache = cache;
            return this;
        }

    }
}