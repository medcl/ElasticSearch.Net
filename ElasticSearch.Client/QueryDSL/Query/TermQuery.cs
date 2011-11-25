using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Matches documents that have fields that contain a term (not analyzed). The term query maps to Lucene TermQuery. 
	/// </summary>
	[JsonObject("term")]
	[JsonConverter(typeof(TermQueryConverter))]
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

		public TermQuery SetBoost(double boost)
		{
			Boost = boost;
			return this;
		}
	

    }


	public class SpanQuery:IQuery
	{
		
	}

	/// <summary>
	/// Matches spans near the beginning of a field. The span first query maps to Lucene SpanFirstQuery
	/// </summary>
	[JsonObject("span_first")]
	[JsonConverter(typeof(SpanFirstQueryConverter))]
	public class SpanFirstQuery : SpanQuery
	{
		
	}

	internal class SpanFirstQueryConverter
	{
	}

	/// <summary>
	/// Matches spans which are near one another. One can specify slop, the maximum number of intervening unmatched positions, as well as whether matches are required to be in-order. The span near query maps to Lucene SpanNearQuery
	/// </summary>
	[JsonObject("span_near")]
	[JsonConverter(typeof(SpanNearQueryConverter))]
	public class SpanNearQuery : SpanQuery
	{
		public List<SpanTermQuery> Clauses;

		public SpanNearQuery(SpanTermQuery spanTerm)
		{
			Clauses=new List<SpanTermQuery>();
			Clauses.Add(spanTerm);
		}

		public SpanNearQuery Or(SpanTermQuery termQuery)
		{
			Clauses.Add(termQuery);
			return this;
		}

		public int Slop;
		public bool InOrder;
		public bool CollectPayloads;
	}

	internal class SpanNearQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			SpanNearQuery term = (SpanNearQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("span_near");
				writer.WriteStartObject();

				writer.WritePropertyName("clauses");
				writer.WriteStartArray();
				foreach (var spanTermQuery in term.Clauses)
				{
					serializer.Serialize(writer, spanTermQuery);
				}
				writer.WriteEndArray();

				writer.WritePropertyName("slop");
				serializer.Serialize(writer, term.Slop);

				writer.WritePropertyName("in_order");
				serializer.Serialize(writer, term.InOrder.ToString().ToLower());

				writer.WritePropertyName("collect_payloads");
				serializer.Serialize(writer, term.CollectPayloads.ToString().ToLower());

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
			return typeof(SpanNearQuery).IsAssignableFrom(objectType);
		}
	}

	/// <summary>
	/// Removes matches which overlap with another span query. The span not query maps to Lucene SpanNotQuery
	/// </summary>
	[JsonObject("span_not")]
	[JsonConverter(typeof(SpanNotQueryConverter))]
	public class SpanNotQuery : SpanQuery
	{
		public SpanQuery Include;
		public SpanQuery Exclude;
	}


	internal class SpanNotQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			SpanNotQuery term = (SpanNotQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("span_not");
				writer.WriteStartObject();
				writer.WritePropertyName("include");
				serializer.Serialize(writer, term.Include);

				writer.WritePropertyName("exclude");
				serializer.Serialize(writer, term.Exclude);
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
			return typeof(SpanNotQuery).IsAssignableFrom(objectType);
		}
	}

	/// <summary>
	/// Matches the union of its span clauses. The span or query maps to Lucene SpanOrQuery
	/// </summary>
	[JsonObject("span_or")]
	[JsonConverter(typeof(SpanOrQueryConverter))]
	public class SpanOrQuery : SpanQuery
	{
		public List<SpanTermQuery> Clauses;

		public SpanOrQuery(SpanTermQuery spanTerm)
		{
			Clauses=new List<SpanTermQuery>();
			Clauses.Add(spanTerm);
		}

		public SpanOrQuery Or(SpanTermQuery termQuery)
		{
			Clauses.Add(termQuery);
			return this;
		}
	}

	internal class SpanOrQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			SpanOrQuery term = (SpanOrQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("span_or");
				writer.WriteStartObject();
				writer.WritePropertyName("clauses");
				writer.WriteStartArray();
				foreach (var spanTermQuery in term.Clauses)
				{
					serializer.Serialize(writer, spanTermQuery);
				}
				writer.WriteEndArray();
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
			return typeof(SpanOrQuery).IsAssignableFrom(objectType);
		}
	}

	/// <summary>
	/// Matches spans containing a term. The span term query maps to Lucene SpanTermQuery
	/// </summary>
	[JsonObject("span_term")]
	[JsonConverter(typeof(SpanTermQueryConverter))]
	public class SpanTermQuery : SpanQuery
	{
		public float Boost;
		public string Field;
		public string Value;

		public SpanTermQuery(string field, string value)
		{
			Field = field;
			Value = value;
		}
	}

	internal class SpanTermQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			SpanTermQuery term = (SpanTermQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("span_term");
				writer.WriteStartObject();
				writer.WritePropertyName(term.Field);
				writer.WriteStartObject();

				writer.WritePropertyName("value");
				writer.WriteValue(term.Value);

				if (term.Boost > 0)
				{
					writer.WritePropertyName("boost");
					writer.WriteValue(term.Boost);
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
			return typeof(SpanTermQuery).IsAssignableFrom(objectType);
		}
	}
}