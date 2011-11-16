using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    public class FilteredQueryConvert : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            //  {
            //    "filtered" : {
            //        "query" : {
            //            "term" : { "tag" : "wow" }
            //        },
            //        "filter" : {
            //            "range" : {
            //                "age" : { "from" : 10, "to" : 20 }
            //            }
            //        }
            //    }
            //}

            FilteredQuery term = (FilteredQuery) value;
            if (term != null)
            {

                writer.WriteStartObject();
                writer.WritePropertyName("filtered");
                writer.WriteStartObject();

                writer.WritePropertyName("query");
                serializer.Serialize(writer, term.Query);
               
                writer.WritePropertyName("filter");
                serializer.Serialize(writer, term.Filter);
               
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
            return typeof(FilteredQuery).IsAssignableFrom(objectType);
        }
    }
}