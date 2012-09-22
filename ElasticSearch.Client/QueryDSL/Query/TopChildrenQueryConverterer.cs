using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class TopChildrenQueryConverterer:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			TopChildrenQuery term = (TopChildrenQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("top_children");
				writer.WriteStartObject();

				writer.WritePropertyName("type");
				writer.WriteValue(term.Type);

				writer.WritePropertyName("score");
				writer.WriteValue( term.Score);

				writer.WritePropertyName("factor");
				writer.WriteValue(term.Factor);

				writer.WritePropertyName("incremental_factor");
				writer.WriteValue(term.IncrementalFactor);

				writer.WritePropertyName("query");
				serializer.Serialize(writer, term.Query);

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
			return typeof(TopChildrenQuery).IsAssignableFrom(objectType);
		}
	}
}