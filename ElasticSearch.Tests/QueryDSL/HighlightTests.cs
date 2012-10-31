using System;
using System.Collections.Generic;
using System.Threading;
using ElasticSearch.Client;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Domain;
using ElasticSearch.Client.Mapping;
using ElasticSearch.Client.QueryDSL;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class HighlightTests
    {
         [Test]
        public void TestHighlight()
         {
             ElasticSearch.Client.ElasticSearchClient client=new ElasticSearchClient("localhost",9200,TransportType.Http);


             string indexName = Guid.NewGuid().ToString();


             client.CreateIndex(indexName);


             TypeSetting type=new TypeSetting("type");
             type.AddStringField("title").Analyzer = "whitespace";
             type.AddStringField("snippet").Analyzer = "whitespace";
             client.PutMapping(indexName, type);

             //index sample
             Dictionary<string, object> dict=new Dictionary<string, object>();
             dict["title"] = "quick fox jump away";
             dict["snippet"] = "quick fox jump away,where are you?";
             client.Index(indexName, "type", "1", dict);
             
             dict=new Dictionary<string, object>();
             dict["title"] = "fox river is nearby";
             dict["snippet"] = "where is fox river,where is it?";
             client.Index(indexName, "type", "2", dict);

   
             ElasticQuery query=new ElasticQuery(
                 new QueryStringQuery("fox")
                 .AddField("title",5)
                 .AddField("snippet",5),null,0,5 );

             query.AddHighlightField(new HightlightField("title"));
             query.AddHighlightField(new HightlightField("snippet"));

             client.Refresh(indexName);

             var result= client.Search(indexName, query);
             Console.Out.WriteLine(result.Query);
             Console.Out.WriteLine(result.Response);

             Console.Out.WriteLine("---");
             HitStatus hits = result.GetHits();
             if (hits != null)
                 foreach (var o in hits.Hits)
                 {
                     foreach (var pair in o.Highlight)
                     {
                         Console.Out.WriteLine(pair.Key + ":");
                         foreach (var field in pair.Value)
                         {
                             Console.Out.WriteLine(field);
                         }

                         Console.Out.WriteLine();
                     }
                 
                 }


             client.DeleteIndex(indexName);
         }
    }
}