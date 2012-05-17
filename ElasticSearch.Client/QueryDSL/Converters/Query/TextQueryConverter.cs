using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class TextQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			TextQuery term = (TextQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("text");
				writer.WriteStartObject();
				writer.WritePropertyName(term.Field);
				writer.WriteStartObject();
				writer.WritePropertyName("query");
				writer.WriteValue(term.Text);
				if(!string.IsNullOrEmpty(term.QueryType))
				{
					writer.WritePropertyName("type");
					writer.WriteValue(term.QueryType);
				}
				if(term.Operator!=default(Operator))
				{
					writer.WritePropertyName("operator");
					writer.WriteValue(term.Operator.ToString().ToLower());
				}
				if(term.Slop!=default(int))
				{
					writer.WritePropertyName("slop");
					writer.WriteValue(term.Slop);
				}

				if (!Equals(term.Fuzziness, default(float)))
				{
					writer.WritePropertyName("fuzziness");
					writer.WriteValue(term.Fuzziness);
				}

                if (!Equals(term.Boost, default(float)))
                {
                    writer.WritePropertyName("boost");
                    writer.WriteValue(term.Boost);
                }

				if (!Equals(term.PrefixLength, default(int)))
				{
					writer.WritePropertyName("prefix_length");
					writer.WriteValue(term.PrefixLength);
				}

				if (!Equals(term.MaxExpansions, default(int)))
				{
					writer.WritePropertyName("max_expansions");
					writer.WriteValue(term.MaxExpansions);
				}


                if (!string.IsNullOrEmpty(term.Analyzer))
                {
                    writer.WritePropertyName("analyzer");
                    writer.WriteValue(term.Analyzer);
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
			return typeof(TextQuery).IsAssignableFrom(objectType);
		}
	}
}