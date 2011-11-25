using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// Fuzzy like this query find documents that are ¡°like¡± provided text by running it against one or more fields
	/// </summary>
	[JsonObject("fuzzy_like_this")]
	[JsonConverter(typeof(FuzzyLikeThisQueryConverter))]
	public class FuzzyLikeThisQuery :IQuery
	{
		public List<string> Fields;
		public string LikeText;
		public int MaxQueryTerms=25;
		public bool IgnoreTermFrequency ;
		public float MinSimilarity=0.5f;
		public int PrefixLength;
		public float Boost;
		public string Analyzer;

		public FuzzyLikeThisQuery(List<string > fields,string likeText, int maxQueryTerms,bool ignoreTermFrequency,float minSimilarity,int prefixLenght,float boost,string analyzer)
		{
			Fields = fields;
			LikeText = likeText;
			MaxQueryTerms = maxQueryTerms;
			IgnoreTermFrequency = ignoreTermFrequency;
			MinSimilarity = minSimilarity;
			PrefixLength = prefixLenght;
			Boost = boost;
			Analyzer = analyzer;
		}

		public FuzzyLikeThisQuery(List<string > fields,string likeText):this(fields,likeText,25,false,0.5f,0,1,null)
		{
			
		}

		public FuzzyLikeThisQuery(string field,string likeText):this(new List<string>(){field},likeText )
		{
			
		}
	}
}