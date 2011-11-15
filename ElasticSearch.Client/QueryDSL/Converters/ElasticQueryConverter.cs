using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	public class ElasticQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ElasticQuery term = (ElasticQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();

                writer.WriteRaw("\"from\": " + term.From);
                writer.WriteRaw(",\"size\": " + term.Size+",");
				writer.WritePropertyName("query");
				serializer.Serialize(writer,term.Query);
               
                if(term.Fields!=null&&term.Fields.Count>0)
                {
                    writer.WritePropertyName("fields");
                    writer.WriteStartArray();
                    foreach (var field in term.Fields)
                    {
                        writer.WriteRawValue("\"" + field + "\"");
                    }
                    writer.WriteEndArray();
                }

                writer.WriteRaw(",\"explain\": " + term.Explain.ToString().ToLower());

                writer.WriteEndObject();
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(ElasticQuery).IsAssignableFrom(objectType); 
		}
	}
}