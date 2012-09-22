using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    /// <summary>
    /// Matches documents that have fields that contain a term (not analyzed). The term query maps to Lucene TermQuery
    /// </summary>
    [JsonObject("term")]
    [JsonConverter(typeof(TermFilterConverter))]
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