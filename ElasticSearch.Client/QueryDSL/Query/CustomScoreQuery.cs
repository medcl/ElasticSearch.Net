using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// custom_score query allows to wrap another query and customize the scoring of it optionally with a computation derived from other field values in the doc (numeric ones) using script expression
	/// </summary>
	[JsonObject("custom_score")]
	[JsonConverter(typeof(CustomScoreQueryConverter))]
	public class CustomScoreQuery:IQuery
	{
		public IQuery Query;
		public string Script;
		public Dictionary<string, object> Params;
		public CustomScoreQuery(IQuery query,string script,Dictionary<string,object> param)
		{
			Query = query;
			Script = script;
			Params = param;
		}
	}
}