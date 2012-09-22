using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class FuzzyLikeThisQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			FuzzyLikeThisQuery term = (FuzzyLikeThisQuery)value;
			if (term != null)
			{

				writer.WriteStartObject();
				writer.WritePropertyName("fuzzy_like_this");
				writer.WriteStartObject();

				writer.WritePropertyName("fields");
				writer.WriteStartArray();
				foreach (var field in term.Fields)
				{
					writer.WriteValue(field);
				}
				writer.WriteEndArray();

				writer.WritePropertyName("like_text");
				writer.WriteValue(term.LikeText);

				if(term.MaxQueryTerms!=25)
				{
					writer.WritePropertyName("max_query_terms");
					writer.WriteValue(term.MaxQueryTerms);
				}

				if (Math.Abs(term.MinSimilarity - 0.5f) > 0)
				{
					writer.WritePropertyName("min_similarity");
					writer.WriteValue(term.MinSimilarity);
				}		
				
				if (term.PrefixLength  > 0)
				{
					writer.WritePropertyName("prefix_length");
					writer.WriteValue(term.PrefixLength);
				}

				if (term.IgnoreTermFrequency != default(bool))
				{
					writer.WritePropertyName("ignore_tf");
					writer.WriteValue(term.IgnoreTermFrequency.ToString().ToLower());
				}		
				
				if (term.Boost != 1.0f)
				{
					writer.WritePropertyName("boost");
					writer.WriteValue(term.Boost);
				}

				if (! string.IsNullOrEmpty(term.Analyzer))
				{
					writer.WritePropertyName("analyzer");
					writer.WriteValue(term.Analyzer);
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
			return typeof(FuzzyLikeThisQuery).IsAssignableFrom(objectType);
		}
	}
}