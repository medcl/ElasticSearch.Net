using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ElasticSearch.Client;
using ElasticSearch.Client.Domain;
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
	        var query = new QueryStringQuery("gender:true");
	        var result = client.Search(index, "type" , query, 0, 5);
	        Assert.AreEqual(50, result.GetTotalCount());
	        Assert.AreEqual(5, result.GetHits().Hits.Count);
	    }

        [Test]
        public void TestConstantSocreWithTermQuery()
        {
            var query = new TermQuery("gender", "true");

            var constanQuery = new ConstantScoreQuery(query);
            
            var result = client.Search(index, "type" , constanQuery, 0, 5);
            
            Assert.AreEqual(50, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);
        }  
        
        
        [Test]
        public void TestConstantSocreWithStringQuery()
        {
            var query = new QueryStringQuery("gender:true");

            var constanQuery = new ConstantScoreQuery(query);
            
            var result = client.Search(index, "type" , constanQuery, 0, 5);
            
            Assert.AreEqual(50, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);
        }  
        
        [Test]
        public void TestConstantSocreWithWildQuery()
        {
            var query  = new WildcardQuery("name","张*");

            var constanQuery = new ConstantScoreQuery(query);
            
            var result = client.Search(index, "type" , constanQuery, 0, 5);
            
            Assert.AreEqual(3, result.GetTotalCount());
            Assert.AreEqual(3, result.GetHits().Hits.Count);
        }    
        
  
	    [Test]
	    public void TestTermQuery()
	    {
	        var query = new TermQuery("gender","true");
	        var result = client.Search(index, "type" , query, 0, 5);
	        Assert.AreEqual(50, result.GetTotalCount());
	        Assert.AreEqual(5, result.GetHits().Hits.Count);
	    }
        
	    [Test]
	    public void TestTermQueryWithBoost()
	    {
	        var query = new TermQuery("gender", "true",2);
	        var result = client.Search(index, "type" , query, 0, 5);
	        Assert.AreEqual(50, result.GetTotalCount());
	        Assert.AreEqual(5, result.GetHits().Hits.Count);
	    }
        
	    [Test]
	    public void TestTermsQuery()
	    {
	        var query = new TermsQuery("gender", "true","false");
	        var result = client.Search(index, "type" , query, 0, 5);
	        Assert.AreEqual(100, result.GetTotalCount());
	        Assert.AreEqual(5, result.GetHits().Hits.Count);
	    }
	    [Test]
	    public void TestTermsQueryWithMinuMatch()
	    {
	        var query = new TermsQuery("gender",2, "true", "false");
	        var result = client.Search(index, "type" , query, 0, 5);
	        Assert.AreEqual(0, result.GetTotalCount());
	        Assert.AreEqual(0, result.GetHits().Hits.Count);
			
			
	        var item = new IndexItem("type","addition_key1");
	        item.Add("name", Guid.NewGuid().ToString());
	        item.Add("id", 2012);
	        item.Add("gender", true);
	        item.Add("gender", false);
	        client.Index(index, item);

	        Thread.Sleep(1000);
	        result = client.Search(index, "type" , query, 0, 5);
	        Assert.AreEqual(1, result.GetTotalCount());
	        Assert.AreEqual(1, result.GetHits().Hits.Count);

	    }
        
	    [Test]
	    public void TestWildcardQuery()
	    {
	        var query = new WildcardQuery("name","张*");
	        var result = client.Search(index, "type" , query, 0, 5);
	        Assert.AreEqual(3, result.GetTotalCount());
	        Assert.AreEqual(3, result.GetHits().Hits.Count);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Source["name"]);
	        }

	        Console.WriteLine("--");
	        query = new WildcardQuery("name", "张三*");
	        result = client.Search(index, "type" , query, 0, 5);
	        Assert.AreEqual(2, result.GetTotalCount());
	        Assert.AreEqual(2, result.GetHits().Hits.Count);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Source["name"]);
	        }
	    }

	    [Test]
	    public void TestBoolQuery()
	    {
	        var query = new BoolQuery();
	        query.Must(new TermQuery("type", "common"));
	        query.SetBoost(5);
	        var result = client.Search(index, "type" , query, 0, 5);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Source["name"]);
	        }
	        Assert.AreEqual(3, result.GetTotalCount());
	        Assert.AreEqual(3, result.GetHits().Hits.Count);

	        query.Must(new WildcardQuery("name", "张三*"));
//			query.SetMinimumNumberShouldMatch(1);
	        result = client.Search(index, "type" , query, 0, 5);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Source["name"]);
	        }
	        Assert.AreEqual(2, result.GetTotalCount());
	        Assert.AreEqual(2, result.GetHits().Hits.Count);


	        query.MustNot(new TermQuery("age", 24));
	        result = client.Search(index, "type" , query, 0, 5);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Source["name"]);
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
            var result = client.Search(index, "type" , query, 0, 5);
            Assert.AreEqual(1, result.GetTotalCount());
            Assert.AreEqual(1, result.GetHits().Hits.Count);


            query = new BoolQuery();
            query.Must(new TermQuery("type", "common"));
            query.MustNot(new TermQuery("age", "23"));
            query.SetBoost(5);
            result = client.Search(index, "type" , query, 0, 5);
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

	        var result = client.Search(index, "type" , query2, 0, 5);
	        foreach (var VARIABLE in result.GetHits().Hits)
	        {
	            Console.WriteLine(VARIABLE.Source["name"]);
	        }
	    }
        

        [Test]
        public void TestSettingReturnFileds()
        {
            var q = new TermQuery("gender","true");
            
            client.Search(index, "type" , q,null, 0, 5,new string[]{"_id"});
        }

#endregion

		#region TestQuery_dsl

		[Test]
		public void TestTextQuery()
		{
			string textType = "text";
			var typeSetting = new TypeSetting(textType);
			typeSetting.AddStringField("message").Analyzer="whitespace";
			client.PutMapping(index, typeSetting);
			client.Refresh();

			var dict = new Dictionary<string, object>();
			dict["message"] = "the quick brown fox jumped over thelazy dog";
			var op= client.Index(index, textType, "text_k1", dict);
			Assert.True(op.Success);
			client.Refresh();
			var textQuery = new TextQuery("message", "fox");
			var result= client.Search(index, textType, textQuery);

			Assert.AreEqual(1,result.GetTotalCount());
		}

		[Test]
		public void TestCustomScoreQuery()
		{
			//age 23 24 25
			var query = new TermQuery("type", "common");
			var dict = new Dictionary<string, object>();
			dict["param1"] = 0.2;
			var script = "_score + doc['age'].value - param1";
			var q = new CustomScoreQuery(query, script, dict);
			var result = client.Search(index, "type", q, 0, 5);
			foreach (var o in result.GetHits().Hits)
			{
				Console.WriteLine(o.ToString());
			}
			Assert.AreEqual("张",result.GetHits().Hits[0].Source["name"]);
		}

		[Test,Ignore]
		public void TestDisMaxQuery()
		{
			string textType = "dismax";
			var typeSetting = new TypeSetting(textType);
			// hed is the most important field, dek is secondary
			typeSetting.AddStringField("hed").Analyzer = "standard";
			typeSetting.AddStringField("dek").Analyzer = "standard";
			client.PutMapping(index, typeSetting);
			client.Refresh();

			// d1 is an "ok" match for:  albino elephant
			var 
			dict = new Dictionary<string, object>();
			dict["id"] = "d1";
			dict["hed"] = "elephant";
			dict["dek"] = "elephant";
			var op = client.Index(index, textType, "d1", dict);
			Assert.True(op.Success);

			// d2 is a "good" match for:  albino elephant
			IndexItem 
			item = new IndexItem(textType, "d2");
			item.Add("id", "d2");
			item.Add("hed", "elephant");
			item.Add("dek", "albino");
			item.Add("dek", "elephant");
			op = client.Index(index,  item);
			Assert.True(op.Success);

			//d3 is a "better" match for:  albino elephant
			item = new IndexItem(textType, "d3");
			item.Add("id", "d3");
			item.Add("hed", "albino");
			item.Add("hed", "elephant");
			op = client.Index(index, item);
			Assert.True(op.Success);

			// d4 is the "best" match for:  albino elephant
			item = new IndexItem(textType, "d4");
			item.Add("id", "d4");
			item.Add("hed", "albino");
			item.Add("hed", "elephant");
			item.Add("dek", "albino");
			op = client.Index(index, item);
			Assert.True(op.Success);


			client.Refresh();


			var dismaxQuery = new DisjunctionMaxQuery(0.0f);
			dismaxQuery.AddQuery(new TermQuery("hed", "albino"));
			dismaxQuery.AddQuery(new TermQuery("hed", "elephant"));
			
			var result = client.Search(index, textType, dismaxQuery);
			Console.WriteLine("all docs should match");
			Assert.AreEqual(4,result.GetTotalCount());
			foreach (var o in result.GetHits().Hits)
			{
				Console.WriteLine(o.ToString());
			}

			dismaxQuery = new DisjunctionMaxQuery(0.0f);
			dismaxQuery.AddQuery(new TermQuery("dek", "albino"));
			dismaxQuery.AddQuery(new TermQuery("dek", "elephant"));

			result = client.Search(index, textType, dismaxQuery);
			Console.WriteLine("3 docs should match");
			Assert.AreEqual(3, result.GetTotalCount());
			foreach (var o in result.GetHits().Hits)
			{
				Console.WriteLine(o.ToString());
			}
			
			dismaxQuery = new DisjunctionMaxQuery(0.0f);
			dismaxQuery.AddQuery(new TermQuery("dek", "albino"));
			dismaxQuery.AddQuery(new TermQuery("dek", "elephant"));
			dismaxQuery.AddQuery(new TermQuery("hed", "albino"));
			dismaxQuery.AddQuery(new TermQuery("hed", "elephant"));

			result = client.Search(index, textType, dismaxQuery);
			Console.WriteLine("all docs should match");
			Assert.AreEqual(4, result.GetTotalCount());
			foreach (var o in result.GetHits().Hits)
			{
				Console.WriteLine(o.ToString());
			}


			dismaxQuery = new DisjunctionMaxQuery(0.01f);
			dismaxQuery.AddQuery(new TermQuery("dek", "albino"));
			dismaxQuery.AddQuery(new TermQuery("dek", "elephant"));

			result = client.Search(index, textType, dismaxQuery);
			Console.WriteLine("3 docs should match");
			float score0 = Convert.ToSingle(result.GetHits().Hits[0].Score);
			float score1 = Convert.ToSingle(result.GetHits().Hits[1].Score);
			float score2 = Convert.ToSingle(result.GetHits().Hits[2].Score);

			foreach (var o in result.GetHits().Hits)
			{
				Console.WriteLine(o.ToString());
			}

			Assert.IsTrue(score0 > score1);
			Assert.AreEqual(score1, score2);
			Assert.AreEqual(3, result.GetTotalCount());
			Assert.AreEqual("d2", result.GetHits().Hits[0].Source["id"]);


		}

		[Test]
		public void TestFieldQuery()
		{
			var fieldQuery = new FieldQuery("age", "+23");
			var result= client.Search(index, "type", fieldQuery);
			Assert.AreEqual(1,result.GetTotalCount());
		}

		[Test]
		public void TestFuzzQuery()
		{

			string texType = "doc_fuzzy";

			var typeSetting = new TypeSetting(texType);
			typeSetting.AddStringField("f").Analyzer = "standard";
		var op=	client.PutMapping(index, typeSetting);
			Assert.True(op.Success);
			client.Refresh();

			var doc = new IndexItem(texType,"1");
			doc.Add("f","aaaaa");
			op=client.Index(index, doc);
			Assert.True(op.Success);
			doc = new IndexItem(texType,"2");
			doc.Add("f", "aaaab");
			op = client.Index(index, doc);
			Assert.True(op.Success);
			doc = new IndexItem(texType, "3");
			doc.Add("f", "aaabb");
			op = client.Index(index, doc);
			Assert.True(op.Success);
			doc = new IndexItem(texType, "4");
			doc.Add("f", "aabbb");
			op = client.Index(index, doc);
			Assert.True(op.Success);
			doc = new IndexItem(texType, "5");
			doc.Add("f", "abbbb");
			op = client.Index(index, doc);
			Assert.True(op.Success);
			doc = new IndexItem(texType, "6");
			doc.Add("f", "bbbbb");
			op = client.Index(index, doc);
			Assert.True(op.Success);
			doc = new IndexItem(texType, "7");
			doc.Add("f", "ddddd");
			op = client.Index(index, doc);
			Assert.True(op.Success);

			client.Refresh();

			var fuzzyQ = new FuzzyQuery("f", "aaaaa");
			var result=client.Search(index, texType, fuzzyQ);
			Assert.AreEqual(3,result.GetTotalCount());
			fuzzyQ = new FuzzyQuery("f", "aaaaa");
			fuzzyQ.PrefixLength = 1;
			result = client.Search(index, texType, fuzzyQ);
			Assert.AreEqual(3, result.GetTotalCount());
			fuzzyQ = new FuzzyQuery("f", "aaaaa");
			fuzzyQ.PrefixLength = 2;
			result = client.Search(index, texType, fuzzyQ);
			Assert.AreEqual(3, result.GetTotalCount());
			fuzzyQ = new FuzzyQuery("f", "aaaaa");
			fuzzyQ.PrefixLength = 3;
			result = client.Search(index, texType, fuzzyQ);
			Assert.AreEqual(3, result.GetTotalCount());
			fuzzyQ = new FuzzyQuery("f", "aaaaa");
			fuzzyQ.PrefixLength = 4;
			result = client.Search(index, texType, fuzzyQ);
			Assert.AreEqual(2, result.GetTotalCount());
			fuzzyQ = new FuzzyQuery("f", "aaaaa");
			fuzzyQ.PrefixLength = 5;
			result = client.Search(index, texType, fuzzyQ);
			Assert.AreEqual(1, result.GetTotalCount());
			fuzzyQ = new FuzzyQuery("f", "aaaaa");
			fuzzyQ.PrefixLength = 6;
			result = client.Search(index, texType, fuzzyQ);
			Assert.AreEqual(1, result.GetTotalCount());


		}


		[Test]
		public void TestHasChildQuery()
		{
			var index = "index_test_parent_child_type123_with_has_child_query_query";

			#region preparing mapping

//		    client.DeleteIndex(index);

			var parentType = new TypeSetting("blog");
			parentType.AddStringField("title");
			client.CreateIndex(index);
			var op = client.PutMapping(index, parentType);

			Assert.AreEqual(true, op.Acknowledged);

			var childType = new TypeSetting("comment", parentType);
			childType.AddStringField("comments");
		    childType.AddStringField("author").Analyzer = "keyword";

			op = client.PutMapping(index, childType);
			Assert.AreEqual(true, op.Acknowledged);

			var mapping = client.GetMapping(index, "comment");

			Assert.True(mapping.IndexOf("_parent") > 0);

			client.Refresh();

			#endregion


			#region preparing test data

			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict["title"] = "this is the blog title";
			client.Index(index, "blog", "1", dict);

			dict = new Dictionary<string, object>();
			dict["title"] = "hello.elasticsearch";
			client.Index(index, "blog", "2", dict);

			//child docs
			dict = new Dictionary<string, object>();
			dict["title"] = "awful,that's bullshit";
			dict["author"] = "lol";
			client.Index(index, "comment", "c1", dict, "1");

			dict = new Dictionary<string, object>();
			dict["title"] = "hey,lol,i can't agree more!";
			dict["author"] = "laday-guagua";
			client.Index(index, "comment", "c2", dict, "1");

			dict = new Dictionary<string, object>();
			dict["title"] = "it rocks";
			dict["author"] = "laday-guagua";
			client.Index(index, "comment", "c3", dict, "2");
			#endregion

			client.Refresh();

			var q = new ConstantScoreQuery(new HasChildQuery("comment", new TermQuery("author", "lol")));
			var result = client.Search(index, "blog" , q, 0, 5);

			Assert.AreEqual(1, result.GetTotalCount());
			Assert.AreEqual("1", result.GetHitIds()[0]);


			q = new ConstantScoreQuery(new HasChildQuery("comment", new TermQuery("author", "laday-guagua")));
			result = client.Search(index, "blog" , q, 0, 5);

			Assert.AreEqual(2, result.GetTotalCount());

			client.DeleteIndex(index);
		}



		[Test]
		public void TestTopChildrenQuery()
		{
			var index = "index_test_parent_child_type123_with_top_child_query_query";
//		    client.DeleteIndex(index);

			#region preparing mapping

			var parentType = new TypeSetting("blog");
			parentType.AddStringField("title");
			client.CreateIndex(index);
			var op = client.PutMapping(index, parentType);

			Assert.AreEqual(true, op.Acknowledged);

			var childType = new TypeSetting("comment", parentType);
			childType.AddStringField("comments");
		    childType.AddStringField("author").Analyzer = "keyword";

			op = client.PutMapping(index, childType);
			Assert.AreEqual(true, op.Acknowledged);

			var mapping = client.GetMapping(index, "comment");

			Assert.True(mapping.IndexOf("_parent") > 0);

			client.Refresh();

			#endregion


			#region preparing test data

			Dictionary<string, object> dict = new Dictionary<string, object>();
			dict["title"] = "this is the blog title";
			client.Index(index, "blog", "1", dict);

			dict = new Dictionary<string, object>();
			dict["title"] = "hello.elasticsearch";
			client.Index(index, "blog", "2", dict);

			//child docs
			dict = new Dictionary<string, object>();
			dict["title"] = "awful,that's bullshit";
			dict["author"] = "lol";
			client.Index(index, "comment", "c1", dict, "1");

			dict = new Dictionary<string, object>();
			dict["title"] = "hey,lol,i can't agree more!";
			dict["author"] = "laday-guagua";
			client.Index(index, "comment", "c2", dict, "1");

			dict = new Dictionary<string, object>();
			dict["title"] = "it rocks";
			dict["author"] = "laday-guagua";
			client.Index(index, "comment", "c3", dict, "2");
			#endregion

			client.Refresh();

			var q = new ConstantScoreQuery(new TopChildrenQuery("comment", new TermQuery("author", "lol")));
			var result = client.Search(index, "blog" , q, 0, 5);

			Assert.AreEqual(1, result.GetTotalCount());
			Assert.AreEqual("1", result.GetHitIds()[0]);


			q = new ConstantScoreQuery(new TopChildrenQuery("comment", new TermQuery("author", "laday-guagua")));
			result = client.Search(index, "blog" , q, 0, 5);

			Assert.AreEqual(2, result.GetTotalCount());

			client.DeleteIndex(index);
		}


		[Test]
		public void TestMatchAllQuery()
		{
			var filter = new TermFilter("gender", "true");//new TermsQuery("gender", "true", "false");
			var filterQuery = new FilteredQuery( new MatchAllQuery(),filter);
			var result = client.Search(index, "type" , filterQuery, 0, 5);
			Assert.AreEqual(50, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);

			var filter1 = new TermFilter("gender", "true");
			filterQuery = new FilteredQuery(new MatchAllQuery(), filter1);
			result = client.Search(index, "type" , filterQuery, 0, 5);
			Assert.AreEqual(50, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);
		}

		[Test]
		public void TestFuzzLikeTextQuery()
		{
			string textType = "text_flt";
			var typeSetting = new TypeSetting(textType);
			typeSetting.AddStringField("message").Analyzer="standard";
			client.PutMapping(index, typeSetting);
			client.Refresh();

			var dict = new Dictionary<string, object>();
			dict["message"] = "the quick brown fox jumped over thelazy dog";
			var op= client.Index(index, textType, "text_k1", dict);
			Assert.True(op.Success);
			client.Refresh();

			var flt = new FuzzyLikeThisQuery("message", "lazy dob");

			var result= client.Search(index, textType, flt);

			Assert.AreEqual(1,result.GetTotalCount());

			flt = new FuzzyLikeThisQuery("message", "lazy cat");

			result = client.Search(index, textType, flt);

			Assert.AreEqual(0, result.GetTotalCount());
		}

		[Test]
		public void TestIdsQuery()
		{
			var constantScoreQuery = new ConstantScoreQuery(new IdsQuery("type", "1", "2", "3"));
			var result = client.Search(index, "type" , constantScoreQuery, 0, 5);
			Assert.AreEqual(3, result.GetTotalCount());
			Assert.AreEqual(3, result.GetHits().Hits.Count);

			constantScoreQuery = new ConstantScoreQuery(new IdsQuery("type", "1", "2", "3", "1121"));
			result = client.Search(index, "type" , constantScoreQuery, 0, 5);
			Assert.AreEqual(3, result.GetTotalCount());
			Assert.AreEqual(3, result.GetHits().Hits.Count);


			var item = new IndexItem("type1", "uk111");
			item.Add("iid", 1);
			client.Index(index, item);
			item = new IndexItem("type1", "dk222");
			item.Add("iid", 2);
			client.Index(index, item);

			constantScoreQuery = new ConstantScoreQuery(new IdsQuery("type", "1", "2", "3", "1121", "uk111"));
			result = client.Search(index, "type" , constantScoreQuery, 0, 5);
			Assert.AreEqual(3, result.GetTotalCount());
			Assert.AreEqual(3, result.GetHits().Hits.Count);


			//ids can't query corss type
			constantScoreQuery = new ConstantScoreQuery(new IdsQuery(new string[] { "type", "type1" }, "1", "2", "3", "1121", "uk111"));
			result = client.Search(index, "type" , constantScoreQuery, 0, 5);
			Assert.AreEqual(3, result.GetTotalCount());
			Assert.AreEqual(3, result.GetHits().Hits.Count);

			//waiting for refresh
			Thread.Sleep(1000);

			constantScoreQuery = new ConstantScoreQuery(new IdsQuery(new string[] { "type", "type1" }, "1", "2", "3", "1121", "uk111"));
			result = client.Search(index, null, constantScoreQuery, 0, 5);
			Assert.AreEqual(4, result.GetTotalCount());
			Assert.AreEqual(4, result.GetHits().Hits.Count);


			constantScoreQuery = new ConstantScoreQuery(new IdsQuery(new string[] { "type", "type1" }, "1", "2", "3", "1121", "uk111", "dk222"));
			result = client.Search(index, null, constantScoreQuery, 0, 5);
			Assert.AreEqual(5, result.GetTotalCount());
			Assert.AreEqual(5, result.GetHits().Hits.Count);
		}
		
		[Test,Ignore]
		public void TestMoreLikethisQuery()
		{

			//TODO
			var mlt = new MoreLikeThisQuery(new List<string>() {"age","name"}, "张");
			var result=client.Search(index, "type", mlt);

			
		}
		
		[Test]
		public void TestPrefixQuery()
		{
			var prefixQuery = new PrefixQuery("name", "张");
			var termFilter = new TermFilter("type", "common");
			var q = new FilteredQuery(prefixQuery,termFilter);
			var result = client.Search(index, "type" , q, 0, 5);

			Assert.AreEqual(3, result.GetTotalCount());

			prefixQuery = new PrefixQuery("name", "张三");
			termFilter = new TermFilter("type", "common");
			q = new FilteredQuery(prefixQuery, termFilter);
			result = client.Search(index, "type" , q, 0, 5);

			Assert.AreEqual(2, result.GetTotalCount());
		}

		[Test]
		public void TestRangeQuery()
		{
			var rangefilter = new RangeQuery("age", "22", "25", true, true);
			ConstantScoreQuery query = new ConstantScoreQuery(rangefilter);

			var result = client.Search(index, "type" , query, 0, 5);

			Assert.AreEqual(4, result.GetTotalCount());


			rangefilter = new RangeQuery("age", "22", "25", false, true);
			query = new ConstantScoreQuery(rangefilter);

			result = client.Search(index, "type" , query, 0, 5);

			Assert.AreEqual(3, result.GetTotalCount());

			rangefilter = new RangeQuery("age", "22", "25", false, false);
			query = new ConstantScoreQuery(rangefilter);

			result = client.Search(index, "type" , query, 0, 5);

			Assert.AreEqual(2, result.GetTotalCount());
		}

		
		[Test,Ignore]
		public void TestSpanTermQuery()
		{
			//TODO

		}

		[Test,Ignore]
		public void TestSpanOrQuery()
		{
			//TODO

		}

		[Test, Ignore]
		public void TestSpanNotQuery()
		{
			//TODO

		}		
		
		[Test, Ignore]
		public void TestSpanNearQuery()
		{
			//TODO

		}	
		
		[Test, Ignore]
		public void TestNestedQuery()
		{
			//TODO

		}	
		
		[Test, Ignore]
		public void TestCustomFiltersScoreQuery()
		{
			//TODO

		}
		
		#endregion


	    #region TestFilter



		[Test]
		public void TestRangeFilter()
		{
			var rangefilter = new RangeFilter("age", "22", "25", true, true);
			ConstantScoreQuery query = new ConstantScoreQuery(rangefilter);

			var result = client.Search(index, "type" , query, 0, 5);

			Assert.AreEqual(4, result.GetTotalCount());


			rangefilter = new RangeFilter("age", "22", "25", false, true);
			query = new ConstantScoreQuery(rangefilter);

			result = client.Search(index, "type" , query, 0, 5);

			Assert.AreEqual(3, result.GetTotalCount());

			rangefilter = new RangeFilter("age", "22", "25", false, false);
			query = new ConstantScoreQuery(rangefilter);

			result = client.Search(index, "type" , query, 0, 5);

			Assert.AreEqual(2, result.GetTotalCount());
		}


        [Test]
        public void TestConstantSocreWithTermFilter()
        {

            var termFilter = new TermFilter("gender", "true");
            var constanFilter = new ConstantScoreQuery(termFilter);
            var result2 = client.Search(index, "type" , constanFilter, 0, 5);
            Assert.AreEqual(50, result2.GetTotalCount());
            Assert.AreEqual(5, result2.GetHits().Hits.Count);
        }


        [Test]
        public void TestConstantScoreWithRangeFilter()
        {
            var termFilter = new RangeFilter("age", "22","25",true,true);
            var constanFilter = new ConstantScoreQuery(termFilter);
            var result2 = client.Search(index, "type", constanFilter, 0, 5);
            Assert.AreEqual(4, result2.GetTotalCount());
            Assert.AreEqual(4, result2.GetHits().Hits.Count);
        }

        [Test]
        public void TestConstantScoreNestedAndFilter()
        {

            IFilter termFilter = new TermFilter("gender", "true");

            var andFilter = new AndFilter(termFilter);
            
            var constanFilter = new ConstantScoreQuery(andFilter);
            
            var result2 = client.Search(index, "type", constanFilter, 0, 5);
            Assert.AreEqual(50, result2.GetTotalCount());
            Assert.AreEqual(5, result2.GetHits().Hits.Count);


            //test and filter and range filter

            termFilter = new RangeFilter("age", "22", "25", true, true);

            andFilter = new AndFilter(termFilter);

            constanFilter = new ConstantScoreQuery(andFilter);
            result2 = client.Search(index, "type", constanFilter, 0, 5);
            Assert.AreEqual(4, result2.GetTotalCount());
            Assert.AreEqual(4, result2.GetHits().Hits.Count);


            //test bool filter and range filter
            termFilter = new RangeFilter("age", "22", "25", true, true);

            var boolfilter = new BoolFilter();
            boolfilter.Must(termFilter);

            constanFilter = new ConstantScoreQuery(boolfilter);
            result2 = client.Search(index, "type", constanFilter, 0, 5);
            Assert.AreEqual(4, result2.GetTotalCount());
            Assert.AreEqual(4, result2.GetHits().Hits.Count);


        }



        
        [Test]
        public void TestConstantQueryWithQueryFilter()
        {
            var termQuery = new TermQuery("gender", "true");
            var queryFilter = new QueryFilter(termQuery);
            var constanFilter = new ConstantScoreQuery(queryFilter);
            var result2 = client.Search(index, "type" , constanFilter, 0, 5);
            Assert.AreEqual(50, result2.GetTotalCount());
            Assert.AreEqual(5, result2.GetHits().Hits.Count);
        }

        [Test]
        public void CompareQueryWithFilter()
        {
            var query = new TermQuery("gender", "true");

            var constanQuery = new ConstantScoreQuery(query);

            var result = client.Search(index, "type" , constanQuery, 0, 5);
            Assert.AreEqual(50, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);

            var termQuery = new TermQuery("gender", "true");
            var result1 = client.Search(index, "type" , termQuery, 0, 5);
            Assert.AreEqual(50, result1.GetTotalCount());
            Assert.AreEqual(5, result1.GetHits().Hits.Count);

            var termFilter = new TermFilter("gender", "true");
            var constanFilter = new ConstantScoreQuery(termFilter);
            var result2 = client.Search(index, "type" , constanFilter, 0, 5);
            Assert.AreEqual(50, result2.GetTotalCount());
            Assert.AreEqual(5, result2.GetHits().Hits.Count);


            //perf test
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                client.Search(index, "type" , termQuery, 0, 5);
            }
            stopwatch.Stop();
            var time1 = stopwatch.ElapsedMilliseconds;


            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                client.Search(index, "type" , constanQuery, 0, 5);
            }
            stopwatch.Stop();
            var time2 = stopwatch.ElapsedMilliseconds;

            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                client.Search(index, "type" , constanFilter, 0, 5);
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
            var result2 = client.Search(index, "type" , filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);

            var wildQuery = new WildcardQuery("name", "张三*");
            termFilter = new TermFilter("age", "23");
            filteredQuery = new FilteredQuery(wildQuery, termFilter);
            result2 = client.Search(index, "type" , filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三丰",result2.GetHits().Hits[0].Source["name"]);


            var boolQuery = new BoolQuery();
            boolQuery.Must(new TermQuery("type", "common"));
            boolQuery.Must(new WildcardQuery("name", "张三*"));
            boolQuery.Should(new TermQuery("age", 24));

            filteredQuery=new FilteredQuery(boolQuery,new TermFilter("age","23"));

            result2 = client.Search(index, "type" , filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三丰", result2.GetHits().Hits[0].Source["name"]);



            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("age", "24"));

            result2 = client.Search(index, "type" , filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三", result2.GetHits().Hits[0].Source["name"]);


            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.Search(index, "type" , filteredQuery, 0, 5);
            Assert.AreEqual(2, result2.GetTotalCount());
            Assert.AreEqual(2, result2.GetHits().Hits.Count);

            
            //test should

            boolQuery = new BoolQuery();
            boolQuery.Must(new TermQuery("type", "common"));
            boolQuery.Must(new WildcardQuery("name", "张*"));
            boolQuery.Should(new TermQuery("age", 24));

            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.Search(index, "type" , filteredQuery, 0, 5);
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

            result2 = client.Search(index, "type" , filteredQuery, 0, 5);
            Assert.AreEqual(3, result2.GetTotalCount());
            Assert.AreEqual(3, result2.GetHits().Hits.Count);


            boolQuery = new BoolQuery();
            boolQuery.Should(new TermQuery("age", 24));
            boolQuery.Should(new TermQuery("age", 28));
            boolQuery.Should(new TermQuery("age", 22));
            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.Search(index, "type" , filteredQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三", result2.GetHits().Hits[0].Source["name"]);


            boolQuery.Should(new TermQuery("age", 25));
            filteredQuery = new FilteredQuery(boolQuery, new TermFilter("type", "common"));

            result2 = client.Search(index, "type" , filteredQuery, 0, 5);
            Assert.AreEqual(2, result2.GetTotalCount());
            Assert.AreEqual(2, result2.GetHits().Hits.Count);
        }

        [Test]
        public void TestAndFilter()
        {
            var termFilter = new TermFilter("age", 24);
//            var termFilter1 = new TermFilter("name", "张三");
            var andFilter = new AndFilter(termFilter);

            var termQuery = new TermQuery("type", "common");

            var q = new FilteredQuery(termQuery,andFilter);

            var result2 = client.Search(index, "type" , q, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三", result2.GetHits().Hits[0].Source["name"]);



            var constantQuery = new ConstantScoreQuery(andFilter);

            result2 = client.Search(index, "type" , constantQuery, 0, 5);
            Assert.AreEqual(1, result2.GetTotalCount());
            Assert.AreEqual(1, result2.GetHits().Hits.Count);
            Assert.AreEqual("张三", result2.GetHits().Hits[0].Source["name"]);

        }

        [Test]
        public void TestOrFilter()
        {
            var termFilter = new TermFilter("age", 24);
            var termFilter1 = new TermFilter("name", "张三丰");

            var orFilter = new OrFilter(termFilter,termFilter1);

            var termQuery = new TermQuery("type", "common");

            var q = new FilteredQuery(termQuery, orFilter);

            var result = client.Search(index, "type" , q, 0, 5);

            Assert.AreEqual(2, result.GetTotalCount());
        }


        [Test]
        public void TestPrefixFilter()
        {
            var prefixfilter = new PrefixFilter("name", "张");
            var termQuery = new TermQuery("type", "common");
            var q = new FilteredQuery(termQuery, prefixfilter);
            var result = client.Search(index, "type" , q, 0, 5);

            Assert.AreEqual(3, result.GetTotalCount());

            prefixfilter = new PrefixFilter("name", "张三");
            termQuery = new TermQuery("type", "common");
            q = new FilteredQuery(termQuery, prefixfilter);
            result = client.Search(index, "type" , q, 0, 5);

            Assert.AreEqual(2, result.GetTotalCount());
        }

        [Test]
        public void TestNotFilter()
        {
            var termFilter = new TermFilter("age", 24);
            var notFilter = new NotFilter(termFilter);

            var termQuery = new TermQuery("type", "common");

            var q = new FilteredQuery(termQuery, notFilter);

            var result2 = client.Search(index, "type" , q, 0, 5);
            Assert.AreEqual(2, result2.GetTotalCount());
            Assert.AreEqual(2, result2.GetHits().Hits.Count);
        }

        [Test]
        public void TestBoolFilterWithTwoCondition()
        {
            var boolFilter = new BoolFilter();
            boolFilter.Must(new TermFilter("type", "common"));
            boolFilter.Must(new TermFilter("age", "23"));

            var constantScoreQuery = new ConstantScoreQuery(boolFilter);
            var result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(1, result.GetTotalCount());
            Assert.AreEqual(1, result.GetHits().Hits.Count);


            boolFilter = new BoolFilter();
            boolFilter.Must(new TermFilter("type", "common"));
            boolFilter.MustNot(new TermFilter("age", "23"));
            constantScoreQuery = new ConstantScoreQuery(boolFilter);
            result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(2, result.GetTotalCount());
            Assert.AreEqual(2, result.GetHits().Hits.Count);
        }


        [Test]
        public void TestExistsFilter()
        {
            var constantScoreQuery = new ConstantScoreQuery(new ExistsFilter("age"));
            var  result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(4, result.GetTotalCount());
            Assert.AreEqual(4, result.GetHits().Hits.Count);



            constantScoreQuery = new ConstantScoreQuery(new ExistsFilter("ids"));
            result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(100, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);
        }

        [Test]
        public void TestMissingFiledFilter()
        {
            var constantScoreQuery = new ConstantScoreQuery(new MissingFilter("age"));
            var result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(100, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count); 



            constantScoreQuery = new ConstantScoreQuery(new MissingFilter("ids"));
            result = client.Search(index, "type" , constantScoreQuery, 0, 5);
         Assert.AreEqual(4, result.GetTotalCount());
            Assert.AreEqual(4, result.GetHits().Hits.Count);
        }

        [Test]
        public void TestIdsFilter()
        {
            var constantScoreQuery = new ConstantScoreQuery(new IdsFilter("type","1","2","3"));
            var result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(3, result.GetTotalCount());
            Assert.AreEqual(3, result.GetHits().Hits.Count);

            constantScoreQuery = new ConstantScoreQuery(new IdsFilter("type", "1", "2", "3","1121"));
            result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(3, result.GetTotalCount());
            Assert.AreEqual(3, result.GetHits().Hits.Count);


           var item = new IndexItem("type1", "uk111");
           item.Add("iid", 1);
            client.Index(index, item);
            item = new IndexItem("type1", "dk222");
            item.Add("iid", 2);
            client.Index(index, item);

            constantScoreQuery = new ConstantScoreQuery(new IdsFilter("type", "1", "2", "3", "1121", "uk111"));
            result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(3, result.GetTotalCount());
            Assert.AreEqual(3, result.GetHits().Hits.Count);


            //ids can't query corss type
            constantScoreQuery = new ConstantScoreQuery(new IdsFilter(new string[] { "type", "type1" }, "1", "2", "3", "1121", "uk111"));
            result = client.Search(index, "type" , constantScoreQuery, 0, 5);
            Assert.AreEqual(3, result.GetTotalCount());
            Assert.AreEqual(3, result.GetHits().Hits.Count);

            //waiting for refresh
            Thread.Sleep(1000);

            constantScoreQuery = new ConstantScoreQuery(new IdsFilter(new string[] { "type", "type1" }, "1", "2", "3", "1121", "uk111"));
            result = client.Search(index, null, constantScoreQuery, 0, 5);
            Assert.AreEqual(4, result.GetTotalCount());
            Assert.AreEqual(4, result.GetHits().Hits.Count);


            constantScoreQuery = new ConstantScoreQuery(new IdsFilter(new string[] { "type", "type1" }, "1", "2", "3", "1121", "uk111", "dk222"));
            result = client.Search(index, null, constantScoreQuery, 0, 5);
            Assert.AreEqual(5, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);
        }

        [Test]
        public void TestLimitFilter()
        {
            string testForShard = "test_for_shard";
            client.CreateIndex(testForShard, new IndexSetting(6, 0));
            for (int i = 0; i < 200; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["a"] = i;
                client.Index(testForShard, "type", i.ToString(), dict);
            }

            Thread.Sleep(1000);
            var constantScoreQuery = new ConstantScoreQuery(new LimitFilter(1));
            var result = client.Search(testForShard, "type" , constantScoreQuery, 0, 25);
            Assert.AreEqual(6, result.GetTotalCount());
            Assert.AreEqual(6, result.GetHits().Hits.Count);


             constantScoreQuery = new ConstantScoreQuery(new LimitFilter(2));
             result = client.Search(testForShard, "type" , constantScoreQuery, 0, 25);
            Assert.AreEqual(12, result.GetTotalCount());
            Assert.AreEqual(12, result.GetHits().Hits.Count);

            constantScoreQuery = new ConstantScoreQuery(new LimitFilter(3));
            result = client.Search(testForShard, "type" , constantScoreQuery, 0, 25);
            Assert.AreEqual(18, result.GetTotalCount());
            Assert.AreEqual(18, result.GetHits().Hits.Count);

            client.DeleteIndex(testForShard);
        }

        [Test]
        public void TestTypeFilter()
        {
            string testForShard = "test_for_shard_123";
            client.CreateIndex(testForShard, new IndexSetting(6, 0));
            for (int i = 0; i < 10; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["a"] = i;
                client.Index(testForShard, "type1", i.ToString(), dict);
            }

            for (int i = 0; i < 10; i++)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["a"] = i;
                client.Index(testForShard, "type2", i.ToString(), dict);
            }

            Thread.Sleep(1000);
            var constantScoreQuery = new ConstantScoreQuery(new TypeFilter("type2"));
            var result = client.Search(testForShard, null, constantScoreQuery, 0, 25);
            Assert.AreEqual(10, result.GetTotalCount());
            Assert.AreEqual(10, result.GetHits().Hits.Count);

            client.DeleteIndex(testForShard);
        }


        [Test]
        public void TestMatchAllFilter()
        {
            var query = new TermQuery("gender","true");//new TermsQuery("gender", "true", "false");
            var filterQuery = new FilteredQuery(query, new MatchAllFilter());
            var result = client.Search(index, "type" , filterQuery, 0, 5);
            Assert.AreEqual(50, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);

            var query1 = new TermsQuery("gender", "true");
            filterQuery = new FilteredQuery(query1, new MatchAllFilter());
            result = client.Search(index, "type" , filterQuery, 0, 5);
            Assert.AreEqual(50, result.GetTotalCount());
            Assert.AreEqual(5, result.GetHits().Hits.Count);
        }

        [Test]
        public void TestHasChildFilter()
        {
            var index = "index_test_parent_child_type123_with_has_child_query";
//            client.DeleteIndex(index);
            #region preparing mapping

            var parentType = new TypeSetting("blog");
            parentType.AddStringField("title");
        	client.CreateIndex(index);
            var op = client.PutMapping(index, parentType);

            Assert.AreEqual(true, op.Acknowledged);

            var childType = new TypeSetting("comment", parentType);
            childType.AddStringField("comments");
            childType.AddStringField("author").Analyzer = "keyword";

            op = client.PutMapping(index, childType);
            Assert.AreEqual(true, op.Acknowledged);

            var mapping = client.GetMapping(index, "comment");

            Assert.True(mapping.IndexOf("_parent") > 0);

            client.Refresh();

            #endregion


            #region preparing test data

            Dictionary<string, object> dict=new Dictionary<string, object>();
            dict["title"] = "this is the blog title";
            client.Index(index, "blog", "1", dict);

            dict = new Dictionary<string, object>();
            dict["title"] = "hello.elasticsearch";
            client.Index(index, "blog", "2", dict);

            //child docs
            dict = new Dictionary<string, object>();
            dict["title"] = "awful,that's bullshit";
            dict["author"] = "lol";
            client.Index(index, "comment", "c1", dict,"1");

            dict = new Dictionary<string, object>();
            dict["title"] = "hey,lol,i can't agree more!";
            dict["author"] = "laday-guagua";
            client.Index(index, "comment", "c2", dict, "1");

            dict = new Dictionary<string, object>();
            dict["title"] = "it rocks";
            dict["author"] = "laday-guagua";
            client.Index(index, "comment", "c3", dict, "2");
            #endregion

            client.Refresh();
            Thread.Sleep(2000);

            var q=new ConstantScoreQuery(new HasChildFilter("comment",new TermQuery("author","lol")));
            var result=client.Search(index, "blog", q,0,5);

            Assert.AreEqual(1,result.GetTotalCount());
            Assert.AreEqual("1",result.GetHitIds()[0]);


            q = new ConstantScoreQuery(new HasChildFilter("comment", new TermQuery("author", "laday-guagua")));
            result = client.Search(index, "blog" , q, 0, 5);

            Assert.AreEqual(2, result.GetTotalCount());
             
            client.DeleteIndex(index);
        }

        [Test]
        public void TestScriptFilter()
        {
            //age 23 24 25
            var query = new TermQuery("type", "common");
            var dict = new Dictionary<string, object>();
            dict["param1"] = 20;
            var filter = new ScriptFilter("doc['age'].value > param1",dict);
            var filterQ = new FilteredQuery(query, filter);
            var result= client.Search(index, "type" , filterQ, 0, 5);
            Assert.AreEqual(3,result.GetTotalCount());


            dict = new Dictionary<string, object>();
            dict["param1"] = 23;
            filter = new ScriptFilter("doc['age'].value > param1", dict);
            filterQ = new FilteredQuery(query, filter);
            result = client.Search(index, "type" , filterQ, 0, 5);
            Assert.AreEqual(2, result.GetTotalCount());

            dict = new Dictionary<string, object>();
            dict["param1"] = 24;
            filter = new ScriptFilter("doc['age'].value >= param1", dict);
            filterQ = new FilteredQuery(query, filter);
            result = client.Search(index, "type" , filterQ, 0, 5);
            Assert.AreEqual(2, result.GetTotalCount());

            dict = new Dictionary<string, object>();
            dict["param1"] = 25;
            filter = new ScriptFilter("doc['age'].value >= param1", dict);
            filterQ = new FilteredQuery(query, filter);
            result = client.Search(index, "type" , filterQ, 0, 5);
            Assert.AreEqual(1, result.GetTotalCount());

            dict = new Dictionary<string, object>();
            dict["param1"] = 25;
            filter = new ScriptFilter("doc['age'].value < param1", dict);
            filterQ = new FilteredQuery(query, filter);
            result = client.Search(index, "type" , filterQ, 0, 5);
            Assert.AreEqual(2, result.GetTotalCount());

            dict = new Dictionary<string, object>();
            dict["param1"] = 20;
            filter = new ScriptFilter("doc['age'].value < param1", dict);
            filterQ = new FilteredQuery(query, filter);
            result = client.Search(index, "type" , filterQ, 0, 5);
            Assert.AreEqual(0, result.GetTotalCount());
        }

        [Test]
        public void TestNumRangeFilter()
        {
            var rangefilter = new NumericRangeFilter("age", 22, 25, true, true);
            ConstantScoreQuery query=new ConstantScoreQuery(rangefilter);

            var result= client.Search(index, "type" , query, 0, 5);

            Assert.AreEqual(4, result.GetTotalCount());


            rangefilter = new NumericRangeFilter("age", 22, 25, false, true);
            query = new ConstantScoreQuery(rangefilter);

            result = client.Search(index, "type" , query, 0, 5);

            Assert.AreEqual(3, result.GetTotalCount());

            rangefilter = new NumericRangeFilter("age", 22, 25, false, false);
            query = new ConstantScoreQuery(rangefilter);

            result = client.Search(index, "type" , query, 0, 5);

            Assert.AreEqual(2, result.GetTotalCount());
        }


        [Test]
        public void TestTermsFilter()
        {
            var termsFilter = new TermsFilter("age", 22, 23, 24,25);
            var constantQ = new ConstantScoreQuery(termsFilter);
            var result= client.Search(index, "type" , constantQ, 0, 5);
            Assert.AreEqual(4,result.GetTotalCount());

            termsFilter = new TermsFilter("age", 22);
            constantQ = new ConstantScoreQuery(termsFilter);
            result = client.Search(index, "type" , constantQ, 0, 5);
            Assert.AreEqual(1, result.GetTotalCount());
        }

        [Test,Ignore]
        public void TestTermsFilterCache()
        {

            for (int i = 50; i < 10000; i++)
            {
                var dict = new Dictionary<string, object>();
                dict["age"] = i;
                client.Index(index, "type", i.ToString(),dict);
            }

            var termsFilter = new TermsFilter("age", 22, 23, 24, 25);
            var constantQ = new ConstantScoreQuery(termsFilter);
            var result = client.Search(index, "type" , constantQ, 0, 5);
            Assert.AreEqual(4, result.GetTotalCount());

            Stopwatch stopwatch=new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 30000; i++)
            {
                client.Search(index, "type" , constantQ, 0, 5);
            }
            stopwatch.Stop();
            var time1 = stopwatch.ElapsedMilliseconds;

            
            termsFilter = new TermsFilter("age", 22, 23, 24, 25);
            termsFilter.SetCache(true);
            constantQ = new ConstantScoreQuery(termsFilter);
            result = client.Search(index, "type" , constantQ, 0, 5);
            Assert.AreEqual(4, result.GetTotalCount());
            
            stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 30000; i++)
            {
                client.Search(index, "type" , constantQ, 0, 5);
            }
            stopwatch.Stop();
            var time2 = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("NoCache:"+time1);
            Console.WriteLine("Cache:"+time2);
        }


        [Test,Ignore]
        public void TestNestedFilter()
        {
//    {
//    "name": "jackson",
//    "resume": {
//        "age": 22,
//        "gender": "male",
//        "mail": "aaa@bb.cc"
//    }
//}
            var obj = "{     \"type\": \"vip\", \"name\": \"jackson\",    \"resume\": {        \"age\": 22,        \"gender\": \"male\",        \"mail\": \"aaa@bb.cc\"    }}";
            var op= client.Index(index, "person", "key1", obj);
            Assert.True(op.Success);

            obj = "{     \"type\": \"vip\", \"name\": \"tom\",    \"resume\": {        \"age\": 24,        \"gender\": \"female\",        \"mail\": \"tom@bb.cc\"    }}";
            op = client.Index(index, "person", "key2", obj);
            Assert.True(op.Success);
        	client.Refresh();
            var nestFilter = new NestedFilter("person", new TermQuery("resume.age", 22), true);

            var q = new FilteredQuery(new TermQuery("type","vip"), nestFilter);
            var result=client.Search(index, "person", q, 0, 5);
            Assert.AreEqual(1,result.GetTotalCount());
            Assert.AreEqual("jackson", result.GetHits().Hits[0].Source["name"]);
        
            
        }

		[Test]
		public void  TestWithSort()
		{
			var termq = new TermQuery("type", "common");
			var conq = new ConstantScoreQuery(termq);
			var result= client.Search(index, new string[] {"type"}, conq, new SortItem("age", SortType.Asc), 0, 5);
			Assert.AreEqual(3,result.GetTotalCount());

			Assert.AreEqual(23,result.GetHits().Hits[0].Source["age"]);
			Assert.AreEqual(24,result.GetHits().Hits[1].Source["age"]);
			Assert.AreEqual(25,result.GetHits().Hits[2].Source["age"]);

			result = client.Search(index, new string[] { "type" }, conq, new SortItem("age", SortType.Desc), 0, 5);
			Assert.AreEqual(3, result.GetTotalCount());

			Assert.AreEqual(25, result.GetHits().Hits[0].Source["age"]);
			Assert.AreEqual(24, result.GetHits().Hits[1].Source["age"]);
			Assert.AreEqual(23, result.GetHits().Hits[2].Source["age"]);
		}

	    #endregion


		[TestFixtureSetUp]
		public void Init()
		{
			var typesetting = new TypeSetting("type");
			typesetting.AddFieldSetting("name", new StringFieldSetting() { Index = IndexType.not_analyzed });
			typesetting.AddFieldSetting("id", new NumberFieldSetting() { });
			typesetting.AddFieldSetting("gender", new BooleanFieldSetting() { Index = IndexType.not_analyzed });

			client.CreateIndex(index);
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