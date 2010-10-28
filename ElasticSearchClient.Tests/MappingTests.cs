using System;
using ElasticSearch.Client;
using ElasticSearch.Mapping;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class MappingTests
	{
		[Test]
		public void TestCreatingMapping()
		{
			var index = "index_operate" + Guid.NewGuid().ToString();
			StringFieldSetting stringFieldSetting = new StringFieldSetting() { Analyzer = "standard", Type = "string", NullValue = "mystr" };

			TypeSetting typeSetting = new TypeSetting("custom_type");
			typeSetting.AddFieldSetting("medcl", stringFieldSetting);

			var typeSetting2 = new TypeSetting("hell_type1");
			var numfield = new NumberFieldSetting() { Store = Store.yes, NullValue = 0.00 };
			typeSetting2.AddFieldSetting("name", numfield);

			var preparedata = ElasticSearchClient.Instance.Index(index, "_prepare", "_medcl", "{}");
			var result = ElasticSearchClient.Instance.PutMapping(index, typeSetting);
			Assert.AreEqual(true, result);

			result = ElasticSearchClient.Instance.PutMapping(index, typeSetting2);
			Assert.AreEqual(true, result);

			var result2 = ElasticSearchClient.Instance.DeleteIndex(index);
			Assert.AreEqual(true, result2.Success);
		}

		[Test]
		public void TestCreateIndex()
		{
			var index = "index_operate" + Guid.NewGuid().ToString();
			ElasticSearchClient.Instance.CreateIndex(index, new IndexSetting(10, 1));

			var result2 = ElasticSearchClient.Instance.DeleteIndex(index);
			Assert.AreEqual(true, result2.Success);
		}
		[Test]
		public void TestModifyIndex()
		{
			var index = "index_operate" + Guid.NewGuid().ToString();
			ElasticSearchClient.Instance.CreateIndex(index, new IndexSetting(10, 1));
			ElasticSearchClient.Instance.ModifyIndex(index, new IndexSetting(10, 2));

			var result2 = ElasticSearchClient.Instance.DeleteIndex(index);
			Assert.AreEqual(true, result2.Success);
		}
	}
}