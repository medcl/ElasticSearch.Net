using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElasticSearch.Client.QueryDSL
{
    public enum Operator
    {
        OR,
        AND
    }

    /// <summary>
    /// A query that uses a query parser in order to parse its content.
    /// </summary>
    [JsonObject("query_string")]
    [JsonConverter(typeof(QueryStringConverter))]
    public class QueryStringQuery : IQuery
    {
        #region Properties

        /// <summary>
        /// The actual query to be parsed.
        /// </summary>
        public string Query { get; private set; }
        /// <summary>
        /// The default field for query terms if no prefix field is specified. Defaults to the _all field.
        /// </summary>
        [JsonProperty("default_field")]
        public string DefaultField { get; private set; }
        /// <summary>
        /// The query_string query can also run against multiple fields. The idea of running the query_string query against multiple 
        /// fields is by internally creating several queries for the same query string, each with default_field that match the fields provided. 
        /// Since several queries are generated, combining them can be automatically done either using a dis_max query or a simple bool query. 
        /// For example (the name is boosted by 5 using ^5 notation):
        /// </summary>
        public List<string> Fields { get; private set; }
        /// <summary>
        /// The default operator used if no explicit operator is specified. For example, 
        /// with a default operator of OR, the query capital of Hungary is translated to capital OR of OR Hungary, 
        /// and with default operator of AND, the same query is translated to capital AND of AND 
        /// Hungary. The default value is OR.
        /// </summary>
        [JsonProperty("default_operator")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Operator DefaultOperator { get; private set; }
        /// <summary>
        /// The analyzer name used to analyze the query string.
        /// </summary>
        public string Analyzer { get; private set; }
        /// <summary>
        /// When set, * or ? are allowed as the first character. Defaults to true.
        /// </summary>
        [JsonProperty("allow_leading_wildcard")]
        public bool AllowLeadingWildcard { get; private set; }
        /// <summary>
        /// Whether terms of wildcard, prefix, fuzzy, and range queries are to be automatically lower-cased or
        /// not (since they are not analyzed). Defaults to true.
        /// </summary>
        [JsonProperty("lowercase_expanded_terms")]
        public bool LowercaseExpendedTerms { get; private set; }
        /// <summary>
        /// Get wheter position increments are enabled in result queries. Defaults to true.
        /// </summary>
        [JsonProperty("enable_position_increments")]
        public bool EnablePositionIncrements { get; private set; }
        /// <summary>
        /// Get the prefix length for fuzzy queries. Default is 0.
        /// </summary>
        [JsonProperty("fuzzy_prefix_length")]
        public int FuzzyPrefixLength { get; private set; }
        /// <summary>
        /// Set the minimum similarity for fuzzy queries. Defaults to 0.5
        /// </summary>
        [JsonProperty("fuzzy_min_sim")]
        public double FuzzyMinimumSimilarity { get; private set; }
        /// <summary>
        /// Get the default slop for phrases. If zero, then exact phrase matches are required. Default value is 0.
        /// </summary>
        [JsonProperty("phrase_slop")]
        public double PhraseSlop { get; private set; }
        /// <summary>
        /// Get the boost value of the query. Defaults to 1.0.
        /// </summary>
        [JsonProperty("boost")]
        [DefaultValue(1.0)]
        public double Boost { get; private set; }

        /// <summary>
        /// Gets wheter the queries should be combined using dis_max (set it to true), or a bool query (set it to false). Defaults to true. (only used when using multiple fields)
        /// </summary>
        [JsonProperty("use_dis_max")]
        public bool UseDismax { get; private set; }
        /// <summary>
        /// When using dis_max, the disjunction max tie breaker. Defaults to 0. (only used when using multiple fields)
        /// </summary>
        [JsonProperty("tie_breaker")]
        public int TieBreaker { get; private set; }

        /// <summary>
        /// By default, wildcards terms in a query string are not analyzed. By setting this value to true, a best effort will be made to analyze those as well.
        /// </summary>
        [JsonProperty("analyze_wildcard")]
        public bool AnalyzeWildcard { get; set; }

        /// <summary>
        /// Default to false.
        /// </summary>
        [JsonProperty("auto_generate_phrase_queries")]
        public bool AutoGeneratePhraseQueries { get; set; }
        #endregion

        public QueryStringQuery(string query)
        {
            this.Query = query;
            this.DefaultOperator = Operator.OR;
            this.AllowLeadingWildcard = true;
            this.LowercaseExpendedTerms = true;
            this.EnablePositionIncrements = true;
            this.FuzzyMinimumSimilarity = 0.5;
            this.Boost = 1.0;
            this.UseDismax = true;
            this.TieBreaker = 0;
        }

        public QueryStringQuery(List<string> fields, string query)
        {
            Fields = fields;
            Query = query;
        }

        /// <summary>
        /// The default field for query terms if no prefix field is specified. 
        /// </summary>
        /// <param name="field">Defaults to the _all field.</param>
        /// <returns></returns>
        public QueryStringQuery SetDefaultField(string field)
        {
            this.DefaultField = field;
            return this;
        }
        /// <summary>
        /// The query_string query can also run against multiple fields. The idea of running the query_string query against multiple 
        /// fields is by internally creating several queries for the same query string, each with default_field that match the fields provided. 
        /// Since several queries are generated, combining them can be automatically done either using a dis_max query or a simple bool query. 
        /// </summary>
        /// <param name="fields">list with fields to perform the query on</param>
        /// <returns></returns>
        public QueryStringQuery SetFields(List<string> fields)
        {
            this.Fields = fields;
            return this;
        }

        public QueryStringQuery AddField(string field, int weight=10)
        {
            if (Fields == null) Fields = new List<string>();
            Fields.Add(string.Format("{0}^{1}", field, weight));
            return this;
        }
        /// <summary>
        ///  The default operator used if no explicit operator is specified. For example, 
        /// with a default operator of OR, the query capital of Hungary is translated to capital OR of OR Hungary, 
        /// and with default operator of AND, the same query is translated to capital AND of AND 
        /// Hungary.
        /// </summary>
        /// <param name="operator">The default value is OR</param>
        /// <returns></returns>
        public QueryStringQuery SetDefaultOperator(Operator @operator)
        {
            this.DefaultOperator = @operator;
            return this;
        }
        /// <summary>
        /// The analyzer name used to analyze the query string.
        /// </summary>
        /// <param name="analyzer">Custom analyzer name</param>
        /// <returns></returns>
        public QueryStringQuery SetAnalyzer(string analyzer)
        {
            this.Analyzer = analyzer;
            return this;
        }
        /// <summary>
        /// When set, * or ? are allowed as the first character.
        /// </summary>
        /// <param name="allowLeadingWildcard">Defaults to true.</param>
        /// <returns></returns>
        public QueryStringQuery SetAllowLeadingWildcard(bool allowLeadingWildcard)
        {
            this.AllowLeadingWildcard = allowLeadingWildcard;
            return this;
        }
        /// <summary>
        /// Whether terms of wildcard, prefix, fuzzy, and range queries are to be automatically lower-cased or
        /// not (since they are not analyzed).
        /// </summary>
        /// <param name="lowercaseExpendedTerms">Defaults to true.</param>
        /// <returns></returns>
        public QueryStringQuery SetLowercaseExpendedTerms(bool lowercaseExpendedTerms)
        {
            this.LowercaseExpendedTerms = lowercaseExpendedTerms;
            return this;
        }
        /// <summary>
        /// Set to true to enable position increments in result queries. 
        /// </summary>
        /// <param name="enablePositionIncrements">Defaults to true.</param>
        /// <returns></returns>
        public QueryStringQuery SetEnablePositionIncrements(bool enablePositionIncrements)
        {
            this.EnablePositionIncrements = enablePositionIncrements;
            return this;
        }
        /// <summary>
        /// Set to true to enable position increments in result queries. 
        /// </summary>
        /// <param name="fuzzyPrefixLength">Defaults to true.</param>
        /// <returns></returns>
        public QueryStringQuery SetFuzzyPrefixLength(int fuzzyPrefixLength)
        {
            this.FuzzyPrefixLength = fuzzyPrefixLength;
            return this;
        }
        /// <summary>
        /// Set the minimum similarity for fuzzy queries. 
        /// </summary>
        /// <param name="fuzzyMinimumSimilarity">Defaults to 0.5</param>
        /// <returns></returns>
        public QueryStringQuery SetFuzzyMinimumSimilarity(double fuzzyMinimumSimilarity)
        {
            this.FuzzyMinimumSimilarity = fuzzyMinimumSimilarity;
            return this;
        }
        /// <summary>
        /// Sets the default slop for phrases. If zero, then exact phrase matches are required. Default value is 0
        /// </summary>
        /// <param name="phraseSlop">Default value is 0</param>
        /// <returns></returns>
        public QueryStringQuery SetPhraseSlop(double phraseSlop)
        {
            this.PhraseSlop = phraseSlop;
            return this;
        }
        /// <summary>
        /// Sets the boost value of the query
        /// </summary>
        /// <param name="boost">defaults to 1.0 </param>
        /// <returns></returns>
        public QueryStringQuery SetBoost(double boost)
        {
            this.Boost = boost;
            return this;
        }
        /// <summary>
        /// Should the queries be combined using dis_max (set it to true), or a bool query (set it to false). 
        /// </summary>
        /// <param name="useDismax">Defaults to true</param>
        /// <returns></returns>
        public QueryStringQuery SetUseDisMax(bool useDismax)
        {
            this.UseDismax = useDismax;
            return this;
        }
        /// <summary>
        /// When using dis_max, the disjunction max tie breaker. 
        /// </summary>
        /// <param name="tieBreaker">Defaults to 0</param>
        /// <returns></returns>
        public QueryStringQuery SetTieBreaker(int tieBreaker)
        {
            this.TieBreaker = tieBreaker;
            return this;
        }

    }
}
