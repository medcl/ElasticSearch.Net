using System;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	internal  class TermsQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			TermsQuery term = (TermsQuery)value;
			//{    "terms" : {        "tags" : [ "blue", "pill" ],        "minimum_match" : 1    }}
			if (term != null)
			{

			    var stringBuilder = new StringBuilder();

                stringBuilder.Append("{    \"terms\" : {        \"" + term.Field + "\" : [");

			    var i = 0;
				foreach(var t in term.Values)
				{
                    if(i>0)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append("\""+t+"\"");
				    i++;
				}

                stringBuilder.Append("],  \"minimum_match\" : "+term.MinimumMatch+"    }}");

                writer.WriteRaw(stringBuilder.ToString());
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