using System.Collections.Generic;
using ElasticSearch.Client;
using ElasticSearch.Client.Config;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class ClusterSetupTests
	{
		[Test]
		public void TestDynamicUserClusters()
		{
			ElasticSearchClient client = new ElasticSearchClient("127.0.0.1", 9200, TransportType.Http);

			List<string> indices= client.GetIndices();

			client=new ElasticSearchClient("127.0.0.1",9500,TransportType.Thrift);

			indices= client.GetIndices();
		}
	}
}