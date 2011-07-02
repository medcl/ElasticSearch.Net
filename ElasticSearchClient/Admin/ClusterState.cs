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
		[JsonProperty("index")]
		public IndexStoreStatus StoreStatus;

		[JsonProperty("docs")]
		public DocStatus DocStatus;

		[JsonProperty("translog")]
		public TrasnslogStatus TrasnslogStatus;

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", StoreStatus,
				                                           DocStatus,TrasnslogStatus);
		}
	}
	
	public class TrasnslogStatus
	{
		[JsonProperty("operations")]
		public int Operations { get; set; }
		public override string ToString()
		{
			return string.Format("operations:{0}", Operations);
		}
	}

	public class IndexStoreStatus
	{
		[JsonProperty("primary_size")]
		public string PrimarySize { get; set; }

		[JsonProperty("primary_size_in_bytes")]
		public string PrimarySizeInBytes { get; set; }

		[JsonProperty("size")]
		public string Size { get; set; }

		[JsonProperty("size_in_bytes")]
		public string SizeInBytes { get; set; }

		public override string ToString()
		{
			return string.Format("size:{0}", Size);
//			string.Format("primary_size:{0},primary_size_in_bytes:{1},size:{2},size_in_bytes:{3}", PrimarySize, PrimarySizeInBytes, Size, SizeInBytes);
		}
		
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
			return string.Format("num:{0},del:{1},max:{2}", NumDocs, DeletedDocs, MaxDoc);
		}
	}
}