using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class HasChildFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
             HasChildFilter term = (HasChildFilter)value;
             if (term != null)
             {

                 writer.WriteStartObject();
                 writer.WritePropertyName("has_child");
                 writer.WriteStartObject();

                 if (term.Query != null)
                 {
                     writer.WritePropertyName("query");
                     serializer.Serialize(writer, term.Query);
                     writer.WriteRaw(",\"type\": \"" + term.Type+"\"");
                     if(term.Scope!=null)
                     {
                         writer.WriteRaw(",\"_scope\": \"" + term.Scope + "\"");    
                     }
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
            return typeof(HasChildFilter).IsAssignableFrom(objectType);
        }
    }
}