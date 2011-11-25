using System.Collections.Generic;
using ElasticSearch.Client;
using ElasticSearch.Client.Domain;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class BulkOperatesTests
	{
		[Test]
		public void TestBulk()
		{
			var client = new ElasticSearchClient("localhost");
			var fields = new Dictionary<string, object>();
			fields.Add("name", "jack");
			fields.Add("age", 25);
			var index = "index_1231231231";
			var result = client.Bulk(new List<BulkObject>() { new BulkObject() { Id = "1", Index = index, Type = "type", Fields = fields }, new BulkObject() { Id = "2", Index = index, Type = "type", Fields = fields }, new BulkObject() { Id = "3", Index = index, Type = "type", Fields = fields } });
			Assert.AreEqual(true, result.Success);

			result = client.Delete(index, "type", new string[] { "1", "2", "3" });
			Assert.AreEqual(true, result.Success);

			result=client.DeleteIndex(index);
			Assert.AreEqual(true, result.Success);
		}
		[Test]
		public void TestBulkIndexWithParentId()
		{
            var client = new ElasticSearchClient("localhost");
			var fields = new Dictionary<string, object>();
			fields.Add("name", "jack");
			fields.Add("age", 25);
			var index = "index_1231231231";
			var jsondata = JsonSerializer.Get(fields);

			var result = client.Bulk(new List<BulkObject>()
			                                               	{
			                                               		new BulkObject() { Id = "1", Index = index, Type = "type",ParentId = "1", JsonData = jsondata }, 
																new BulkObject() { Id = "2", Index = index, Type = "type",ParentId = "1", JsonData = jsondata }, 
																new BulkObject() { Id = "3", Index = index, Type = "type",ParentId = "1", JsonData = jsondata }
			                                               	});
			Assert.AreEqual(true, result.Success);

			result = client.Delete(index, "type", new string[] { "1", "2", "3" });
			Assert.AreEqual(true, result.Success);

			client.DeleteIndex(index);
		}
	}
}