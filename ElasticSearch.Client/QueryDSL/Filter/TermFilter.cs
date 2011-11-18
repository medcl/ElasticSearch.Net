using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
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


    [JsonObject("match_all")]
    [JsonConverter(typeof(MatchAllFilterConverter))]
    public class MatchAllFilter:IFilter
    {
        
    }

    public class MatchAllFilterConverter:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}