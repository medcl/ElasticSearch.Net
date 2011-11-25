using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class MoreLikeThisQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			MoreLikeThisQuery term = (MoreLikeThisQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("more_like_this");
				writer.WriteStartObject();

				if(term.Fields!=null&&term.Fields.Count>0)
				{
					writer.WritePropertyName("fields");
					writer.WriteStartArray();
					foreach (var field in term.Fields)
					{
						writer.WriteValue(field);
					}
					writer.WriteEndArray();
				}

				if(term.StopWords!=null&&term.StopWords.Count>0)
				{
					writer.WritePropertyName("stop_words");
					writer.WriteStartArray();
					foreach (var word in term.StopWords)
					{
						writer.WriteValue(word);
					}
					writer.WriteEndArray();
				}

				if (!string.IsNullOrEmpty(term.LikeText))
				{
					writer.WritePropertyName("like_text");
					writer.WriteValue(term.LikeText);
				}

				if (Math.Abs(term.PercentTermsToMatch - default(float)) > 0)
				{
					writer.WritePropertyName("percent_terms_to_match");
					writer.WriteValue(term.PercentTermsToMatch);
				}

				if (term.MaxQueryTerms > 0)
				{
					writer.WritePropertyName("max_query_terms");
					writer.WriteValue(term.MaxQueryTerms);
				}

				if (term.MinTermFreq > 0)
				{
					writer.WritePropertyName("min_term_freq");
					writer.WriteValue(term.MinTermFreq);
				}


				if (term.MinDocFreq > 0)
				{
					writer.WritePropertyName("min_doc_freq");
					writer.WriteValue(term.MinDocFreq);
				}
				if (term.MaxDocFreq > 0)
				{
					writer.WritePropertyName("max_doc_freq");
					writer.WriteValue(term.MaxDocFreq);
				}
				if (term.MinWordLen > 0)
				{
					writer.WritePropertyName("min_word_len");
					writer.WriteValue(term.MinWordLen);
				}
				if (term.MaxWordLen > 0)
				{
					writer.WritePropertyName("max_word_len");
					writer.WriteValue(term.MaxWordLen);
				}
				if (term.BoostTerms > 0)
				{
					writer.WritePropertyName("boost_terms");
					writer.WriteValue(term.BoostTerms);
				}		
				
				if (term.Boost > 0)
				{
					writer.WritePropertyName("boost");
					writer.WriteValue(term.Boost);
				}		
				
				if (!string.IsNullOrEmpty(term.Analyzer))
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
			return typeof(MoreLikeThisQuery).IsAssignableFrom(objectType);
		}
	}
}