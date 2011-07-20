using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	public class TermQueryConvert:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			TermQuery term = (TermQuery)value;
			if (term != null)
				writer.WriteRaw(string.Format("{{ \"{0}\" : {{ \"term\" : \"{1}\", \"boost\":{2} }}}}",term.Field,term.Value,term.Boost));
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

	/// <summary>
	/// Matches documents that have fields that contain a term (not analyzed). The term query maps to Lucene TermQuery. 
	/// </summary>
	[JsonObject("term")]
	[JsonConverter(typeof(TermQueryConvert))]
	public class TermQuery:IQuery
	{
		public string Field { get; set; }
		public object Value { get; set; }
		[JsonProperty("boost")]
		[DefaultValue(1.0)]
		public double Boost { get; set; }

		public TermQuery(string field, object value,double boost=1.0)
		{
			Field = field;
			Value = value;
			Boost = boost;
		}
	}
}