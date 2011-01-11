using System;
using ElasticSearch.Client;
using ElasticSearch.Mapping;
using ElasticSearch.Utils;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class TemplateTests
	{
		[Test]
		public void TestTemplate()
		{
			var tempkey = "test_template_key1";
			var template = new TemplateSetting(tempkey);
			template.Template = "business_*";
			template.IndexSetting = new IndexSetting(3, 2);

			var type1 = new TypeSetting("mytype") { };
			type1.CreateNumField("identity", NumType.Float);
			type1.CreateDateField("datetime");

			var type2 = new TypeSetting("mypersontype");
			type2.CreateStringField("personid");

			type2.SourceSetting = new SourceSetting();
			type2.SourceSetting.Enabled = false;

			template.AddTypeSetting(type1);
			template.AddTypeSetting(type2);

			var jsonstr = JsonSerializer.Get(template);
			Console.WriteLine(jsonstr);

			var result = ElasticSearchClient.Instance.CreateTemplate(tempkey, template);
			Console.WriteLine(result.JsonString);
			Assert.AreEqual(true, result.Success);

			result = ElasticSearchClient.Instance.CreateIndex("business_111");
			Assert.AreEqual(true, result.Success);
			result = ElasticSearchClient.Instance.CreateIndex("business_132");
			Assert.AreEqual(true, result.Success);
			result = ElasticSearchClient.Instance.CreateIndex("business_31003");
			Assert.AreEqual(true, result.Success);

			var temp = ElasticSearchClient.Instance.GetTemplate(tempkey);

			TemplateSetting result1;
			Assert.AreEqual(true, temp.TryGetValue(tempkey, out result1));
			Assert.AreEqual(template.Order, result1.Order);
			Assert.AreEqual(template.Template, result1.Template);

			ElasticSearchClient.Instance.DeleteIndex("business_111");
			ElasticSearchClient.Instance.DeleteIndex("business_132");
			ElasticSearchClient.Instance.DeleteIndex("business_31003");
		}
	}
}