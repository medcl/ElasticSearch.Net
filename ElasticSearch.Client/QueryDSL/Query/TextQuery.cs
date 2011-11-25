using Newtonsoft.Json;

namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// A family of text queries that accept text, analyzes it, and constructs a query out of it
	/// </summary>
	[JsonObject("text")]
	[JsonConverter(typeof(TextQueryConverter))]
	public class TextQuery:IQuery
	{
		public string Field;
		public string Text;
		/// <summary>
		/// The default text query is of type boolean. It means that the text provided is analyzed and the analysis process constructs a boolean query from the provided text. The operator flag can be set to or or and to control the boolean clauses (defaults to or).
		/// </summary>
		public Operator Operator;
		/// <summary>
		/// The analyzer can be set to control which analyzer will perform the analysis process on the text. It default to the field explicit mapping definition, or the default search analyzer.
		/// </summary>
		public string Analyzer;

		/// <summary>
		/// fuzziness can be set to a value (depending on the relevant type, for string types it should be a value between 0.0 and 1.0) to constructs fuzzy queries for each term analyzed
		/// </summary>
		public float Fuzziness;

		/// <summary>
		/// The prefix_length and max_expansions can be set in this case to control the fuzzy process
		/// </summary>
		public int PrefixLength;

		/// <summary>
		/// max_expansions parameter that can control to how many prefixes the last term will be expanded. It is highly recommended to set it to an acceptable value to control the execution time of the query.
		/// </summary>
		public int MaxExpansions;

		/// <summary>
		///phrase
		///boolean
		///phrase_prefix
		/// </summary>
		public string QueryType;

		public float Boost;

		public int Slop;

		public TextQuery(string field, string text, Operator oPperator = Operator.OR, string analyzer = null)
		{
			Field = field;
			Text = text;
			Operator = oPperator;
			Analyzer = analyzer;
		}
	}
}