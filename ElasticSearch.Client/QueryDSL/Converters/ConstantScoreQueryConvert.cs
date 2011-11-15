using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    public class ConstantScoreQueryConvert : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            ConstantScoreQuery term = (ConstantScoreQuery)value;
            if (term != null)
            {

                writer.WriteStartObject();
                writer.WritePropertyName("constant_score");
                writer.WriteStartObject();

                if (term.Query != null)
                {
                    writer.WritePropertyName("query");
                    serializer.Serialize(writer, term.Query);
                    writer.WriteRaw(",\"boost\": " + term.Boost);
                }else if(term.Filter!=null)
                {
                    writer.WritePropertyName("filter");
                    serializer.Serialize(writer, term.Filter);
                    writer.WriteRaw(",\"boost\": " + term.Boost);  
                }
                writer.WriteEndObject();
                writer.WriteEndObject();

//                var stringBuilder = new StringBuilder();
//                stringBuilder.Append("{constant_score:{ ");
//                stringBuilder.Append("\"default_field\":\"" + term.DefaultField + "\" ");
//                stringBuilder.Append(",\"query\":\"" + term.Query + "\" ");
//                stringBuilder.Append(",\"default_operator\":\"" + term.DefaultOperator + "\" ");
//                stringBuilder.Append(",\"analyzer\":\"" + term.Analyzer + "\" ");
//                stringBuilder.Append("}}");
//
//                //TODO 完成更多参数+ 参数判断
//
//                writer.WriteRawValue(stringBuilder.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ConstantScoreQuery).IsAssignableFrom(objectType); 
        }
    }
}