using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearch.Config;
using ElasticSearch.Exception;
using ElasticSearch.Utils;

namespace ElasticSearch
{
	internal class ESNodeManager
	{
		public static readonly ESNodeManager Instance = new ESNodeManager();
		private static readonly LogWrapper logger = LogWrapper.GetLogger();
		private readonly Random rand;
		private ElasticSearchConfig config;
		private List<string> httpNodes;
		private List<ESNode> nodes;

		private ESNodeManager()
		{
			config = ElasticSearchConfig.Instance;
			nodes = BuildNodes(config.ThriftNodes);
			httpNodes = BuildHttpNodes(config.HttpNodes);
			rand = new Random((int) DateTime.Now.Ticks);
			ElasticSearchConfig.ConfigChanged += ElasticSearchConfig_ConfigChanged;
		}

		private void ElasticSearchConfig_ConfigChanged(object sender, EventArgs e)
		{
			var elasticSearchConfig = sender as ElasticSearchConfig;
			if (elasticSearchConfig != null)
			{
				logger.Info("ElasticSearchConfig config reloading");
				config = elasticSearchConfig;
				Instance.ReloadConfig(config);
				httpNodes = BuildHttpNodes(config.HttpNodes);
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

		public void ReloadConfig(ElasticSearchConfig definition)
		{
			var newNodes = new List<ESNode>(nodes);

			// remove no longer exist nodes
			var removeList = new List<ESNode>();
			foreach (ESNode node in newNodes)
			{
				if (!definition.ContainsNode(definition.ThriftNodes, node.Host, node.Port))
					removeList.Add(node);
			}

			foreach (ESNode node in removeList)
				newNodes.Remove(node);

			// add new nodes & reload already exist nodes config
			var map = new Dictionary<string, ESNode>(newNodes.Count);
			foreach (ESNode node in newNodes)
				map.Add(node.ToString(), node);

			foreach (NodeDefinition nodeDefinition in definition.ThriftNodes)
			{
				ESNode node;
				if (!map.TryGetValue(nodeDefinition.ToString(), out node))
				{
					if (logger.IsDebugEnabled)
						logger.InfoFormat("Add new node '{0}' ", nodeDefinition.ToString());
					newNodes.Add(new ESNode(nodeDefinition));
				}
			}
			nodes = newNodes;
		}

		public ESNode GetNode()
		{
			var candidates = new List<ESNode>(from node in nodes
			                                  where node.Enabled && !node.InDangerZone
			                                  select node);
			if (candidates.Count > 0)
			{
				return candidates[rand.Next(candidates.Count)];
			}
			throw new ElasticSearchException("no live node available");
		}

		public string GetHttpNode()
		{
			if (httpNodes.Count > 0)
			{
				return httpNodes[rand.Next(httpNodes.Count)];
			}
			throw new ElasticSearchException("no live node available");
		}

		internal void AggregateCounterTicker()
		{
			foreach (ESNode node in nodes)
				node.AggregateCounterTicker();
		}
	}
}