using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Filters documents that only have the provided ids. Note, this filter does not require the _id field to be indexed since it works using the _uid field
	/// </summary>
	[JsonObject("ids")]
	[JsonConverter(typeof(IdsQueryConverterer))]
	public class IdsQuery:IQuery
	{
		public List<string> Types;
		public List<string> Values;

		public IdsQuery(string[] type, params string[] ids)
		{
			Types = new List<string>(type);
			Values = new List<string>(ids);
		}

		public IdsQuery(string type, params string[] ids)
		{
			Types = new List<string>();
			Types.Add(type);
			Values = new List<string>(ids);
		}

		public void AddIds(params string[] ids)
		{
			Values.AddRange(ids);
		}

		public void AddType(string type)
		{
			Types.Add(type);
		}
	}
}