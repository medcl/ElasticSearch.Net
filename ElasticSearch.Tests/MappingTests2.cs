using System;
using System.Threading;
using ElasticSearch.Client;
using ElasticSearch.Client.Domain;
using ElasticSearch.Client.Mapping;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MappingTests2
    {
        ElasticSearchClient client = new ElasticSearchClient("localhost");
        [Test]
        public void TestCreatingMapping()
        {
            var index = "index_operate" + Guid.NewGuid();
            var stringFieldSetting = new StringFieldSetting { Analyzer = "standard", Type = "string", NullValue = "mystr" };

            var typeSetting = new TypeSetting("custom_type");
            typeSetting.AddFieldSetting("medcl", stringFieldSetting);

            var typeSetting2 = new TypeSetting("hell_type1");
            var numfield = new NumberFieldSetting { Store = Store.yes, NullValue = 0.00 };
            typeSetting2.AddFieldSetting("name", numfield);

            client.CreateIndex(index);
            var result = client.PutMapping(index, typeSetting);
            Assert.AreEqual(true, result.Success);

            result = client.PutMapping(index, typeSetting2);
            Assert.AreEqual(true, result.Success);

            var result2 = client.DeleteIndex(index);
            Assert.AreEqual(true, result2.Success);
        }

        [Test]
        public void TestCreateIndex()
        {
            var index = "index_operate" + Guid.NewGuid();
            var result2 = client.CreateIndex(index, new IndexSetting(10, 1));
            Assert.AreEqual(true, result2.Success);
            result2 = client.DeleteIndex(index);
            Assert.AreEqual(true, result2.Success);
        }

        [Test]
        public void TestModifyIndex()
        {
            var index = "index_operate" + Guid.NewGuid();
            var result2 = client.CreateIndex(index, new IndexSetting(10, 1));
            Assert.AreEqual(true, result2.Success);
            Thread.Sleep(1000);
            result2 = client.ModifyIndex(index, new IndexSetting(10, 2));
            Console.WriteLine(result2.JsonString);
            Assert.AreEqual(true, result2.Success);
            result2 = client.DeleteIndex(index);
            Assert.AreEqual(true, result2.Success);
        }

        [Test]
        public void TestCreateUser()
        {
            var template_key = "test_user_template";
            client.DeleteTemplate(template_key);

            var template = new TemplateSetting(template_key);
            template.Template = "test_user*";
            template.IndexSetting = new TemplateIndexSetting(1, 1);
            var type = new TypeSetting("Staff");
            type.AddStringField("Name");
            type.AddStringField("Email");
            type.AddStringField("EnglishName");

            type.AddNumField("UserID", NumType.Integer);
            template.AddTypeSetting(type);

            var jsonstr = JsonSerializer.Get(template);
            Console.WriteLine(jsonstr);

            var result = client.CreateTemplate(template_key, template);
            Console.WriteLine(result.JsonString);
            Assert.AreEqual(true, result.Success);

            var temp2= client.GetTemplate(template_key);
            Assert.AreEqual("test_user*", temp2[template_key].Template);
            Assert.True(temp2[template_key].Mappings.ContainsKey("Staff"));

            Assert.AreEqual(1, temp2[template_key].IndexSetting.NumberOfReplicas);
            Assert.AreEqual(1, temp2[template_key].IndexSetting.NumberOfShards);

            result= client.DeleteTemplate(template_key);
            Assert.AreEqual(true,result.Success);
        }

        [Test]
        public void TestCreateAnalysisMapping()
        {
            var template = new TemplateSetting("index_1*");
            var type = new TypeSetting("type88");
            type.AddStringField("name", null, Store.no, IndexType.analyzed, TermVector.no, 1, null, true, true, "keyword", "keyword", "keyword");
            template.AddTypeSetting(type);
            var result = client.CreateTemplate("template_123", template).Success;
            Assert.AreEqual(true, result);
            result = client.CreateIndex("index_1123").Success;
            Assert.AreEqual(true, result);
            var a = client.Index("index_1123", "type88", "key1", "{name:\"张三\"}");
            Assert.AreEqual(true, a.Success);
            a = client.Index("index_1123", "type88", "key2", "{name:\"张三丰\"}");
            Assert.AreEqual(true, a.Success);
            a = client.Index("index_1123", "type88", "key3", "{name:\"大张旗鼓\"}");
            Assert.AreEqual(true, a.Success);
            a = client.Index("index_1123", "type88", "key4", "{name:\"子张怡\"}");
            Assert.AreEqual(true, a.Success);
            Thread.Sleep(2000);
            var count = client.Count("index_1123", "type88", "name:张");
            Assert.AreEqual(0, count);
            count = client.Count("index_1123", "type88", "name:张三");
            Assert.AreEqual(1, count);
            count = client.Count("index_1123", "type88", "name:张三丰");
            Assert.AreEqual(1, count);
            count = client.Count("index_1123", "type88", "name:张三*");
            Assert.AreEqual(2, count);
            count = client.Count("index_1123", "type88", "name:张三*");
            Assert.AreEqual(2, count);
            count = client.Count("index_1123", "type88", "name:张三?");
            Assert.AreEqual(1, count);
            count = client.Count("index_1123", "type88", "name:*张*");
            Assert.AreEqual(4, count);
            count = client.Count("index_1123", "type88", "name:?张*");
            Assert.AreEqual(2, count);

            //test for _all
            //only works after setting default_analysis to keyword(or set against filed: _all )
//			count = client.Count("index_1123", "type88", "_all:张");
//			Assert.AreEqual(0, count);
//			count = client.Count("index_1123", "type88", "_all:张三");
//			Assert.AreEqual(1, count);
//			count = client.Count("index_1123", "type88", "_all:张三丰");
//			Assert.AreEqual(1, count);
//			count = client.Count("index_1123", "type88", "_all:张三*");
//			Assert.AreEqual(2, count);
//			count = client.Count("index_1123", "type88", "_all:张三*");
//			Assert.AreEqual(2, count);
//			count = client.Count("index_1123", "type88", "_all:张三?");
//			Assert.AreEqual(1, count);
//			count = client.Count("index_1123", "type88", "_all:*张*");
//			Assert.AreEqual(4, count);
//			count = client.Count("index_1123", "type88", "_all:?张*");
//			Assert.AreEqual(2, count);
        }

        [Test]
        public void TestTemplate()
        {
            var tempkey = "test_template_key1";

            client.DeleteTemplate(tempkey);

            var template = new TemplateSetting(tempkey);
            template.Template = "business_*";
            template.IndexSetting = new TemplateIndexSetting(3, 2);

            var type1 = new TypeSetting("mytype") { };
            type1.AddNumField("identity", NumType.Float);
            type1.AddDateField("datetime");

            var type2 = new TypeSetting("mypersontype");
            type2.AddStringField("personid");

            type2.SourceSetting = new SourceSetting();
            type2.SourceSetting.Enabled = false;

            template.AddTypeSetting(type1);
            template.AddTypeSetting(type2);

            var jsonstr = JsonSerializer.Get(template);
            Console.WriteLine(jsonstr);

            var result = client.CreateTemplate(tempkey, template);
            Console.WriteLine(result.JsonString);
            Assert.AreEqual(true, result.Success);

            result = client.CreateIndex("business_111");
            Assert.AreEqual(true, result.Success);
            result = client.CreateIndex("business_132");
            Assert.AreEqual(true, result.Success);
            result = client.CreateIndex("business_31003");
            Assert.AreEqual(true, result.Success);

            var temp = client.GetTemplate(tempkey);

            TemplateSetting result1;
            Assert.AreEqual(true, temp.TryGetValue(tempkey, out result1));
            Assert.AreEqual(template.Order, result1.Order);
            Assert.AreEqual(template.Template, result1.Template);

            result = client.DeleteTemplate(tempkey);
            Assert.AreEqual(true, result.Success);
            temp = client.GetTemplate(tempkey);
            Assert.IsEmpty(temp);
        }
        [TestFixtureTearDown]
        public void CleanUp()
        {
            client.DeleteTemplate("template_123");
            client.DeleteIndex("index_1123");
            client.DeleteIndex("business_111");
            client.DeleteIndex("business_132");
            client.DeleteIndex("business_31003");
        }
    }
}