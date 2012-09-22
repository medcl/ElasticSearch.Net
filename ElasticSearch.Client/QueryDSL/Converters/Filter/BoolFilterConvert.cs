using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class BoolFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            BoolFilter term = (BoolFilter)value;
            if (term != null)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("bool");
                writer.WriteStartObject();
                bool pre = false;
                if (term.MustFilters != null && term.MustFilters.Count > 0)
                {
                    writer.WritePropertyName("must");
                    writer.WriteStartArray();
                    foreach (var VARIABLE in term.MustFilters)
                    {
                        serializer.Serialize(writer, VARIABLE);
                    }
                    writer.WriteEndArray();
                }
                if (term.MustNotFilters != null && term.MustNotFilters.Count > 0)
                {
                    writer.WritePropertyName("must_not");
                    writer.WriteStartArray();
                    foreach (var VARIABLE in term.MustNotFilters)
                    {
                        serializer.Serialize(writer, VARIABLE);
                    }
                    writer.WriteEndArray();
                }
                if (term.ShouldFilters != null && term.ShouldFilters.Count > 0)
                {
                    writer.WritePropertyName("should");
                    writer.WriteStartArray();
                    foreach (var shouldQuery in term.ShouldFilters)
                    {
                        serializer.Serialize(writer, shouldQuery);
                    }
                    writer.WriteEndArray();
                }
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
            return typeof(BoolFilter).IsAssignableFrom(objectType);
        }
    }
}