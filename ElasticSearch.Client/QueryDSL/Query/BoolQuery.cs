using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// A query that matches documents matching boolean combinations of other queries. The bool query maps to Lucene BooleanQuery. It is built using one or more boolean clauses, each clause with a typed occurrence. 
	/// The bool query also supports disable_coord parameter (defaults to false).
	/// </summary>
	[JsonObject("bool")]
	[JsonConverter(typeof(BoolQueryConverterer))]
	public class BoolQuery:IQuery
	{
		internal List<IQuery> ShouldQueries;
		internal List<IQuery> MustQueries;
		internal List<IQuery> MustNotQueries;
		[DefaultValue(1)]
		internal int MinimumNumberShouldMatch=1;
		[DefaultValue(1.0)]
		internal double Boost=1.0;
		[DefaultValue(false)]
		internal bool DisableCoord=false;
		public BoolQuery Must(IQuery query)
		{
			if (MustQueries == null)
			{
				MustQueries = new List<IQuery>();
			}
			MustQueries.Add(query); 
			return this;
		}

		public BoolQuery Should(IQuery query)
		{
			if (ShouldQueries == null)
			{
				ShouldQueries = new List<IQuery>();
			}
			ShouldQueries.Add(query);
			return this;
		}

		public BoolQuery MustNot(IQuery query)
		{
			if (MustNotQueries == null)
			{
				MustNotQueries = new List<IQuery>();
			}
			MustNotQueries.Add(query);
			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number">default 1</param>
		/// <returns></returns>
		public BoolQuery SetMinimumNumberShouldMatch(int number)
		{
			MinimumNumberShouldMatch = number;
			return this;
		}

		/// <summary>
		/// default 1.0
		/// </summary>
		/// <param name="boost"></param>
		/// <returns></returns>
		public BoolQuery SetBoost(double boost)
		{
			Boost = boost;
			return this;
		}
	}
}