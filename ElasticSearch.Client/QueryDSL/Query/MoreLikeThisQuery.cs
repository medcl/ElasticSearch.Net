using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	[JsonObject("more_like_this")]
	[JsonConverter(typeof(MoreLikeThisQueryConverter))]
	public class MoreLikeThisQuery:IQuery
	{
		public List<string> Fields;
		public string LikeText;
		public int MinTermFreq;
		public int MaxQueryTerms;
		public float PercentTermsToMatch;
		public List<string> StopWords;
		public int MinDocFreq;
		public int MaxDocFreq;
		public int MinWordLen;
		public int MaxWordLen;
		public int BoostTerms;
		public float Boost;
		public string Analyzer;

		public MoreLikeThisQuery(List<string> fields, string liketext, int minTermFreq=2, int maxQueryTerms=25)
		{
			Fields = fields;
			LikeText = liketext;
			MinTermFreq = minTermFreq;
			MaxQueryTerms = maxQueryTerms;
		}

	}
}