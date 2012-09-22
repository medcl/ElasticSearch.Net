using Lucene.Net.Index;
using Lucene.Net.Search;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class QueryTests
	{
		[Test,Ignore]
		public void TestQuery()
		{
			BooleanQuery booleanQuery=new BooleanQuery();
			booleanQuery.Add(new TermQuery(new Term("name","medcl")),BooleanClause.Occur.MUST);
			booleanQuery.Add(new TermQuery(new Term("age","25")),BooleanClause.Occur.MUST);

			new ElasticSearch.Client.ElasticSearchClient("localhost").Search("index", "type", booleanQuery.ToString());
		}
	}
}