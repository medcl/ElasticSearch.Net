using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class FieldQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			FieldQuery term = (FieldQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("field");
				writer.WriteStartObject();
				writer.WritePropertyName(term.Field);
				writer.WriteValue(term.QueryString);
				
				if(!term.Boost.Equals(default(float)))
				{
					writer.WritePropertyName("boost");
					writer.WriteValue(term.Boost);	
				}

				if (term.EnablePositionIncrements != default(bool))
				{
					writer.WritePropertyName("enable_position_increments");
					writer.WriteValue(term.EnablePositionIncrements.ToString().ToLower());
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
			return typeof(FieldQuery).IsAssignableFrom(objectType);
		}
	}
}