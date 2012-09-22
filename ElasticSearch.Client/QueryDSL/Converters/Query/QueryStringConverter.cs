using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class QueryStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            QueryStringQuery term = (QueryStringQuery)value;
            if (term != null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("{query_string:{ ");
                if (!string.IsNullOrEmpty(term.DefaultField)) { stringBuilder.Append("\"default_field\":\"" + term.DefaultField + "\", "); }
                stringBuilder.Append("\"query\":\"" + term.Query + "\" ");
                if (term.Fields != null && term.Fields.Count > 0)
                {
                    stringBuilder.Append(",\"fields\":[\"" + string.Join("\",\"", term.Fields.ToArray()) + "\"] ");
                }
                stringBuilder.Append(",\"default_operator\":\"" + term.DefaultOperator + "\" ");
                if (!string.IsNullOrEmpty(term.Analyzer)) { stringBuilder.Append(",\"analyzer\":\"" + term.Analyzer + "\" "); }
                stringBuilder.Append("}}");

                //TODO 完成更多参数+ 参数判断

                writer.WriteRawValue(stringBuilder.ToString());
            }

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(QueryStringQuery).IsAssignableFrom(objectType);
        }
    }
}