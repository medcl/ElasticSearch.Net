using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	[JsonConverter(typeof(ElasticQueryConverter))]
	public class ElasticQuery
	{
		public ElasticQuery(int from,int size,bool explatin=false)
		{
			From = from;
			Size = size;
			Explain = explatin;
		}

		[DefaultValue(0)]
		public int From { get; private set; }
		[DefaultValue(10)]
		public int Size { get; private set; }
		[DefaultValue(false)]
		public bool Explain { set; get; }

		public List<string> Fields;

		public IQuery Query;

		public void AddQuery(IQuery query)
		{
			Query = query;

//			//TODO Top Children Query  Nested Query
//			if(query is QueryString)
//			{
//				Query.Add("query_string",query);
//				return;
//			}
//			if(query is TermQuery)
//			{
//				Query.Add("term", query);
//				return;
//			}
//			if (query is TermsQuery)
//			{
//				Query.Add("terms", query); //terms or in
//				return;
//			}
//			if (query is WildcardQuery)
//			{
//				Query.Add("wildcard", query);
//				return;
//			}
//			if (query is BoolQuery)
//			{
//				Query.Add("bool", query);
//				return;
//			}
//			throw new NotSupportedException();
		}

		public void AddField(string field)
		{
			if(Fields==null){Fields=new List<string>();}
			Fields.Add(field);
		}
	}

	public class ElasticQueryConverter:JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ElasticQuery term = (ElasticQuery)value;
			if (term != null)
			{
				writer.WriteStartObject();
				writer.WritePropertyName("query");
				serializer.Serialize(writer,term.Query);

				writer.WritePropertyName("explain");
				writer.WriteValue(term.Explain);

				writer.WritePropertyName("from");
				writer.WriteValue(term.From);

				writer.WritePropertyName("size");
				writer.WriteValue(term.Size);

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
