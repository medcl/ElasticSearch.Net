using System;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal class ElasticQueryConverterer : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ElasticQuery term = (ElasticQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();

				writer.WriteRaw("\"from\": " + term.From);
				writer.WriteRaw(",\"size\": " + term.Size + ",");
				writer.WritePropertyName("query");
				serializer.Serialize(writer, term.Query);

				if (term.Fields != null && term.Fields.Count > 0)
				{
					writer.WritePropertyName("fields");
					writer.WriteStartArray();
					foreach (var field in term.Fields)
					{
						writer.WriteRawValue("\"" + field + "\"");
					}
					writer.WriteEndArray();
				}
				if (term.SortItems != null && term.SortItems.Count > 0)
				{
					writer.WritePropertyName("sort");
					writer.WriteStartObject();
					foreach (var sortItem in term.SortItems)
					{
					    if (sortItem != null)
					    {
					        writer.WritePropertyName(sortItem.FieldName);
					        writer.WriteValue(sortItem.SortType.ToString().ToLower());
					    }
					}

					writer.WriteEndObject();
				}

                //facets
                if (term.Facets != null)
                {
                    writer.WritePropertyName("facets");
                    serializer.Serialize(writer, term.Facets);
                }

                //hightlight
                if(term.Hightlight!=null)
                {
                    writer.WritePropertyName("highlight");
                    serializer.Serialize(writer,term.Hightlight);
                }

                if (term.Explain)
                {
                    writer.WriteRaw(",\"explain\": " + term.Explain.ToString().ToLower());
                }

				writer.WriteEndObject();
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(ElasticQuery).IsAssignableFrom(objectType);
		}
	}
}