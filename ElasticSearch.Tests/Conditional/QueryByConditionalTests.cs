using System;
using System.Collections.Generic;
using System.Threading;
using ElasticSearch.Client;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Domain;
using ElasticSearch.Client.QueryString;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class QueryByConditionalTests
	{
		private string app = "app";
		private ElasticSearch.Client.ElasticSearchClient client;

		[Test]
		public void AllInOne()
		{
			string indexType = "v1";

			var dict = new Dictionary<string, object>();
			dict.Add("age", 22);
			var result =client.Index(app, indexType, "key1", dict);
			Assert.AreEqual(true, result.Success);

			var indexItem1 = new IndexItem("testKey");
			indexItem1.Add("age", 21);
			result = client.Index(app,  indexItem1);
			Assert.AreEqual(true, result.Success);

			var indexItem = new IndexItem(indexType, "key2");
			indexItem.Add("age", 23);
			result = client.Index(app,  indexItem);
			Assert.AreEqual(true, result.Success);

			client.Refresh();
			var count = client.Count(app,  indexType, ExpressionEx.Eq("age", 25));
			Assert.AreEqual(0, count);

			count = client.Count(app,  indexType, Conditional.Get(ExpressionEx.Eq("age", 22)));
			Assert.AreEqual(1, count);
			count = client.Count(app,  indexType, Conditional.Get(ExpressionEx.Eq("age", 23)));
			Assert.AreEqual(1, count);

			count = client.Count(app,  indexType, Conditional.Get(ExpressionEx.Between("age", 22, 23, true)));
			Assert.AreEqual(2, count);
            
            //a coplex example
            var cond1= Conditional.Get(ExpressionEx.Eq("name", "jack"))
                .And(ExpressionEx.Between("age",22,30))
                .And(ExpressionEx.Fuzzy("address","beijing",0.7f,4))
                .And(ExpressionEx.Le("no",87));
            Conditional cond2 = Conditional.Or(cond1, Conditional.Not(ExpressionEx.Eq("gender", "male")));
            client.Search(app, "type", cond2.Query);


		}
		[TestFixtureSetUp]
		public void Setup()
		{
			client=new ElasticSearchClient("localhost");
		}
		[TestFixtureTearDown]
		public void CleanUp()
		{
			client.DeleteIndex(app);
		}

		[Test]
		public void TestBulk()
		{
			var type = "bulk";
			List<IndexItem> indexItems = new List<IndexItem>();
			var indexItem = new IndexItem(type, "k1");
			indexItem.Add("Name", "medcl");
			indexItems.Add(indexItem);

			indexItem = new IndexItem(type, "k2");
			indexItem.Add("Name", "netease");
			indexItems.Add(indexItem);

			indexItem = new IndexItem("k3");
			indexItem.Add("Name", "sina");
			indexItems.Add(indexItem);

			var result = client.Index(app, indexItems);
			Assert.AreEqual(true, result.Success);
            
		}

		[Test]
		public void TestBulkWithDuplicatedFieldName()
		{
			var type = "bulk";
			IList<IndexItem> indexItems = new List<IndexItem>();
			var indexItem = new IndexItem(type, "kk1");
			indexItem.Add("Name", "medcl1");
			indexItem.Add("Name", "medcl2");
			indexItems.Add(indexItem);

			indexItem = new IndexItem(type, "kk2");
			indexItem.Add("Name", "网易");
			indexItem.Add("Name", "163");
			indexItems.Add(indexItem);

			var result = client.Index(app,  indexItems);
			Assert.AreEqual(true, result.Success);
		}

		[Test]
		public void TestServerException()
		{
			string indexType = "v2";

			var dict = new Dictionary<string, object>();
			dict.Add("age", 22);

			try
			{
				client.Index(app,  indexType, "key1", dict);
			}
			catch (System.Net.Sockets.SocketException e)
			{

				Console.WriteLine("can't connect to server.");
			}
		}

		[Test]
		public void TestFuzzyQuery()
		{
			ExpressionEx.Like("age", "张三").MinSimilarity(0.5f);
		}




	}
}