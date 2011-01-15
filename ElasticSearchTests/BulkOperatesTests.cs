using System.Collections.Generic;
using ElasticSearch.Client;
using ElasticSearch.Client.EMO;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class BulkOperatesTests
	{
		[Test]
		public void TestBulk()
		{
			var fields = new Dictionary<string, object>();
			fields.Add("name", "jack");
			fields.Add("age", 25);
			var index = "index_1231231231";
			var result = ElasticSearchClient.Instance.Bulk(new List<BulkObject>() { new BulkObject() { Id = "1", Index = index, Type = "type", Fields = fields }, new BulkObject() { Id = "2", Index = index, Type = "type", Fields = fields }, new BulkObject() { Id = "3", Index = index, Type = "type", Fields = fields } });
			Assert.AreEqual(true, result.Success);

			result = ElasticSearchClient.Instance.Delete(index, "type", new string[] { "1", "2", "3" });
			Assert.AreEqual(true, result.Success);

			result=ElasticSearchClient.Instance.DeleteIndex(index);
			Assert.AreEqual(true, result.Success);
		}
	}
}