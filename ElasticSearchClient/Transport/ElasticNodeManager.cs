using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Exception;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Transport
{
	internal class ESNodeManager
	{
		public static readonly ESNodeManager Instance = new ESNodeManager();
		private static readonly LogWrapper logger = LogWrapper.GetLogger();
		private readonly Random rand;
		private ElasticSearchConfig config;
		private Dictionary<string ,List<string>>  ClusterHttpNodes;
		private Dictionary<string ,List<ESNode>>   ClusterThriftNodes;
		private Timer aggregateCounterTickTimer;
		private ESNodeManager()
		{
			config = ElasticSearchConfig.Instance;

			foreach (var esNode in config.Clusters)
			{
				ClusterThriftNodes[esNode.ClusterName] = BuildNodes(esNode.ThriftNodes);
				ClusterHttpNodes[esNode.ClusterName] = BuildHttpNodes(esNode.HttpNodes);
			}
			rand = new Random((int) DateTime.Now.Ticks);
			ElasticSearchConfig.ConfigChanged += ElasticSearchConfig_ConfigChanged;
			aggregateCounterTickTimer = new Timer(AggregateCounterTicker, null, 500, 500);
		}

		private void ElasticSearchConfig_ConfigChanged(object sender, EventArgs e)
		{
			var elasticSearchConfig = sender as ElasticSearchConfig;
			if (elasticSearchConfig != null)
			{
				logger.Info("ElasticSearchConfig config reloading");
				config = elasticSearchConfig;
				foreach (var esNode in config.Clusters)
				{
					 ClusterThriftNodes[esNode.ClusterName] = BuildNodes(esNode.ThriftNodes);
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
						if (nodeDefinition.Port <= 0)
						{
							nodeDefinition.Port = 80;
						}
						result.Add("http://" + nodeDefinition.Host.Trim() + ":" + nodeDefinition.Port);
					}
				}
			}
			return result;
		}

		private static List<ESNode> BuildNodes(NodeDefinition[] definitions)
		{
			var result = new List<ESNode>();
			if (definitions != null && definitions.Length > 0)
			{
				foreach (NodeDefinition nodeDefinition in definitions)
					result.Add(new ESNode(nodeDefinition));
			}
			return result;
		}
		
		public ESNode GetNode(string clusterName)
		{
			List<ESNode> nodes;
			ClusterThriftNodes.TryGetValue(clusterName, out nodes);
			if (nodes != null)
			{
				var candidates = new List<ESNode>(from node in nodes
				                                  where node.Enabled && !node.InDangerZone
				                                  select node);
				if (candidates.Count > 0)
				{
					return candidates[rand.Next(candidates.Count)];
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
					return httpNodes[rand.Next(httpNodes.Count)];
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
	}
}