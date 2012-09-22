using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class NotFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            NotFilter term = (NotFilter)value;
            if (term != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("not");
                writer.WriteStartObject();
                writer.WritePropertyName("filter");
//                writer.WriteStartArray();
//                foreach (var filter in term.Filters)
//                {
                serializer.Serialize(writer, term.Filter);
//                }
//                writer.WriteEndArray();
                writer.WriteEndObject();
//                writer.WriteRaw(",\"_cache\": " + term.Cache.ToString().ToLower());
                writer.WriteEndObject();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(NotFilter).IsAssignableFrom(objectType);
        }
    }
}