using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class TermQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			TermQuery term = (TermQuery)value;
			if (term != null)
				writer.WriteRawValue(string.Format("{{term: {{ \"{0}\" : {{ \"term\" : \"{1}\", \"boost\":{2} }}}} }}", term.Field, term.Value, term.Boost));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(TermQuery).IsAssignableFrom(objectType);
		}
	}
}