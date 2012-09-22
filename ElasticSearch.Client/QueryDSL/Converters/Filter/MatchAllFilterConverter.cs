using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class MatchAllFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            MatchAllFilter term = (MatchAllFilter)value;
            if (term != null)
            {
                writer.WriteRawValue("{\"match_all\" : { }}");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(MatchAllFilter).IsAssignableFrom(objectType);
        }
    }
}