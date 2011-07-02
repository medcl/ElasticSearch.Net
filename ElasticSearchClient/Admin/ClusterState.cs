using System.Collections.Generic;
using ElasticSearch.Client.EMO;
using Newtonsoft.Json;

namespace ElasticSearch.Client.Admin
{
	public class ClusterState
	{
		[JsonProperty("cluster_name")]
		public string ClusterName;

		[JsonProperty("master_node")] 
		public string MasterNode;

	}

	public class ClusterIndexStatus
	{
		[JsonProperty("ok")]
		public bool Success;

		[JsonProperty("_shards")]
		public ShardStatus ShardStatus;

		[JsonProperty("indices")]
		public Dictionary<string, IndexStatus> IndexStatus;

	}

	public class IndexStatus
	{
		[JsonProperty("store_size")]
		public string StoreSize;

		[JsonProperty("store_size_in_bytes")]
		public string StoreSizeInBytes;

		[JsonProperty("translog_operations")] 
		public string TranslogOperations;

		[JsonProperty("docs")]
		public DocStatus DocStatus;
	}
	
	public class DocStatus
	{
		[JsonProperty("num_docs")]
		public int NumDocs;

		[JsonProperty("max_doc")] 
		public int MaxDoc;

		[JsonProperty("deleted_docs")] 
		public int DeletedDocs;

		public override string ToString()
		{
			return string.Format("Num:{0},Del:{1},Max:{2}", NumDocs, DeletedDocs, MaxDoc);
		}
	}
}