using System;
using System.Threading;
using ElasticSearch.Client;
using ElasticSearch.Client.EMO;
using ElasticSearch.Client.Mapping;
using ElasticSearch.Client.QueryDSL;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class QueryDSLTests
	{
		string index = "index_search_operate" + Guid.NewGuid().ToString();
		ElasticSearchClient client = new ElasticSearchClient("localhost");
		[Test]
		public void TestQueryString()
		{
			//http://localhost:9200/index/type/_search?q=gender:False&sort=id&from=0
			var result =  client.QueryDSL.SearchByDSL(index, new string[] { "type" }, "gender:true", 0, 5);

			Assert.AreEqual(50, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);

			var query = new QueryString("gender:true");
			result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
			Assert.AreEqual(50, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);
		}

		[Test]
		public void TestTermQuery()
		{
			var query = new TermQuery("gender","true");
			var result = client.QueryDSL.Search(index, new string[] {"type"}, query, 0, 5);
			Assert.AreEqual(50, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);
		}



		[Test]
		public void TestTermQueryWithBoost()
		{
			var query = new TermQuery("gender", "true",2);
			var result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
			Assert.AreEqual(50, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);
		}



		[Test]
		public void TestTermsQuery()
		{
			var query = new TermsQuery("gender", "true","false");
			var result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
			Assert.AreEqual(100, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);
		}
		[Test]
		public void TestTermsQueryWithMinuMatch()
		{
			var query = new TermsQuery("gender",2, "true", "false");
			var result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
			Assert.AreEqual(0, result.GetTotalCount());
			Assert.AreEqual(0, result.GetHits().Hits.Count);
			
			
			var item = new IndexItem("type","addition_key1");
			item.Add("name", Guid.NewGuid().ToString());
			item.Add("id", 2012);
			item.Add("gender", true);
			item.Add("gender", false);
			client.Index(index, item);

			Thread.Sleep(1000);
			result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
			Assert.AreEqual(1, result.GetTotalCount());
			Assert.AreEqual(1, result.GetHits().Hits.Count);

		}


		[Test]
		public void TestWildcardQuery()
		{
			var item = new IndexItem("type","addition_key2");
			item.Add("name", "张");
			client.Index(index, item);

			 item = new IndexItem("type","addition_key3");
			item.Add("name", "张三");
			client.Index(index, item);

			item = new IndexItem("type", "addition_key4");
			item.Add("name", "张三丰");
			client.Index(index, item);

			item = new IndexItem("type","addition_key5");
			item.Add("name", "二张三张");
			client.Index(index, item);

			Thread.Sleep(1000);

			var query = new WildcardQuery("name","张*");
			var result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
			Assert.AreEqual(3, result.GetTotalCount());
			Assert.AreEqual(3, result.GetHits().Hits.Count);
			foreach (var VARIABLE in result.GetHits().Hits)
			{
				Console.WriteLine(VARIABLE.Fields["name"]);
			}

			Console.WriteLine("--");
			query = new WildcardQuery("name", "张三*");
			result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
			Assert.AreEqual(2, result.GetTotalCount());
			Assert.AreEqual(2, result.GetHits().Hits.Count);
			foreach (var VARIABLE in result.GetHits().Hits)
			{
				Console.WriteLine(VARIABLE.Fields["name"]);
			}

		}

		[TestFixtureSetUp]
		public void Init()
		{
			var typesetting = new TypeSetting("type");
			typesetting.AddFieldSetting("name", new StringFieldSetting() { Index = IndexType.not_analyzed });
			typesetting.AddFieldSetting("id", new NumberFieldSetting() { });
			typesetting.AddFieldSetting("gender", new BooleanFieldSetting() { Index = IndexType.not_analyzed });

			client.Index(index, "type", "_medcl", "{}");
			client.PutMapping(index, typesetting);
			
			for (int i = 0; i < 100; i++)
			{
				var item = new IndexItem("type", i.ToString());
				item.Add("name", Guid.NewGuid().ToString());
				item.Add("id", i);
				if (i >= 50)
				{
					item.Add("gender", true);
				}
				else
				{
					item.Add("gender", false);
				}
	
			
			client.Index(index,item);
			}
			Thread.Sleep(1000);
		}


		[TestFixtureTearDown]
		public void Cleanup()
		{
			client.DeleteIndex(index);
		}
	}
}