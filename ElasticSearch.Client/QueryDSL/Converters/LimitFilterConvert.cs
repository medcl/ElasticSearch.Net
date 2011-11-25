using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class LimitFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
//         {
//             "limit" : {"value" : 100}
//         }
            LimitFilter term = (LimitFilter)value;
            if (term != null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("{limit:{ \"value\" : ");
                stringBuilder.Append(term.Limit);
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
            return typeof(LimitFilter).IsAssignableFrom(objectType);
        }
    }
}