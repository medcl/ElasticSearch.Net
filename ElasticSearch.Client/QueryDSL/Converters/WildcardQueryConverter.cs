using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	public class WildcardQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			WildcardQuery term = (WildcardQuery)value;

			if (term != null)
				writer.WriteRaw(string.Format("{{ \"{0}\" : {{ \"wildcard\" : \"{1}\", \"boost\":{2} }}}}", term.Field, term.WildCardPattern, term.Boost));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(WildcardQuery).IsAssignableFrom(objectType); 
		}
	}
}