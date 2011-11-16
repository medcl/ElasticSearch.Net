using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    public class AndFilterConvert : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            AndFilter term = (AndFilter)value;
            if (term != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("and");
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
            return typeof(AndFilter).IsAssignableFrom(objectType);
        }
    }
}