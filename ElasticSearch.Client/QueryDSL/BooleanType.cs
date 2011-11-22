namespace ElasticSearch.Client.QueryDSL
{
	/// <summary>
	/// must:The clause (query) must appear in matching documents.
	/// should:The clause (query) should appear in the matching document. A boolean query with no must clauses, one or more should clauses must match a document. The minimum number of should clauses to match can be set using minimum_number_should_match parameter.
	/// must_not:The clause (query) must not appear in the matching documents. N ote that it is not possible to search on documents that only consists of a must_not clauses
	/// </summary>
	enum BooleanType
	{
		Must,
		Should,
		MustNot
	}
}