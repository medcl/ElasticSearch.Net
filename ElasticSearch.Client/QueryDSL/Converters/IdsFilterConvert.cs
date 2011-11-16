using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    public class IdsFilterConvert:JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //{ "ids": {  "type": ["my_type","type2"],  "values": ["1","4","100"  ] }}

            IdsFilter term = (IdsFilter)value;
            if (term != null)
            {
                if(term.Values==null||term.Values.Count<0){
                    throw new ArgumentException();
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.Append("{ \"ids\": {  \"type\": [");
                int i = 0;
                foreach(var type in term.Types)
                {
                    if(i>0)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append("\"");
                    stringBuilder.Append(type);
                    stringBuilder.Append("\"");
                    i++;
                }
                stringBuilder.Append("],  \"values\": [ ");
                i = 0;
                foreach (var v in term.Values)
                {
                    if (i > 0)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append("\"");
                    stringBuilder.Append(v);
                    stringBuilder.Append("\"");
                    i++;
                }
                stringBuilder.Append(" ] }}");
                
                writer.WriteRawValue(stringBuilder.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IdsFilter).IsAssignableFrom(objectType);
        }
    }
}