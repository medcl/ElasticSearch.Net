using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using ElasticSearch.Client;
using ElasticSearch.Client.Admin;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.EMO;
using ElasticSearch.DataManager.Dialogs;

namespace ElasticSearchDataManager
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			treeViewAdv1.ShowNodeToolTips = true;
			treeViewAdv1.DefaultToolTipProvider = new ElasticTooltipProvider();
			foreach (var variable in ElasticSearchConfig.Instance.Clusters)
			{
				toolStripComboBox1.Items.Add(variable);
			}
			//			DuplicateCheck();
		}

		private void DuplicateCheck()
		{
			//			var result= descClient.Search("setting.tms.beisen.com", "*", 0, 500);
			//			foreach (var hitse in result.GetHits().Hits)
			//			{
			//				if (hitse.Fields.ContainsKey("_tenantid") && hitse.Fields.ContainsKey("Userid"))
			//				{
			//					var key = string.Format("{0}-{1}-{2}", hitse.Fields["_tenantid"], hitse.Fields["Userid"], hitse.Type);
			//					if(cache.ContainsKey(key))
			//					{
			//						var temp = cache[key];
			//						cache[key] = ++temp;
			//					}
			//					else
			//					{
			//						cache[key] = 1;
			//					}}
			//			}
			//			foreach (var i in cache)
			//			{
			//				if(i.Value>1){WriteLog("{0} : {1}",i.Key,i.Value);}
			//			}
		}

		Regex tenantRegex = new Regex("[0-9]+");


		private void connectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (toolStripComboBox1.Text != null && !string.IsNullOrEmpty(toolStripComboBox1.Text))
			{
				string clusterName = toolStripComboBox1.Text;
				var srcClient = new ElasticSearchClient(clusterName);

				InitTree(clusterName, srcClient);
			}
		}

		private void InitTree(string clusterName, ElasticSearchClient instance)
		{
			var model = new TreeModel();
			if (instance != null)
			{
				var indices = instance.Status();
				var node = new ElasticNode(clusterName);
				model.Root.Nodes.Add(node);
				var sortedIndices = indices.IndexStatus.OrderBy(d => d.Key);
				foreach (var index in sortedIndices)
				{
					var tempNode = new ElasticNode(string.Format("{0} ({1})", index.Key, index.Value.DocStatus.NumDocs));
					tempNode.ElasticSearchInstance = instance;
					tempNode.Tag = index;
					tempNode.IndexName = index.Key;
					tempNode.IndexStatus = index.Value;
					node.Nodes.Add(tempNode);
				}
			}
			treeViewAdv1.Model = model;
			treeViewAdv1.Refresh();
		}


		private void ExportDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Connect connect = new Connect();
			if (connect.ShowDialog() == DialogResult.OK)
			{
				ElasticSearchClient descClient = new ElasticSearchClient(connect.Host, connect.Port, connect.Type);

				Export export = new Export();
				if (export.ShowDialog() == DialogResult.OK)
				{
					var toIndex = export.IndexName;
					var limit = export.LimitSize;
					var buffer = export.BufferSize;
					var bulkSize = export.BulkSize;

					foreach (var selectedNode in treeViewAdv1.SelectedNodes)
					{
						var elasticNode = ((ElasticNode)(selectedNode.Tag));
						var index = elasticNode.IndexName;
						WriteLog("Export for: {0}", selectedNode.Tag);

						var tenantId = tenantRegex.Match(index).Value;

						int bufferSize = buffer;
						int limitSize = limit;
						new Thread(new ThreadStart(delegate()
													{
														var start = DateTime.Now;
														var total = elasticNode.ElasticSearchInstance.Count(index, "*");
														if (limitSize > total) limitSize = total;
														if (bufferSize > limitSize) bufferSize = limitSize;
														WriteLog("Transform Index : {0} To Index : {1},{2} Pending Docs", index, toIndex, limitSize);

														for (int i = 0; i < limitSize; i += bufferSize)
														{
															IndexTransfer(index, toIndex, i, bufferSize, bulkSize, elasticNode.ElasticSearchInstance, descClient);
														}


														WriteLog("Index : {0} Transform Finished,Time Elapsed : {1}", index,
																 DateTime.Now - start);
													})).Start();

					}
				}
			}

		}

		private void IndexTransfer(string index, string toIndex, int from, int limit, int bulkSize, ElasticSearchClient srcClient, ElasticSearchClient descClient)
		{
			var docs = srcClient.Search(index, "*", from, limit, "_id:asc");
			WriteLog("Search:{0},{1},{2}", index, from, limit);
			int i = 0;
			var bulkObjects = new List<BulkObject>();
			foreach (var variable in docs.GetHits().Hits)
			{
				#region logging

				//				WriteLog("\tIndex:{0}", variable.Index);
				//				WriteLog("\tType:{0}", variable.Type);
				//				WriteLog("\tId:{0}", variable.Id);
				Dictionary<string, object> fields = variable.Fields;
				//				WriteLog("\tTotalFieldsCount:{0}", fields.Count);

				#endregion

				#region

				//					                           			foreach (var VARIABLE in fields)
				//					                           			{
				//WriteLog(string.Format("\t\t{0}:{1}", VARIABLE.Key, VARIABLE.Value));
				//					                           			}

				#endregion

				#region multi-tenant

				if (index.StartsWith("setting"))
				{
					fields["_tenantid"] = fields["__TENANTID"];
					fields.Remove("__TENANTID");
					fields.Remove("__TYPEID");
				}

				#endregion

				#region BulkInsert

				i++;
				bulkObjects.Add(new BulkObject()
									{
										Id = variable.Id,
										Index = toIndex,
										Type = variable.Type,
										Fields = fields
									});

				if (i > bulkSize)
				{
					descClient.Bulk(bulkObjects);
					bulkObjects.Clear();
					i = 0;
					WriteLog("Buik Commit.");
				}

				#endregion
			}

			#region final cleanup

			if (i > 0)
			{
				descClient.Bulk(bulkObjects);
				WriteLog("Final Cleanup,{0}.", bulkObjects.Count);
				bulkObjects.Clear();
			}

			#endregion

		}

		void WriteLog(string format, params object[] args)
		{
			var message = string.Format(format + "\r\n", args);
			CallDelegate callDelegate = delegate
			{
				this.richTextBox1.AppendText(message);
				richTextBox1.Focus();
				this.richTextBox1.Select(this.richTextBox1.TextLength, 0);
				this.richTextBox1.ScrollToCaret();
			};
			if (this.richTextBox1.InvokeRequired)
			{
				this.richTextBox1.Invoke(callDelegate);
			}
			else
			{
				this.richTextBox1.AppendText(message);
				this.richTextBox1.SelectionStart = richTextBox1.Text.Length;
				richTextBox1.Focus();
			}

		}


		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox1.Clear();
		}

		/// <summary>
		/// delete index
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<ElasticNode> pendingDelete = new List<ElasticNode>(treeViewAdv1.SelectedNodes.Count);
			foreach (TreeNodeAdv selectedNode in treeViewAdv1.SelectedNodes)
			{
				var elasticNode = (ElasticNode)(selectedNode.Tag);
				var index = elasticNode.IndexName;
				WriteLog("Delete Index: {0}", selectedNode.Tag);
				elasticNode.ElasticSearchInstance.DeleteIndex(index);
				pendingDelete.Add(elasticNode);
			}
			//clean nodes
			foreach (var selectedNode in pendingDelete)
			{
				selectedNode.Parent = null;
			}
		}
		/// <summary>
		/// reload tree
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void aDDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var connect = new Connect();
			if (connect.ShowDialog() == DialogResult.OK)
			{
				var srcClient = new ElasticSearchClient(connect.Host, connect.Port, connect.Type);

				InitTree(connect.Host, srcClient);
			}
		}

		/// <summary>
		/// show top 5
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void viewTop5ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (var selectedNode in treeViewAdv1.SelectedNodes)
			{
				var elasticNode = ((ElasticNode)(selectedNode.Tag));
				var index = elasticNode.IndexName; //treeViewAdv1.SelectedNode.Tag as string;
				WriteLog("View Index: {0}", selectedNode.Tag);
				var result = elasticNode.ElasticSearchInstance.Search(index, "*", 0, 5);
				JsonView jsonView = new JsonView();
				jsonView.LoadJson(result.JsonString);
				jsonView.ShowDialog();
			}
		}

		private void treeViewAdv1_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
		{
			if (treeViewAdv1.SelectedNodes.Count == 1)
			{
				var tempNode = (ElasticNode)treeViewAdv1.SelectedNode.Tag;
				if (tempNode != null && tempNode.IndexStatus != null)
				{
					toolStripStatusLabel1.Text = tempNode.IndexStatus.ToString();
				}
			}
		}

		public class ElasticNode : Node
		{
			public string IndexName { get; set; }
			public ElasticNode(string text) : base(text) { }
			public ElasticSearchClient ElasticSearchInstance { get; set; }

			public IndexStatus IndexStatus { get; set; }
		}
		public class ElasticTooltipProvider : IToolTipProvider
		{
			public string GetToolTip(TreeNodeAdv node, NodeControl nodeControl)
			{
				if (node.Tag is ElasticNode)
				{
					var tempNode = (ElasticNode)node.Tag;
					if (tempNode.IndexStatus != null)
					{
						return tempNode.IndexStatus.ToString();
					}

				}
				return string.Empty;
			}
		}
		internal delegate void CallDelegate();
	}
}
