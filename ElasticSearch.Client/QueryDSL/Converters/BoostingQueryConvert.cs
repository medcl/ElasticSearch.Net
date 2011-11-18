using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	public class BoostingQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			BoostingQuery term = (BoostingQuery)value;
			if (term != null)
			{
				writer.WriteRaw("{boosting:");
				writer.WriteStartObject();
				if (term.Positive!=null)
				{
					writer.WritePropertyName("positive");
					writer.WriteStartObject();
					writer.WritePropertyName("term");
					writer.WriteStartObject();
					writer.WritePropertyName(term.Positive.Name);
					writer.WriteValue(term.Positive.Value);
					writer.WriteEndObject();
					writer.WriteEndObject();
				}
				if (term.Negative != null)
				{
					writer.WritePropertyName("negative");
					writer.WriteStartObject();
					writer.WritePropertyName("term");
					writer.WriteStartObject();
					writer.WritePropertyName(term.Negative.Name);
					writer.WriteValue(term.Negative.Value);
					writer.WriteEndObject();
					writer.WriteEndObject();
				}
				writer.WritePropertyName("negative_boost");
				writer.WriteValue(term.NegativeBoost);
				writer.WriteRaw("}");
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(BoostingQuery).IsAssignableFrom(objectType); 
		}
	}
}