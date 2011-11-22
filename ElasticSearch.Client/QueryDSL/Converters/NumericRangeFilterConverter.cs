using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    public class NumericRangeFilterConverter:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            NumericRangeFilter term = (NumericRangeFilter)value;
            if (term != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("numeric_range");
                writer.WriteStartObject();
                writer.WritePropertyName(term.Field);
                writer.WriteStartObject();
                writer.WritePropertyName("from");
                writer.WriteValue(term.From);
                writer.WritePropertyName("to");
                writer.WriteValue(term.To);
                writer.WritePropertyName("include_lower");
                writer.WriteValue(term.IncludeLower);
                writer.WritePropertyName("include_upper");
                writer.WriteValue(term.IncludeUpper);
                writer.WriteEndObject();
                writer.WriteEndObject();
                writer.WriteRaw(",\"_cache\": " + term.Cache.ToString().ToLower());
                writer.WriteEndObject();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(NumericRangeFilter).IsAssignableFrom(objectType);
        }
    }
}