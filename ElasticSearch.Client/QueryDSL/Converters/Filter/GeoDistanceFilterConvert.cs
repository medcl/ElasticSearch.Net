using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
    internal class GeoDistanceFilterConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            GeoDistanceFilter term = (GeoDistanceFilter)value;
            if (term != null)
            {

                writer.WriteStartObject();
                writer.WritePropertyName("geo_distance");
                writer.WriteStartObject();

                if (!string.IsNullOrEmpty(term.Distance))
                {
                    writer.WritePropertyName("distance");
                    writer.WriteValue(term.Distance);

                    if (!string.IsNullOrEmpty(term.DistanceType))
                    {
                        writer.WritePropertyName("distance_type");
                        writer.WriteValue(term.DistanceType);
                    }
                    if (!string.IsNullOrEmpty(term.Field))
                    {
                        writer.WritePropertyName(term.Field);
                        writer.WriteValue(term.Location);
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
            return typeof(GeoDistanceFilter).IsAssignableFrom(objectType);
        }
    }
}