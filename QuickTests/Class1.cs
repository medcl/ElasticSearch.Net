using System;
using ElasticSearch;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class Class1
	{
		[Test]
		public void SimpleTests()
		{
			var result = ESClient.Instance.Index("testindex", "testtype", "testkey", "{\"a\":\"b\"}");
			var doc = ESClient.Instance.Search("testindex", "testtype", "_id:testkey");
			Console.WriteLine(doc.JsonString);
			Assert.AreEqual(1, doc.hits.hits.Count);
			Assert.AreEqual("b", doc.hits.hits[0]._source["a"]);
			ESClient.Instance.Delete("testindex", "testtype", "testkey");
			doc = ESClient.Instance.Get("testindex", "testtype", "testkey");
			Assert.AreEqual(null, doc.hits);
		}
	}
}
