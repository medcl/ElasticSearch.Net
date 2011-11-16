using System;
using System.Diagnostics;
using System.Threading;
using ElasticSearch.Client;
using ElasticSearch.Client.EMO;
using ElasticSearch.Client.Mapping;
using ElasticSearch.Client.QueryDSL;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class QueryDSLTests
	{
		string index = "index_search_operate" + Guid.NewGuid().ToString();
		ElasticSearchClient client = new ElasticSearchClient("localhost");

	    #region TestQuery

	    [Test]
	    public void TestQueryString()
	    {
	        var query = new QueryString("gender:true");
	        var result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
	        Assert.AreEqual(50, result.GetTotalCount());
	        Assert.AreEqual(5, result.GetHits().Hits.Count);
	    }

        [Test]
        public void TestConstantSocreWithTermQuery()
        {
            var query = new TermQuery("gender", "true");

            var constanQuery = new ConstantScoreQuery(query);
            
            var result = client.QueryDSL.Search(index, new string[] { "type" }, constanQuery, 0, 5);
            
            Assert.AreEqual(50, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);
        }  
        
        
        [Test]
        public void TestConstantSocreWithStringQuery()
        {
            var query = new QueryString("gender:true");

            var constanQuery = new ConstantScoreQuery(query);
            
            var result = client.QueryDSL.Search(index, new string[] { "type" }, constanQuery, 0, 5);
            
            Assert.AreEqual(50, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);
        }  
        
        [Test]
        public void TestConstantSocreWithWildQuery()
        {
            var query  = new WildcardQuery("name","张*");

            var constanQuery = new ConstantScoreQuery(query);
            
            var result = client.QueryDSL.Search(index, new string[] { "type" }, constanQuery, 0, 5);
            
            Assert.AreEqual(3, result.GetTotalCount());
            Assert.AreEqual(3, result.GetHits().Hits.Count);
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

	    [Test]
	    public void TestBoolQuery()
	    {
	        var query = new BoolQuery();
	        query.Must(new TermQuery("type", "common"));
	        query.SetBoost(5);
	        var result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Fields["name"]);
	        }
	        Assert.AreEqual(3, result.GetTotalCount());
	        Assert.AreEqual(3, result.GetHits().Hits.Count);

	        query.Must(new WildcardQuery("name", "张三*"));
//			query.SetMinimumNumberShouldMatch(1);
	        result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Fields["name"]);
	        }
	        Assert.AreEqual(2, result.GetTotalCount());
	        Assert.AreEqual(2, result.GetHits().Hits.Count);


	        query.MustNot(new TermQuery("age", 24));
	        result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Fields["name"]);
	        }
	        Assert.AreEqual(1, result.GetTotalCount());
	        Assert.AreEqual(1, result.GetHits().Hits.Count);
	        Assert.AreEqual("addition_key4", result.GetHitIds()[0]);

		
	    }


        [Test]
        public void TestBoolQueryWithTwoCondition()
        {
            var query = new BoolQuery();
            query.Must(new TermQuery("type", "common"));
            query.Must(new TermQuery("age", "23"));
            query.SetBoost(5);
            var result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
            Assert.AreEqual(1, result.GetTotalCount());
            Assert.AreEqual(1, result.GetHits().Hits.Count);


            query = new BoolQuery();
            query.Must(new TermQuery("type", "common"));
            query.MustNot(new TermQuery("age", "23"));
            query.SetBoost(5);
            result = client.QueryDSL.Search(index, new string[] { "type" }, query, 0, 5);
            Assert.AreEqual(2, result.GetTotalCount());
            Assert.AreEqual(2, result.GetHits().Hits.Count);
        }

	    [Test]
	    public void TestBoostingQuery()
	    {
	        var query = new BoostingQuery();
	        query.SetPositive("name","张");
	        query.SetNegative("name","张三");
	        query.SetNegativeBoost(0.2);

	        var query2 = new BoolQuery();
	        query2.Must(query);
	        query2.Must(new TermsQuery("name", "张三"));

	        var result = client.QueryDSL.Search(index, new string[] { "type" }, query2, 0, 5);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Fields["name"]);
	        }
	    }
        

        [Test]
        public void TestSettingReturnFileds()
        {
            var q = new TermQuery("gender","true");
            
            client.QueryDSL.Search(index, new string[] {"type"}, q, 0, 5,new string[]{"_id"});
        }
#endregion


	    #region TestFilter


        [Test]
        public void TestConstantSocreWithTermFilter()
        {

            var termFilter = new TermFilter("gender", "true");
            var constanFilter = new ConstantScoreQuery(termFilter);
            var result2 = client.QueryDSL.Search(index, new string[] { "type" }, constanFilter, 0, 5);
            Assert.AreEqual(50, result2.GetTotalCount());
            Assert.AreEqual(5, result2.GetHits().Hits.Count);
        }
        
        [Test]
        public void TestConstantQueryWithQueryFilter()
        {
            var termQuery = new TermQuery("gender", "true");
            var queryFilter = new QueryFilter(termQuery);
            var constanFilter = new ConstantScoreQuery(queryFilter);
            var result2 = client.QueryDSL.Search(index, new string[] { "type" }, constanFilter, 0, 5);
            Assert.AreEqual(50, result2.GetTotalCount());
            Assert.AreEqual(5, result2.GetHits().Hits.Count);
        }

        [Test]
        public void CompareQueryWithFilter()
        {
            var query = new TermQuery("gender", "true");

            var constanQuery = new ConstantScoreQuery(query);

            var result = client.QueryDSL.Search(index, new string[] { "type" }, constanQuery, 0, 5);
            Assert.AreEqual(50, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);

            var termQuery = new TermQuery("gender", "true");
            var result1 = client.QueryDSL.Search(index, new string[] { "type" }, termQuery, 0, 5);
            Assert.AreEqual(50, result1.GetTotalCount());
            Assert.AreEqual(5, result1.GetHits().Hits.Count);

            var termFilter = new TermFilter("gender", "true");
            var constanFilter = new ConstantScoreQuery(termFilter);
            var result2 = client.QueryDSL.Search(index, new string[] { "type" }, constanFilter, 0, 5);
            Assert.AreEqual(50, result2.GetTotalCount());
            Assert.AreEqual(5, result2.GetHits().Hits.Count);


            //perf test
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                client.QueryDSL.Search(index, new string[] { "type" }, termQuery, 0, 5);
            }
            stopwatch.Stop();
            var time1 = stopwatch.ElapsedMilliseconds;


            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                client.QueryDSL.Search(index, new string[] { "type" }, constanQuery, 0, 5);
            }
            stopwatch.Stop();
            var time2 = stopwatch.ElapsedMilliseconds;

            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                client.QueryDSL.Search(index, new string[] { "type" }, constanFilter, 0, 5);
            }
            stopwatch.Stop();
            var time3 = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("TermQuery               Time1:" + time1);
            Console.WriteLine("ConstantQueryWithQuery  Time2:" + time2);
            Console.WriteLine("ConstantQueryWithFilter Time3:" + time3);

        }

        [Test]
        public void TestFilterdQuery()
        {

            var termQuery = new TermQuery("type", "common");
            var termFilter = new TermFilter("age", "24");
            var filteredQuery = new FilteredQuery(termQuery,termFilter);
            var result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);

            var wildQuery = new WildcardQuery("name", "张三*");
            termFilter = new TermFilter("age", "23");
            filteredQuery = new FilteredQuery(wildQuery, termFilter);
            result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三丰",result2.GetHits().Hits[0].Fields["name"]);


            var boolQuery = new BoolQuery();
            boolQuery.Must(new TermQuery("type", "common"));
            boolQuery.Must(new WildcardQuery("name", "张三*"));
            boolQuery.Should(new TermQuery("age", 24));

            filteredQuery=new FilteredQuery(boolQuery,new TermFilter("age","23"));

            result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三丰", result2.GetHits().Hits[0].Fields["name"]);



            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("age", "24"));

            result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三", result2.GetHits().Hits[0].Fields["name"]);


            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(2, result2.GetTotalCount());
            Assert.AreEqual(2, result2.GetHits().Hits.Count);

            
            //test should

            boolQuery = new BoolQuery();
            boolQuery.Must(new TermQuery("type", "common"));
            boolQuery.Must(new WildcardQuery("name", "张*"));
            boolQuery.Should(new TermQuery("age", 24));

            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(3, result2.GetTotalCount());
            Assert.AreEqual(3, result2.GetHits().Hits.Count);

            boolQuery = new BoolQuery();
            boolQuery.Must(new TermQuery("type", "common"));
            boolQuery.Must(new WildcardQuery("name", "张*"));
            boolQuery.Should(new TermQuery("age", 24));
            boolQuery.Should(new TermQuery("age", 28));
            boolQuery.Should(new TermQuery("age", 22));
            //must+should ->[should] make nonsense
            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(3, result2.GetTotalCount());
            Assert.AreEqual(3, result2.GetHits().Hits.Count);


            boolQuery = new BoolQuery();
            boolQuery.Should(new TermQuery("age", 24));
            boolQuery.Should(new TermQuery("age", 28));
            boolQuery.Should(new TermQuery("age", 22));
            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三", result2.GetHits().Hits[0].Fields["name"]);


            boolQuery.Should(new TermQuery("age", 25));
            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.QueryDSL.Search(index, new string[] { "type" }, filteredQuery, 0, 5);
            Assert.AreEqual(2, result2.GetTotalCount());
            Assert.AreEqual(2, result2.GetHits().Hits.Count);
        }

        [Test]
        public void TestAndFilter()
        {
            var termFilter = new TermFilter("age", 24);
            var termFilter1 = new TermFilter("name", "张三");
            var andFilter = new AndFilter(termFilter);

            var termQuery = new TermQuery("type", "common");

            var q = new FilteredQuery(termQuery,andFilter);

            var result2 = client.QueryDSL.Search(index, new string[] { "type" }, q, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三", result2.GetHits().Hits[0].Fields["name"]);



            var constantQuery = new ConstantScoreQuery(andFilter);

            result2 = client.QueryDSL.Search(index, new string[] { "type" }, constantQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三", result2.GetHits().Hits[0].Fields["name"]);

        }


        [Test]
        public void TestBoolFilterWithTwoCondition()
        {
            var boolFilter = new BoolFilter();
            boolFilter.Must(new TermFilter("type", "common"));
            boolFilter.Must(new TermFilter("age", "23"));

            var constantScoreQuery = new ConstantScoreQuery(boolFilter);
            var result = client.QueryDSL.Search(index, new string[] { "type" }, constantScoreQuery, 0, 5);
            Assert.AreEqual(1, result.GetTotalCount());
            Assert.AreEqual(1, result.GetHits().Hits.Count);


            boolFilter = new BoolFilter();
            boolFilter.Must(new TermFilter("type", "common"));
            boolFilter.MustNot(new TermFilter("age", "23"));
            constantScoreQuery = new ConstantScoreQuery(boolFilter);
            result = client.QueryDSL.Search(index, new string[] { "type" }, constantScoreQuery, 0, 5);
            Assert.AreEqual(2, result.GetTotalCount());
            Assert.AreEqual(2, result.GetHits().Hits.Count);
        }


        [Test]
        public void TestExistsFilter()
        {
            var constantScoreQuery = new ConstantScoreQuery(new ExistsFilter("age"));
            var  result = client.QueryDSL.Search(index, new string[] { "type" }, constantScoreQuery, 0, 5);
            Assert.AreEqual(4, result.GetTotalCount());
            Assert.AreEqual(4, result.GetHits().Hits.Count);



            constantScoreQuery = new ConstantScoreQuery(new ExistsFilter("ids"));
            result = client.QueryDSL.Search(index, new string[] { "type" }, constantScoreQuery, 0, 5);
            Assert.AreEqual(100, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);
        }

        [Test]
        public void TestIdsFilter()
        {
            var constantScoreQuery = new ConstantScoreQuery(new IdsFilter("type","1","2","3"));
            var result = client.QueryDSL.Search(index, new string[] { "type" }, constantScoreQuery, 0, 5);
            Assert.AreEqual(3, result.GetTotalCount());
            Assert.AreEqual(3, result.GetHits().Hits.Count);
        }

	    #endregion


		[TestFixtureSetUp]
		public void Init()
		{
			var typesetting = new TypeSetting("type");
			typesetting.AddFieldSetting("name", new StringFieldSetting() { Index = IndexType.not_analyzed });
			typesetting.AddFieldSetting("id", new NumberFieldSetting() { });
			typesetting.AddFieldSetting("gender", new BooleanFieldSetting() { Index = IndexType.not_analyzed });

			client.Index(index, "type", "_medcl", "{}");
			client.PutMapping(index, typesetting);

			IndexItem item;
			for (int i = 0; i < 100; i++)
			{
				item = new IndexItem("type", i.ToString());
				item.Add("name", Guid.NewGuid().ToString());
				item.Add("id", i);
				item.Add("ids","ids_{0}".Fill(i));
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


			item = new IndexItem("type", "addition_key2");
			item.Add("name", "张");
			item.Add("age",25);
			item.Add("type","common");
			client.Index(index, item);

			item = new IndexItem("type", "addition_key3");
			item.Add("name", "张三");
			item.Add("age", 24);
			item.Add("type", "common");
			client.Index(index, item);

			item = new IndexItem("type", "addition_key4");
			item.Add("name", "张三丰");
			item.Add("age", 23);
			item.Add("type", "common");
			client.Index(index, item);

			item = new IndexItem("type", "addition_key5");
			item.Add("name", "二张三张");
			item.Add("age", 22);
			item.Add("type", "common2");
			client.Index(index, item);

			Thread.Sleep(1000);
		}
        
		[TestFixtureTearDown]
		public void Cleanup()
		{
			client.DeleteIndex(index);
		}
	}
}