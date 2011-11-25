using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class ScriptFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ScriptFilter term = (ScriptFilter)value;
            if (term != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("script");
                writer.WriteStartObject();
                writer.WritePropertyName("script");
                writer.WriteValue(term.Script);
                if (term.Params != null && term.Params.Count > 0)
                {
                    writer.WritePropertyName("params");
                    writer.WriteStartObject();
                    foreach (var filter in term.Params)
                    {
                        writer.WritePropertyName(filter.Key);
                        writer.WriteValue(filter.Value);
                    }
                    writer.WriteEndObject();
                }
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
            return typeof(ScriptFilter).IsAssignableFrom(objectType);
        }
    }
}