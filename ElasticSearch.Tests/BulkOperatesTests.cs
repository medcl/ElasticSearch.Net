using System;
using System.Collections.Generic;
using ElasticSearch.Client;
using ElasticSearch.Client.Config;
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
			
			var index = "index_123123123121";
			try
			{
				client.DeleteIndex(index);
				client.CreateIndex(index);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			var result = client.Bulk(new List<BulkObject>()
			                         	{
			                         		new BulkObject() { Id = "1", Index = index, Type = "type", Fields = fields }, 
											new BulkObject() { Id = "2", Index = index, Type = "type", Fields = fields }, 
											new BulkObject() { Id = "3", Index = index, Type = "type", Fields = fields }
			                         	});
			Assert.AreEqual(true, result.Success);

			client.Refresh();
			var c = client.Count(index, "type", "age:25");
			Assert.AreEqual(3, c);

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
			var index = "index_12312312311";
			try
			{
				client.DeleteIndex(index);
				client.CreateIndex(index);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			var jsondata = JsonSerializer.Get(fields);

			var result = client.Bulk(new List<BulkObject>()
			                                               	{
			                                               		new BulkObject() { Id = "1", Index = index, Type = "type",ParentId = "1", JsonData = jsondata }, 
																new BulkObject() { Id = "2", Index = index, Type = "type",ParentId = "1", JsonData = jsondata }, 
																new BulkObject() { Id = "3", Index = index, Type = "type",ParentId = "1", JsonData = jsondata }
			                                               	});
			Assert.AreEqual(true, result.Success);
			client.Refresh();
			var c=client.Count(index, "type", "age:25");
			Assert.AreEqual(3,c);
			result = client.Delete(index, "type", new string[] { "1", "2", "3" },"1");
			Assert.AreEqual(true, result.Success);
			client.Refresh();
			c = client.Count(index, "type", "age:25");
			Assert.AreEqual(0, c);
			client.DeleteIndex(index);
		}

		[Test]
		public void TestBulkForFramdThrift()
		{
			var client = new ElasticSearchClient("localhost");

			var fields = new Dictionary<string, object>();
			fields.Add("name", "jack");
			fields.Add("age", 25);
			var index = "index_bulk_framed";
			try
			{
				client.DeleteIndex(index);
				client.CreateIndex(index);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			var jsondata = JsonSerializer.Get(fields);

			var result = client.Bulk(new List<BulkObject>()
			                                               	{
			                                               		new BulkObject() { Id = "1", Index = index, Type = "type", JsonData = jsondata }, 
			                                               		new BulkObject() { Id = "2", Index = index, Type = "type", JsonData = jsondata }, 
			                                               		new BulkObject() { Id = "3", Index = index, Type = "type", JsonData = jsondata }, 
																new BulkObject() { Id = "4", Index = index, Type = "type", JsonData = jsondata }, 
																new BulkObject() { Id = "5", Index = index, Type = "type",ParentId = "1", JsonData = jsondata },
																new BulkObject() { Id = "6", Index = index, Type = "type",ParentId = "1", JsonData = jsondata },
																new BulkObject() { Id = "7", Index = index, Type = "type",ParentId = "1", JsonData = jsondata },
																new BulkObject() { Id = "8", Index = index, Type = "type",ParentId = "1", JsonData = jsondata },
			                                               	});
			Assert.AreEqual(true, result.Success);
		}
	}
}