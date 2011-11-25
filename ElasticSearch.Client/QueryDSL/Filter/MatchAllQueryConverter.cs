using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class MatchAllQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			MatchAllQuery term = (MatchAllQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("match_all");
				writer.WriteStartObject();

				if (!string.IsNullOrEmpty(term.NormsField))
				{
					writer.WritePropertyName("norms_field");
					writer.WriteValue(term.NormsField);
				}

				if (term.Boost > 0)
				{
					writer.WritePropertyName("boost");
					writer.WriteValue(term.Boost);
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
			return typeof(MatchAllQuery).IsAssignableFrom(objectType);
		}
	}
}