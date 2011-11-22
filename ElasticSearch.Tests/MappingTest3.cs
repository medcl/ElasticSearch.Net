using ElasticSearch.Client;
using ElasticSearch.Client.Mapping;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MappingTest3
    {
        [Test]
        public void TestCreateParentChildType()
        {
            var index = "index_test_parent_child_type";
            var parentType = new TypeSetting("blog");
            parentType.AddStringField("title");

            var client = new ElasticSearchClient("localhost");
            var op= client.PutMapping(index, parentType);

            Assert.AreEqual(true,op.Acknowledged);

            var childType = new TypeSetting("comment",parentType);
            childType.AddStringField("comments");

            op=client.PutMapping(index,childType);
            Assert.AreEqual(true, op.Acknowledged);

            var mapping=client.GetMapping(index, "comment");

            Assert.True(mapping.IndexOf("_parent")>0);

            client.DeleteIndex(index);
        }
    }
}