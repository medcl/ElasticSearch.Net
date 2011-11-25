using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class OrFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            OrFilter term = (OrFilter)value;
            if (term != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("or");
                writer.WriteStartObject();
                writer.WritePropertyName("filters");
                writer.WriteStartArray();
                foreach (var filter in term.Filters)
                {
                    serializer.Serialize(writer, filter);
                }
                writer.WriteEndArray();
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
            return typeof(OrFilter).IsAssignableFrom(objectType);
        }
    }
}