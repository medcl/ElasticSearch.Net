using System;
using ElasticSearch.Client;
using ElasticSearch.Client.EMO;
using ElasticSearch.Client.Mapping;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class MappingTests
	{
		public ElasticSearchClient client = new ElasticSearchClient("localhost");
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
			var index = "index_operate" + Guid.NewGuid().ToString();
			client.CreateIndex(index, new IndexSetting(10, 1));

			var result2 = client.DeleteIndex(index);
			Assert.AreEqual(true, result2.Success);
		}
		[Test]
		public void TestModifyIndex()
		{
			var index = "index_operate" + Guid.NewGuid().ToString();
			client.CreateIndex(index, new IndexSetting(10, 1));
			client.ModifyIndex(index, new IndexSetting(10, 2));

			var result2 = client.DeleteIndex(index);
			Assert.AreEqual(true, result2.Success);
		}
	

	}
}