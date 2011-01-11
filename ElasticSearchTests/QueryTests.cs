using Lucene.Net.Index;
using Lucene.Net.Search;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class QueryTests
	{
		[Test]
		public void TestQuery()
		{
			Lucene.Net.Search.BooleanQuery booleanQuery=new BooleanQuery();
			booleanQuery.Add(new TermQuery(new Term("name","medcl")),BooleanClause.Occur.MUST);
			booleanQuery.Add(new TermQuery(new Term("age","25")),BooleanClause.Occur.MUST);

			ElasticSearch.Client.ElasticSearchClient.Instance.Search("index", "type", booleanQuery.ToString());
		}
	}
}