using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    public class QueryFilterConvert : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            QueryFilter term = (QueryFilter)value;
            if (term != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("query");
                serializer.Serialize(writer, term.Query);
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
            return typeof(QueryFilter).IsAssignableFrom(objectType);
        }
    }
}