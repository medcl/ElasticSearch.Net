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
	public class TemplateTests
	{
		ElasticSearchClient client=new ElasticSearchClient("localhost");
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
            
		    client.Refresh();
			var temp = client.GetTemplate(tempkey);

			TemplateSetting result1;
			Assert.AreEqual(true, temp.TryGetValue(tempkey, out result1));
			Assert.AreEqual(template.Order, result1.Order);
			Assert.AreEqual(template.Template, result1.Template);

			client.DeleteIndex("business_111");
			client.DeleteIndex("business_132");
			client.DeleteIndex("business_31003");
		}
	}
}