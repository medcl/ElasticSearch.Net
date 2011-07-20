using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ElasticSearch.Client.QueryDSL
{
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

		public Dictionary<string, IQuery> Query = new Dictionary<string, IQuery>();

		public void AddQuery(IQuery query)
		{
			if(query is QueryString)
			{
				Query.Add("query_string",query);
				return;
			}
			if(query is TermQuery)
			{
				Query.Add("term", query);
				return;
			}
			if (query is TermsQuery)
			{
				Query.Add("terms", query); //terms or in
				return;
			}
			if (query is WildcardQuery)
			{
				Query.Add("wildcard", query);
				return;
			}
			throw new NotSupportedException();
		}

		public void AddField(string field)
		{
			if(Fields==null){Fields=new List<string>();}
			Fields.Add(field);
		}
	}
}
