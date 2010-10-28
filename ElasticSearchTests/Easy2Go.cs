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

			var result = ElasticSearchClient.Instance.Index(indexName, indexType, "testkey", "{\"a\":\"b\"}");
			Assert.AreEqual(true, result.Success);

			ElasticSearchClient.Instance.Refresh(indexName);

			var doc = ElasticSearchClient.Instance.Search(indexName, indexType, "_id:testkey");
			Console.WriteLine(doc.JsonString);
			Assert.AreEqual(1, doc.GetHits().Hits.Count);
			Assert.AreEqual("b", doc.GetHits().Hits[0].Fields["a"]);

			ElasticSearchClient.Instance.Delete(indexName, indexType, "testkey");

			ElasticSearchClient.Instance.Refresh(indexName);

			var doc1 = ElasticSearchClient.Instance.Get(indexName, indexType, "testkey");
			Assert.AreEqual(0, doc1.GetFields().Count);

			ElasticSearchClient.Instance.DeleteIndex(indexName);
		}
	}
}
