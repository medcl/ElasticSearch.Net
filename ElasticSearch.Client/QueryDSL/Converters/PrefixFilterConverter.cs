using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class PrefixFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            PrefixFilter term = (PrefixFilter)value;
            if (term != null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("{prefix:{ \""+term.Field+"\" : \"");
                stringBuilder.Append(term.Prefix);
                stringBuilder.Append("\""); 
                
                stringBuilder.Append(",\"_cache\" :"+term.Cache.ToString().ToLower());
                
                stringBuilder.Append("}}");

                writer.WriteRawValue(stringBuilder.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(PrefixFilter).IsAssignableFrom(objectType);
        }
    }
}