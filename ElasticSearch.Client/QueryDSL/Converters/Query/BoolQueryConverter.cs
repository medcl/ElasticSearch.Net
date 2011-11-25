using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class BoolQueryConverterer : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			BoolQuery term = (BoolQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("bool");
				writer.WriteStartObject();
				bool pre = false;
				if(term.MustQueries!=null&&term.MustQueries.Count>0)
				{
					writer.WritePropertyName("must");
					writer.WriteStartArray();
					foreach (var VARIABLE in term.MustQueries)
					{
						serializer.Serialize(writer, VARIABLE);
					}
					writer.WriteEndArray();
				}
				if (term.MustNotQueries != null&&term.MustNotQueries.Count>0)
				{
					writer.WritePropertyName("must_not");
					writer.WriteStartArray();
					foreach (var VARIABLE in term.MustNotQueries)
					{
						serializer.Serialize(writer, VARIABLE);
					}
					writer.WriteEndArray();
				}
				if (term.ShouldQueries !=null&& term.ShouldQueries.Count > 0)
				{
					writer.WritePropertyName("should");
					writer.WriteStartArray();
					foreach (var shouldQuery in term.ShouldQueries)
					{
						serializer.Serialize(writer,shouldQuery);
					}
					writer.WriteEndArray();
				}
				if (Math.Abs(term.Boost - 1.0) > 0)
				{
					writer.WritePropertyName("boost");
					writer.WriteValue(term.Boost);
				}
				if (term.DisableCoord)
				{
					writer.WritePropertyName("disable_coord");
					writer.WriteValue(term.DisableCoord);
				}
				if (term.MinimumNumberShouldMatch != 1)
				{
					writer.WritePropertyName("minimum_number_should_match");
					writer.WriteValue(term.MinimumNumberShouldMatch);
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
			return typeof(BoolQuery).IsAssignableFrom(objectType);
		}
	}
}