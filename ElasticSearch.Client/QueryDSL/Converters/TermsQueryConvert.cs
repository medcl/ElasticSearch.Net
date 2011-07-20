using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal  class TermsQueryConvert:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			TermsQuery term = (TermsQuery)value;
			//{    "terms" : {        "tags" : [ "blue", "pill" ],        "minimum_match" : 1    }}
			if (term != null)
			{
				writer.WriteRaw("{terms:");
				writer.WriteStartObject();
				writer.WritePropertyName(term.Field);
				writer.WriteStartArray();
				foreach(var t in term.Values)
				{
					writer.WriteValue(t);
				}
				writer.WriteEndArray();
				writer.WritePropertyName("minimum_match");
				writer.WriteValue(term.MinimumMatch);
				writer.WriteEndObject();
				writer.WriteRaw("}");
			}

		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(TermsQuery).IsAssignableFrom(objectType);
		}
	}
}