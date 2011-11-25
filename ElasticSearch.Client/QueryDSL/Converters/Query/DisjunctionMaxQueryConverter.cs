using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class DisjunctionMaxQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DisjunctionMaxQuery term = (DisjunctionMaxQuery)value;
			if (term != null)
			{
				if (term.Queries == null || term.Queries.Count < 0)
				{
					throw new ArgumentException();
				}

				writer.WriteStartObject();
				writer.WritePropertyName("dis_max");
				writer.WriteStartObject();
				writer.WritePropertyName("tie_breaker");
				writer.WriteValue(term.TieBreaker);
				writer.WritePropertyName("boost");
				writer.WriteValue(term.Boost);
				writer.WritePropertyName("queries");
				writer.WriteStartArray();
				foreach (var query in term.Queries)
				{
					serializer.Serialize(writer, query);
				}
				writer.WriteEndArray();
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
			return typeof(DisjunctionMaxQuery).IsAssignableFrom(objectType);
		}
	}
}