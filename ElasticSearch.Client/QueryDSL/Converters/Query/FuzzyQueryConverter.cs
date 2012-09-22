using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class FuzzyQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			FuzzyQuery term = (FuzzyQuery)value;
			if (term != null)
			{

				writer.WriteStartObject();
				writer.WritePropertyName("fuzzy");
				writer.WriteStartObject();

				writer.WritePropertyName(term.Field);
				writer.WriteStartObject();
				
				writer.WritePropertyName("value");
				writer.WriteValue(term.Value);

				if (!string.IsNullOrEmpty(term.MinSimilarity))
				{
					writer.WritePropertyName("min_similarity");
					writer.WriteValue(term.MinSimilarity);
				}	
				
				if (term.PrefixLength>0)
				{
					writer.WritePropertyName("prefix_length");
					writer.WriteValue(term.PrefixLength);
				}	
				
				if (term.Boost>0)
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
			return typeof(FuzzyQuery).IsAssignableFrom(objectType);
		}
	}
}