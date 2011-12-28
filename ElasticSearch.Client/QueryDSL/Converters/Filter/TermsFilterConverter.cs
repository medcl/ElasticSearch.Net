using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class TermsFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TermsFilter term = (TermsFilter)value;
            if (term != null)
            {

                var stringBuilder = new StringBuilder();

                stringBuilder.Append("{    \"terms\" : {        \"" + term.Field + "\" : [");

                var i = 0;
                foreach (var t in term.Values)
                {
                    if (i > 0)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append("\"" + t + "\"");
                    i++;
                }

                stringBuilder.Append("]");
//                stringBuilder.Append(",\"_cache\": " + term.Cache.ToString().ToLower());
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
            return typeof(TermsFilter).IsAssignableFrom(objectType);
        }
    }
}