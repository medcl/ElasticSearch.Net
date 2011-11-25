using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// A fuzzy based query that uses similarity based on Levenshtein (edit distance) algorithm.
	/// </summary>
	[JsonObject("fuzzy")]
	[JsonConverter(typeof(FuzzyQueryConverter))]
	public class FuzzyQuery:IQuery
	{
		public string Field;
		public string Value;
		public float Boost=1.0f;
		public string MinSimilarity;
		public int PrefixLength=0;

		public FuzzyQuery(string field,string value)
		{
			Field = field;
			Value = value;
		}

		public void SetMinSimilarity(int minSimilarity)
		{
			MinSimilarity = minSimilarity.ToString();
		}

	}
}