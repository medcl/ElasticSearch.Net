using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class TermsFacetConverterer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TermsFacet term = (TermsFacet)value;
            if (term != null)
            {
                if (term.facetItems != null)
                {
                    writer.WriteStartObject();
                    foreach (TermsFacet.TermsFacetItem termsFacetItem in term.facetItems)
                    {
                        writer.WritePropertyName(termsFacetItem.FacetName);
                        writer.WriteStartObject();
                        //1
                        writer.WritePropertyName("terms");
                        writer.WriteStartObject();
                        writer.WritePropertyName("field");
                        writer.WriteValue(termsFacetItem.Field);
                        writer.WritePropertyName("size");
                        writer.WriteValue(termsFacetItem.Size);
                        writer.WriteEndObject();
                        //1
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TermsFacet).IsAssignableFrom(objectType);
        }
    }
}