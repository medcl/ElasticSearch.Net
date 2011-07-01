using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Exception;
using ElasticSearch.Client.Utils;
using Thrift.Transport;

namespace ElasticSearch.Client.Transport
{
	internal class ESNodeManager
	{
		public static readonly ESNodeManager Instance = new ESNodeManager();
		private static readonly LogWrapper logger = LogWrapper.GetLogger();
		private readonly Random _rand;
		private ElasticSearchConfig _config;
		private Dictionary<string,TransportType>Clusters=new Dictionary<string, TransportType>();
		private Dictionary<string ,List<string>>  ClusterHttpNodes=new Dictionary<string, List<string>>();
		private Dictionary<string ,List<ESNode>>   ClusterThriftNodes=new Dictionary<string, List<ESNode>>();

		private ESNodeManager()
		{
			_config = ElasticSearchConfig.Instance;

			foreach (var esNode in _config.Clusters)
			{
				BuildCluster(esNode.ClusterName,esNode.TransportType);
				ClusterThriftNodes[esNode.ClusterName] = BuildThriftNodes(esNode.ThriftNodes);
				ClusterHttpNodes[esNode.ClusterName] = BuildHttpNodes(esNode.HttpNodes);
			}
			_rand = new Random((int) DateTime.Now.Ticks);
			ElasticSearchConfig.ConfigChanged += ElasticSearchConfig_ConfigChanged;
		}

		private void ElasticSearchConfig_ConfigChanged(object sender, EventArgs e)
		{
			var elasticSearchConfig = sender as ElasticSearchConfig;
			if (elasticSearchConfig != null)
			{
				logger.Info("ElasticSearchConfig config reloading");
				_config = elasticSearchConfig;
				foreach (var esNode in _config.Clusters)
				{
					 BuildCluster(esNode.ClusterName,esNode.TransportType);
					 ClusterThriftNodes[esNode.ClusterName] = BuildThriftNodes(esNode.ThriftNodes);
					 ClusterHttpNodes[esNode.ClusterName] = BuildHttpNodes(esNode.HttpNodes);	
				}
				logger.Info("ElasticSearchConfig config reloaded");
			}
			else
			{
				logger.Error("Attempt to reload with null ElasticSearchConfig config");
			}
		}

		private static List<string> BuildHttpNodes(NodeDefinition[] definitions)
		{
			var result = new List<string>();
			if (definitions != null && definitions.Length > 0)
			{
				foreach (NodeDefinition nodeDefinition in definitions)
				{
					if (nodeDefinition.Enabled)
					{
						if (nodeDefinition.Port <= 0||nodeDefinition.Port>65534)
						{
							nodeDefinition.Port = 80;
						}
						result.Add("http://" + nodeDefinition.Host.Trim() + ":" + nodeDefinition.Port);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Dynamic Adding Cluster Definition
		/// </summary>
		/// <param name="clusterName"></param>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		/// <param name="transportType"></param>
		public void BuildCustomNodes(string clusterName,string ip,int port,TransportType transportType)
		{
			BuildCluster(clusterName,transportType);

			if (transportType == TransportType.Thrift)
			{	List<ESNode> nodes;
				ClusterThriftNodes.TryGetValue(clusterName, out nodes);
				if(nodes==null)nodes=new List<ESNode>();
				nodes.Add(new ESNode(ip, port));
				ClusterThriftNodes[clusterName] = nodes;
			}
			else
			{
				List<string> nodes;
				ClusterHttpNodes.TryGetValue(clusterName, out nodes);
				if (nodes == null)nodes=new List<string>();
				nodes.Add("http://" + ip.Trim() + ":" + port);
				ClusterHttpNodes[clusterName] = nodes;
			}
		}

		private static List<ESNode> BuildThriftNodes(NodeDefinition[] definitions)
		{
			var result = new List<ESNode>();
			if (definitions != null && definitions.Length > 0)
			{
				foreach (NodeDefinition nodeDefinition in definitions)
				{
					if (nodeDefinition.Enabled)
					{
						result.Add(new ESNode(nodeDefinition));
					}
				}
			}
			return result;
		}
		
		public ESNode GetThriftNode(string clusterName)
		{
			List<ESNode> nodes;
			ClusterThriftNodes.TryGetValue(clusterName, out nodes);
			if (nodes != null)
			{
				var candidates = new List<ESNode>(from node in nodes
				                                  where  !node.InDangerZone
				                                  select node);
				if (candidates.Count > 0)
				{
					return candidates[_rand.Next(candidates.Count)];
				}
			}
			throw new ElasticSearchException("no live node available");
		}

		public string GetHttpNode(string clusterName)
		{
			List<string> httpNodes;
			ClusterHttpNodes.TryGetValue(clusterName, out httpNodes);
			if (httpNodes != null)
				if (httpNodes.Count > 0)
				{
					return httpNodes[_rand.Next(httpNodes.Count)];
				}
			throw new ElasticSearchException("no live node available");
		}

		internal void AggregateCounterTicker(object state)
		{
			foreach (var nodes in ClusterThriftNodes.Values)
			{
				foreach (ESNode node in nodes)
					node.AggregateCounterTicker();	
			}
			
		}

		public void BuildCluster(string clustetName,TransportType transportType)
		{
			Clusters[clustetName]=transportType;
		}
		public TransportType GetClusterType(string clusterName)
		{
			if (Clusters.ContainsKey(clusterName))
			{
				return Clusters[clusterName];
			}
			throw new ElasticSearchException("cluster not found");
		}
	}
}