using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	[JsonObject("range")]
	[JsonConverter(typeof(RangeQueryConverter))]
	public class RangeQuery:IQuery
	{
		public string Field;
		public string From;
		public string To;
		public bool IncludeLower;
		public bool IncludeUpper;
		public bool Cache;

		public RangeQuery(string fileld, string from, string to, bool includeLower, bool includeUpper)
		{
			Field = fileld;
			From = from;
			To = to;
			IncludeLower = includeLower;
			IncludeUpper = includeUpper;
		}

		public RangeQuery SetCache(bool cache)
		{
			Cache = cache;
			return this;
		}  
	}



}