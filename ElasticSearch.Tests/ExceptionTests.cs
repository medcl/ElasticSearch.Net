using ElasticSearch.Client;
using ElasticSearch.Client.Exception;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class ExceptionTests
	{
		ElasticSearchClient client = new ElasticSearch.Client.ElasticSearchClient("localhost");
		[Test, Ignore]
		[ExpectedException(typeof(IndexMissingException))]
		public void TestIndexMissingException()
		{
			
			client.Search("undefinedIndex", "match_all", 0, 5);
		}

		//0.16 之后对异常处理比较完善，typemissing已解决
		[Test, Ignore]
		[ExpectedException(typeof(TypeMissingException))]
		public void TestTypeMissingException()
		{
			client.CreateIndex("notypIndex");
			client.Search("notypIndex", "typeA", "match_all");
		}
		public void TestCleanUp()
		{
			client.DeleteIndex("notypIndex");
		}

		[Test]
		public void TestDoubleDelete()
		{
			client.CreateIndex("hello");
			client.DeleteByQueryString("hello", "*");
			//			client.Delete("hello","type", "1");
			client.DeleteIndex("hello");
		}
	}
}