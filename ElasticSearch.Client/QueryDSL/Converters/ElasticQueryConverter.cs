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
				writer.WritePropertyName("query");
				serializer.Serialize(writer,term.Query);

				writer.WritePropertyName("explain");
				writer.WriteValue(term.Explain);

				writer.WritePropertyName("from");
				writer.WriteValue(term.From);

				writer.WritePropertyName("size");
				writer.WriteValue(term.Size);

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