using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class PrefixQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			PrefixQuery term = (PrefixQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("prefix");
				writer.WriteStartObject();
				writer.WritePropertyName(term.Field);
				writer.WriteStartObject();

				writer.WritePropertyName("value");
				writer.WriteValue(term.Value);

				if (term.Boost > 0)
				{
					writer.WritePropertyName("boost");
					writer.WriteValue(term.Boost);
				}
				writer.WriteEndObject();
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
			return typeof(PrefixQuery).IsAssignableFrom(objectType);
		}
	}
}