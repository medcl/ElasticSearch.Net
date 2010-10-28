using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ElasticSearch.DSL
{
	public class ElasticQuery
	{
		public ElasticQuery(int from,int size,bool explatin=false)
		{
			From = from;
			Size = size;
			Explain = explatin;
		}

		public int From { get; private set; }
		public int Size { get; private set; }
		public bool Explain { set; get; }
	
		public List<string > Fields=new List<string>();

		public Dictionary<string, IQuery> Query = new Dictionary<string, IQuery>();

		public void AddQuery(IQuery query)
		{
			if(query is QueryString)
			{
				Query.Add("query_string",query);
				return;
			}
			throw new NotSupportedException();
		}

	}
}
