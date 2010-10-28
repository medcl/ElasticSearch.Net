using System;
using System.Threading;
using ElasticSearch.Client;
using ElasticSearch.Mapping;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class QueryDSLTests
	{
		string index = "index_search_operate" + Guid.NewGuid().ToString();

		[Test]
		public void TestQuery()
		{
			//http://localhost:9200/index/type/_search?q=gender:False&sort=id&from=0
			var result =  ElasticSearchClient.Instance.SearchByDSL(index, new string[] { "type" }, "gender:true", 0, 5);

			Assert.AreEqual(50, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);
		}

		[TestFixtureSetUp]
		public void Init()
		{
			var typesetting = new TypeSetting("type");
			typesetting.AddFieldSetting("name", new StringFieldSetting() { Index = IndexType.not_analyzed });
			typesetting.AddFieldSetting("id", new NumberFieldSetting() { });
			typesetting.AddFieldSetting("gender", new BooleanFieldSetting() { Index = IndexType.not_analyzed });

			ElasticSearchClient.Instance.Index(index, "type", "_medcl", "{}");
			ElasticSearchClient.Instance.PutMapping(index, typesetting);
			
			for (int i = 0; i < 100; i++)
			{
				var item = new IndexItem("type", i.ToString());
				item.Add("name", new Guid().ToString());
				item.Add("id", i);
				if (i >= 50)
				{
					item.Add("gender", true);
				}
				else
				{
					item.Add("gender", false);
				}
	
			
			ElasticSearchClient.Instance.Index(index,item);
			}
			Thread.Sleep(2000);
		}


		[TestFixtureTearDown]
		public void Cleanup()
		{
			ElasticSearchClient.Instance.DeleteIndex(index);
		}
	}
}