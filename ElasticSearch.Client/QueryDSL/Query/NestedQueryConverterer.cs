using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class NestedQueryConverterer:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			NestedQuery term = (NestedQuery)value;
			if (term != null)
			{

				writer.WriteStartObject();
				writer.WritePropertyName("nested");
				writer.WriteStartObject();

				writer.WritePropertyName("path");
				writer.WriteValue(term.Path);
				writer.WritePropertyName("query");
				if (term.Query != null)
				{
					serializer.Serialize(writer, term.Query);
				}
				else
				{
					serializer.Serialize(writer, term.Filter);
				}
				writer.WritePropertyName("score_mode");
				writer.WriteValue(term.ScoreMode);
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
			return typeof(NestedQuery).IsAssignableFrom(objectType);
		}
	}
}