using System;
using ElasticSearch;
using ElasticSearch.Client;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class Easy2Go
	{
		[Test]
		public void SimpleTests()
		{
			var indexName = "myindex_" + Guid.NewGuid();
			var indexType = "type";

			var client = new ElasticSearchClient("localhost");

			var result = client.Index(indexName, indexType, "testkey", "{\"a\":\"b\",\"c\":\"d\"}");
			Assert.AreEqual(true, result.Success);

			client.Refresh(indexName);

			var doc = client.Search(indexName, indexType, "c:d");
			Console.WriteLine(doc.Response);
			Assert.AreEqual(1, doc.GetHits().Hits.Count);
			Assert.AreEqual("b", doc.GetHits().Hits[0].Source["a"]);

			client.Delete(indexName, indexType, "testkey");

			client.Refresh(indexName);

			var doc1 = client.Get(indexName, indexType, "testkey");
			Assert.AreEqual(null,doc1);

			client.DeleteIndex(indexName);
		}
	}
}
