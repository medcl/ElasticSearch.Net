using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using ElasticSearch.Client;
using ElasticSearch.Client.Admin;
using ElasticSearch.Client.Config;
using ElasticSearch.Client.Domain;
using ElasticSearch.Client.Mapping;
using ElasticSearch.Client.QueryDSL;
using ElasticSearch.Client.Utils;
using ElasticSearch.DataManager.Dialogs;
using Newtonsoft.Json.Linq;

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
					
		}
		Dictionary<string,int > cache=new Dictionary<string, int>();
		private void DuplicateCheck()
		{
			cache=new Dictionary<string, int>();
			var result = currentElasticSearchInstance.Search("index", "*", 0, 500);
			foreach (var hitse in result.GetHits().Hits)
			{
				if (hitse.Fields.ContainsKey("_tenantid") && hitse.Fields.ContainsKey("Userid"))
				{
					var key = string.Format("{0}-{1}-{2}", hitse.Fields["_tenantid"], hitse.Fields["Userid"], hitse.Type);
					if (cache.ContainsKey(key))
					{
						var temp = cache[key];
						cache[key] = ++temp;
					}
					else
					{
						cache[key] = 1;
					}
				}
				else if (hitse.Fields.ContainsKey("Userid"))
				{
					var key = string.Format("{0}-{1}-{2}", hitse.Fields["__TENANTID"], hitse.Fields["Userid"], hitse.Type);
					if (cache.ContainsKey(key))
					{
						var temp = cache[key];
						cache[key] = ++temp;
					}
					else
					{
						cache[key] = 1;
					}
				}
			}
			foreach (var i in cache)
			{
				if (i.Value > 1) { WriteLog("{0} : {1}", i.Key, i.Value); }
			}
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

		private string currentCluster;
		private ElasticSearchClient currentElasticSearchInstance;
		private void InitTree(string clusterName, ElasticSearchClient instance)
		{
			var model = new TreeModel();
			if (instance != null)
			{
				var indices = instance.Status();
				if(indices==null){throw new ServerException("can't connect to server");}
				currentCluster = clusterName;
				currentElasticSearchInstance = instance;
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
					var complicatedSource = export.ComplicatedSource;
					var resolveTenant = export.ResolveTenant;
					var showLog = export.ShowLog;
				    var skipCount = export.SkipCount;

					foreach (var selectedNode in treeViewAdv1.SelectedNodes)
					{
						var elasticNode = ((ElasticNode)(selectedNode.Tag));
						var index = elasticNode.IndexName;
						WriteLog("Export for: {0}", selectedNode.Tag);

						int bufferSize = buffer;
						int limitSize = limit;
						new Thread(new ThreadStart(delegate()
													{
														var start = DateTime.Now;
														var total = elasticNode.ElasticSearchInstance.Count(index, "*");
														if (limitSize > total) limitSize = total;
														if (bufferSize > limitSize) bufferSize = limitSize;
														WriteLog("Transform Index : {0} To Index : {1},{2} Pending Docs", index, toIndex, limitSize);

                                                        for (int i = skipCount; i < limitSize; i += bufferSize)
														{
															IndexTransfer(index, toIndex, i, bufferSize, bulkSize, elasticNode.ElasticSearchInstance, descClient, complicatedSource, resolveTenant, showLog);
														}


														WriteLog("Index : {0} Transform Finished,Time Elapsed : {1}", index,
																 DateTime.Now - start);
													})).Start();

					}
				}
			}

		}

		private void IndexTransfer(string index, string toIndex, int from, int limit, int bulkSize, ElasticSearchClient srcClient, ElasticSearchClient descClient, bool complicatedSource,bool resolveTenant,bool showLog)
		{
			var docs = srcClient.Search(index, "*", from, limit, "_id:asc");
			WriteLog("Search:{0},{1},{2}", index, from, limit);
			int i = 0;
			var bulkObjects = new List<BulkObject>();
			
			if(complicatedSource)
			{
				//complicated object
				if (!string.IsNullOrEmpty(docs.Response))
				{
					var obj = JObject.Parse(docs.Response);
					var hits = obj["hits"]["hits"];
					foreach (var hit in hits)
					{
						var source = ((Newtonsoft.Json.Linq.JObject)(hit["_source"])).ToString().Replace("\r\n",string.Empty);// hit["_source"].Value<string>();
						var _type = hit["_type"].Value<string>();
						var _id = hit["_id"].Value<string>();

						if (resolveTenant)
						{
							//methond for setting
							//if (index.StartsWith("setting") || index.StartsWith("labs.setting") || index.StartsWith("demo.setting"))
							{
								var fields = ElasticSearch.Client.Utils.JsonSerializer.Get<Dictionary<string, object>>(source);

								fields=ParseFieldValue(fields);
								source = ElasticSearch.Client.Utils.JsonSerializer.Get(fields);
							}
						}


						i++;
						
						if(showLog)
						{
							WriteLog("curl -XPOST http://localhost:9200/{0}/{1}/{2} -d'{3}'", toIndex, _type, _id, source);
						}

						bulkObjects.Add(new BulkObject(toIndex,_type, _id, source));
						if (i > bulkSize)
						{
							descClient.Bulk(bulkObjects);
							bulkObjects.Clear();
							i = 0;
							WriteLog("Buik Commit.");
						}
					}
				}
			}
			else
			{
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
				if (resolveTenant)
				{
					if (index.StartsWith("setting") || index.StartsWith("labs.setting") || index.StartsWith("demo.setting"))
					{

 						fields= ParseFieldValue(fields);
//						fields["_tenantid"] = fields["__TENANTID"];
//						fields.Remove("__TENANTID");
//						fields.Remove("__TYPEID");
					}
				}

				#endregion

				#region BulkInsert

				i++;
				bulkObjects.Add(new BulkObject( toIndex, variable.Type,variable.Id, fields));


				if (i > bulkSize)
				{
					descClient.Bulk(bulkObjects);
					bulkObjects.Clear();
					i = 0;
					WriteLog("Buik Commit.");
				}

				#endregion
			}
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

		private static Dictionary<string, object> ParseFieldValue(Dictionary<string, object> fields)
		{
			if (fields.ContainsKey("__TENANTID"))
			{
				fields["_tenantid"] = int.Parse(fields["__TENANTID"].ToString());
				fields.Remove("__TENANTID");
			}
			if (fields.ContainsKey("_tenantid"))
			{
				fields["_tenantid"] = int.Parse(fields["_tenantid"].ToString());
			}

			if (fields.ContainsKey("__TYPEID"))
			{
				fields.Remove("__TYPEID");
			}
			return fields;
		}

		void WriteLog(string message)
		{
			CallDelegate callDelegate = delegate
			{
                if (richTextBox1.Lines.Length > 500)
                {
                    richTextBox1.Clear();
                }
                if (!message.EndsWith("\r\n"))
                {
                    this.richTextBox1.AppendText("\r\n");
                }
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
                if (richTextBox1.Lines.Length > 500)
                {
                    richTextBox1.Clear();
                }
                if (!message.EndsWith("\r\n"))
                {
                    this.richTextBox1.AppendText("\r\n");
                }
				this.richTextBox1.AppendText(message);
				this.richTextBox1.SelectionStart = richTextBox1.Text.Length;
				richTextBox1.Focus();
			}
		}
		void WriteLog(string format, params object[] args)
		{
			var message = string.Format(format + "\r\n", args);
			WriteLog(message);
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
			if (MessageBox.Show("are you sure you really wanna delete these index?", "hey", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
		}
		Connect connect = new Connect();
		/// <summary>
		/// reload tree
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void aDDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
			if (connect.ShowDialog() == DialogResult.OK)
			{
				var srcClient = new ElasticSearchClient(connect.Host, connect.Port, connect.Type);
				var cluster = string.Format("{0}:{1}", connect.Host, connect.Port);
				InitTree(cluster, srcClient);
				//DuplicateCheck();
			}
		}
		JsonView 	jsonView = new JsonView();
		/// <summary>
		/// show top 5
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void viewTop5ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var selectedNode = treeViewAdv1.SelectedNode;
			
				var elasticNode = ((ElasticNode)(selectedNode.Tag));
				var index = elasticNode.IndexName; //treeViewAdv1.SelectedNode.Tag as string;
				WriteLog("View Index: {0}", selectedNode.Tag);
				var result = elasticNode.ElasticSearchInstance.Search(index, "*", 0, 5);
				
		
			jsonView.LoadJson(result.Response);
				jsonView.ShowDialog();
			
		}

		private void treeViewAdv1_NodeMouseClick(object sender, TreeNodeAdvMouseEventArgs e)
		{
			if (treeViewAdv1.SelectedNodes.Count == 1)
			{
			    if (treeViewAdv1.SelectedNode != null)
			    {
			        if (treeViewAdv1.SelectedNode.Tag != null)
			        {
			            var tempNode = (ElasticNode)treeViewAdv1.SelectedNode.Tag;
			            if (tempNode != null && tempNode.IndexStatus != null)
			            {
			                toolStripStatusLabel1.Text = tempNode.IndexStatus.ToString();
			            }
			            else if(treeViewAdv1.SelectedNode.Level==4)
			            {
			                WriteLog("\r\n-----\r\n");
			                WriteLog("\r\n");
			                var type = (treeViewAdv1.SelectedNode.Tag as ElasticNode).Tag as TypeSetting;
			                WriteLog(JsonSerializer.Get(type,true));
			            }
			        }
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

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InitTree(currentCluster, currentElasticSearchInstance);
		}
		NewIndex newIndex = new NewIndex();
		private void newIndexToolStripMenuItem_Click(object sender, EventArgs e)
		{

			
			if(newIndex.ShowDialog()==DialogResult.OK)
			{
				WriteLog("Create New Index: {0},{1},{2}", newIndex.IndexName, newIndex.Shard, newIndex.Replica);
				var result = currentElasticSearchInstance.CreateIndex(newIndex.IndexName,
				                                                           new IndexSetting(newIndex.Shard, newIndex.Replica));
			}
		}
		Search search = new Search();
		private void searchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeViewAdv1.SelectedNodes.Count == 1)
			{
				var tempNode = (ElasticNode) treeViewAdv1.SelectedNode.Tag;
				
				
				if (search.ShowDialog() == DialogResult.OK)
				{
					WriteLog("Search Index: {0},{1},{2},{3},{4}",search.IndexType, search.Query, search.Sort, search.GetFrom, search.GetSize);
					var result = currentElasticSearchInstance.Search(tempNode.IndexName, search.IndexType, search.Query, search.Sort,
					                                                 search.GetFrom, search.GetSize);
					jsonView.LoadJson(result.Response);
					jsonView.ShowDialog();
				}
			}
		}

		private void mappingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (TreeNodeAdv selectedNode in treeViewAdv1.SelectedNodes)
			{
				var elasticNode = (ElasticNode)(selectedNode.Tag);
				var index = elasticNode.IndexName;
				WriteLog("Get Index Mapping: {0}", selectedNode.Tag);
				elasticNode.Nodes.Clear();//NOTE
				var mapNode = new ElasticNode("mapping");
				elasticNode.Nodes.Add(mapNode);

				var mappings= currentElasticSearchInstance.GetMapping(index);
                WriteLog(mappings);
				JObject j = JObject.Parse(mappings);
				var VARIABLE =j[index];
				var typeSetting = JsonSerializer.Get<Dictionary<string, TypeSetting>>(VARIABLE.ToString());
				foreach (var setting in typeSetting)
				{
					var item = new ElasticNode(setting.Key);
					item.Tag = setting.Value;
					mapNode.Nodes.Add(item);
					foreach (var typeSetting1 in setting.Value.FieldSettings)
					{
						item.Nodes.Add(new ElasticNode(typeSetting1.Key));
					}
				}
			}
			
		}

		private void analyzeTestToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tempNode = (ElasticNode)treeViewAdv1.SelectedNode.Tag;
			if (tempNode != null)
			{
				AnalyzeTest analyzeTest = new AnalyzeTest(currentElasticSearchInstance,tempNode.IndexName);
				analyzeTest.ShowDialog();
			}


		}

		private void bulkIndexToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tempNode = (ElasticNode)treeViewAdv1.SelectedNode.Tag;
			if (tempNode != null)
			{
				BulkIndex analyzeTest = new BulkIndex(currentElasticSearchInstance, tempNode.IndexName);
				analyzeTest.ShowDialog();
			}
		}

		private void createTypeMappingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tempNode = (ElasticNode)treeViewAdv1.SelectedNode.Tag;
			if (tempNode != null)
			{
				PostMapping analyzeTest = new PostMapping(currentElasticSearchInstance, tempNode.IndexName,string.Empty);
				analyzeTest.ShowDialog();
			}
		}

        private void modifyShardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tempNode = (ElasticNode)treeViewAdv1.SelectedNode.Tag;
            if (tempNode != null)
            {
                ModifyReplica modifyReplica=new ModifyReplica();
                if(modifyReplica.ShowDialog()==DialogResult.OK)
                {
                    currentElasticSearchInstance.ModifyIndex(tempNode.IndexName, new IndexSetting(5,modifyReplica.Replica));
                    WriteLog("Index:{0} now have {1} replicas".Fill(tempNode.IndexName,modifyReplica.Replica));
                }

            }
        }

        private void forEachUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tempNode = (ElasticNode)treeViewAdv1.SelectedNode.Tag;
            if (tempNode != null)
            {
                var start = DateTime.Now;
                
                var size = 500;
                string type = "bc8c7a80-c2fc-4d62-9de3-ec6b9f59ba95";
                GetInput g=new GetInput(type,"Type");
                if(g.ShowDialog()==DialogResult.OK)
                {
                    type = g.Input;
                }

                string query = "*";
                g = new GetInput(query, "Query");
                if (g.ShowDialog() == DialogResult.OK)
                {
                    query = g.Input;
                }

                var total = currentElasticSearchInstance.Count(tempNode.IndexName, query);

                string sort = "__TIME";
                g = new GetInput(sort, "SortBy");
                if (g.ShowDialog() == DialogResult.OK)
                {
                    sort = g.Input;
                }
                string content = "{\"visibleType\":\"0\"}";

                g = new GetInput(content, "update content");
                if (g.ShowDialog() == DialogResult.OK)
                {
                    content = g.Input;
                }

                int hit = 0;
                for (int i = 0; i < total; i+=size)
                {
                    WriteLog("Index From:{0},Size:{1}".Fill(i,size));
                    
                    SearchResult docs= currentElasticSearchInstance.Search(tempNode.IndexName, type,
                                                        new QueryStringQuery(query), new SortItem(sort, SortType.Asc), i, size);
                    WriteLog("QueryResult:{0} // {1}".Fill(docs.GetHitIds().Count,docs.GetTotalCount()));
                    List<string> ids = docs.GetHitIds();
                    foreach (var id in ids)
                    {
                     var result=   currentElasticSearchInstance.PartialUpdate(tempNode.IndexName, type, id, content);
                        if(result)
                        {
                            hit += 1;
                        }else{WriteLog("Index Failed:{0}".Fill(id));}
                    }
                }
                var end = DateTime.Now;
                WriteLog("Index:{0} Updating Finished.total {1} updated,time:{2},{3} ".Fill(tempNode.IndexName, hit,end-start,hit));

            }
        }

        private void facetToolStripMenuItem_Click(object sender, EventArgs e)
        {

             var tempNode = (ElasticNode)treeViewAdv1.SelectedNode.Tag;
             if (tempNode != null)
             {
                 var g = new GetInput("","pls input a fileld name to do Facet");
                 if (g.ShowDialog() == DialogResult.OK)
                 {

              var facets= currentElasticSearchInstance.Search(tempNode.IndexName,
                                                         new ElasticQuery(new MatchAllQuery(), null, 0, 10)
                                                             {Facets = new TermsFacet("facet", g.Input)});


              tempNode.Nodes.Clear();//NOTE
              var facetNode = new ElasticNode("facets");
              tempNode.Nodes.Add(facetNode);

                     if (facets.Facets != null)
                         foreach (var facet in facets.Facets)
                         {
                             var item = new ElasticNode(facet.Key + "(" + facet.Value.Count + ")");
                             facetNode.Nodes.Add(item);
                             foreach (var VARIABLE in facet.Value)
                             {
                                 item.Nodes.Add(new ElasticNode(VARIABLE.Key + "(" + VARIABLE.Value.ToString() + ")"));
                             }
                         }
                 }
                 }
             }
        }
	
}
