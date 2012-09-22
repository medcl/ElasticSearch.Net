using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class CustomFiltersScoreQueryConverterer:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			CustomFiltersScoreQuery term = (CustomFiltersScoreQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("custom_filters_score");
				writer.WriteStartObject();
				writer.WritePropertyName("query");
				serializer.Serialize(writer,term.Query);
				writer.WritePropertyName("filters");

				writer.WriteStartArray();
				foreach (var filter in term.Filters)
				{
					serializer.Serialize(writer,filter);
				}
				writer.WriteEndArray();

				writer.WritePropertyName("score_mode");
				writer.WriteValue(term.ScoreMode);

				writer.WritePropertyName("script");
				writer.WriteValue(term.Script);

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
			return typeof(CustomFiltersScoreQuery).IsAssignableFrom(objectType);
		}
	}
}