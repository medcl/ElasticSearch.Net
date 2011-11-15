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

		public Facets Facets;

		public void SetQuery(IQuery query)
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

		public void SetFacets()
		{
			
		}

	    public void AddFields(string[] fields)
	    {
            if (Fields == null) { Fields = new List<string>(); }
            Fields.AddRange(fields);
	    }
	}

	public class Facets
	{
//		public Dictionary<string ,IQuery> 

		public void AddFacet(string facertName,string filterClause)
		{
			
		}
	}

	public class Filter
	{
		public IQuery Query;
	}
}
