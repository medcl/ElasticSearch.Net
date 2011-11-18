using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    public class TypeFilterConvert:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
//            {
//    "type" : {
//        "value" : "my_type"
//    }
//}   

            TypeFilter term = (TypeFilter)value;
            if (term != null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("{type:{ \"value\" : \"");
                stringBuilder.Append(term.Type);
                stringBuilder.Append("\"}}");

                writer.WriteRawValue(stringBuilder.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TypeFilter).IsAssignableFrom(objectType);
        }
    }
}