using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Filters documents that only have the provided ids. notes,this filter does not require the _id field to be indexed since it works using the _uid field.
    ///The type is optional and can be omitted, and can also accept an array of values.
    /// </summary>
    [JsonObject("ids")]
    [JsonConverter(typeof(IdsFilterConvert))]
    public class IdsFilter:IFilter
    {
        public List<string> Types;
        public List<string> Values;

        public IdsFilter(string[] type,params string[] ids)
        {
            Types = new List<string>(type);
            Values = new List<string>(ids);
        }

        public IdsFilter(string type, params string[] ids)
        {
            Types = new List<string>();
            Types.Add(type);
            Values = new List<string>(ids);
        }

        public void AddIds(params string[] ids)
        {
            Values.AddRange(ids);
        }

        public void AddType(string type)
        {
            Types.Add(type);
        }

    }
}