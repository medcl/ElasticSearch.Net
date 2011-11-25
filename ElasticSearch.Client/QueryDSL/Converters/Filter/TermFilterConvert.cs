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
                writer.WriteRawValue(string.Format("{{term: {{ \"{0}\" : \"{1}\"}} }}", term.Field, term.Value));
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