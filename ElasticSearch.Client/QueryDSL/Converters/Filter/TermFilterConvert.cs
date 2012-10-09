using Newtonsoft.Json;
using System;
namespace ElasticSearch.Client.QueryDSL
{
    internal class TermFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TermFilter term = (TermFilter)value;
            if (term != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("term");
                writer.WriteStartObject();
                writer.WritePropertyName(term.Field);
                writer.WriteValue(term.Value);
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TermFilter).IsAssignableFrom(objectType);
        }
    }
}