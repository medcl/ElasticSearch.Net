using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    [JsonObject("term")]
    [JsonConverter(typeof(TermFilterConvert))]
    public class TermFilter:IFilter
    {
        public string Field { get; set; }
        public object Value { get; set; }
        [JsonProperty("boost")]
        [DefaultValue(1.0)]
        public double Boost { get; set; }

        public TermFilter(string field, object value, double boost = 1.0)
        {
            Field = field;
            Value = value;
            Boost = boost;
        }

        public TermFilter SetBoost(double boost)
        {
            Boost = boost;
            return this;
        }
    }
}