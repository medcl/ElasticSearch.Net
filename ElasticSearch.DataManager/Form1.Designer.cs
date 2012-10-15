using System.Drawing;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace ElasticSearchDataManager
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewAdv1 = new Aga.Controls.Tree.TreeViewAdv();
            this.treeColumn1 = new Aga.Controls.Tree.TreeColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewTop5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.newIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.analyzeTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bulkIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTypeMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.modifyShardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.forEachUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeTextBox1 = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.facetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.treeViewAdv1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(790, 417);
            this.splitContainer1.SplitterDistance = 306;
            this.splitContainer1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 412);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(306, 5);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 0);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.connectToolStripMenuItem,
            this.aDDToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(306, 27);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(64, 23);
            this.connectToolStripMenuItem.Text = "&Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // aDDToolStripMenuItem
            // 
            this.aDDToolStripMenuItem.Name = "aDDToolStripMenuItem";
            this.aDDToolStripMenuItem.Size = new System.Drawing.Size(109, 23);
            this.aDDToolStripMenuItem.Text = "C&ustom Connect";
            this.aDDToolStripMenuItem.Click += new System.EventHandler(this.aDDToolStripMenuItem_Click);
            // 
            // treeViewAdv1
            // 
            this.treeViewAdv1.AllowColumnReorder = true;
            this.treeViewAdv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewAdv1.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewAdv1.Columns.Add(this.treeColumn1);
            this.treeViewAdv1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewAdv1.DefaultToolTipProvider = null;
            this.treeViewAdv1.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv1.FullRowSelect = true;
            this.treeViewAdv1.GridLineStyle = Aga.Controls.Tree.GridLineStyle.Horizontal;
            this.treeViewAdv1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewAdv1.Location = new System.Drawing.Point(0, 30);
            this.treeViewAdv1.Model = null;
            this.treeViewAdv1.Name = "treeViewAdv1";
            this.treeViewAdv1.NodeControls.Add(this.nodeTextBox1);
            this.treeViewAdv1.SelectedNode = null;
            this.treeViewAdv1.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.treeViewAdv1.Size = new System.Drawing.Size(306, 387);
            this.treeViewAdv1.TabIndex = 0;
            this.treeViewAdv1.Text = "treeViewAdv1";
            this.treeViewAdv1.NodeMouseClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.treeViewAdv1_NodeMouseClick);
            // 
            // treeColumn1
            // 
            this.treeColumn1.Header = "Index";
            this.treeColumn1.Sortable = true;
            this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.treeColumn1.TooltipText = null;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出数据ToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem1,
            this.viewTop5ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.refreshToolStripMenuItem,
            this.toolStripMenuItem3,
            this.newIndexToolStripMenuItem,
            this.toolStripMenuItem4,
            this.searchToolStripMenuItem,
            this.mappingToolStripMenuItem,
            this.toolStripMenuItem5,
            this.analyzeTestToolStripMenuItem,
            this.bulkIndexToolStripMenuItem,
            this.createTypeMappingToolStripMenuItem,
            this.toolStripMenuItem6,
            this.modifyShardToolStripMenuItem,
            this.toolStripMenuItem7,
            this.forEachUpdateToolStripMenuItem,
            this.toolStripMenuItem8,
            this.facetToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 360);
            // 
            // 导出数据ToolStripMenuItem
            // 
            this.导出数据ToolStripMenuItem.Name = "导出数据ToolStripMenuItem";
            this.导出数据ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.导出数据ToolStripMenuItem.Text = "&Migrate";
            this.导出数据ToolStripMenuItem.Click += new System.EventHandler(this.ExportDataToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(179, 6);
            // 
            // viewTop5ToolStripMenuItem
            // 
            this.viewTop5ToolStripMenuItem.Name = "viewTop5ToolStripMenuItem";
            this.viewTop5ToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.viewTop5ToolStripMenuItem.Text = "&ViewTop5";
            this.viewTop5ToolStripMenuItem.Click += new System.EventHandler(this.viewTop5ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(179, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.refreshToolStripMenuItem.Text = "&Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(179, 6);
            // 
            // newIndexToolStripMenuItem
            // 
            this.newIndexToolStripMenuItem.Name = "newIndexToolStripMenuItem";
            this.newIndexToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.newIndexToolStripMenuItem.Text = "&NewIndex";
            this.newIndexToolStripMenuItem.Click += new System.EventHandler(this.newIndexToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(179, 6);
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // mappingToolStripMenuItem
            // 
            this.mappingToolStripMenuItem.Name = "mappingToolStripMenuItem";
            this.mappingToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.mappingToolStripMenuItem.Text = "Mapping";
            this.mappingToolStripMenuItem.Click += new System.EventHandler(this.mappingToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(179, 6);
            // 
            // analyzeTestToolStripMenuItem
            // 
            this.analyzeTestToolStripMenuItem.Name = "analyzeTestToolStripMenuItem";
            this.analyzeTestToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.analyzeTestToolStripMenuItem.Text = "AnalyzeTest";
            this.analyzeTestToolStripMenuItem.Click += new System.EventHandler(this.analyzeTestToolStripMenuItem_Click);
            // 
            // bulkIndexToolStripMenuItem
            // 
            this.bulkIndexToolStripMenuItem.Name = "bulkIndexToolStripMenuItem";
            this.bulkIndexToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.bulkIndexToolStripMenuItem.Text = "BulkIndex";
            this.bulkIndexToolStripMenuItem.Click += new System.EventHandler(this.bulkIndexToolStripMenuItem_Click);
            // 
            // createTypeMappingToolStripMenuItem
            // 
            this.createTypeMappingToolStripMenuItem.Name = "createTypeMappingToolStripMenuItem";
            this.createTypeMappingToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.createTypeMappingToolStripMenuItem.Text = "CreateTypeMapping";
            this.createTypeMappingToolStripMenuItem.Click += new System.EventHandler(this.createTypeMappingToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(179, 6);
            // 
            // modifyShardToolStripMenuItem
            // 
            this.modifyShardToolStripMenuItem.Name = "modifyShardToolStripMenuItem";
            this.modifyShardToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.modifyShardToolStripMenuItem.Text = "ModifyReplica";
            this.modifyShardToolStripMenuItem.Click += new System.EventHandler(this.modifyShardToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(179, 6);
            // 
            // forEachUpdateToolStripMenuItem
            // 
            this.forEachUpdateToolStripMenuItem.Name = "forEachUpdateToolStripMenuItem";
            this.forEachUpdateToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.forEachUpdateToolStripMenuItem.Text = "ForEach-Update";
            this.forEachUpdateToolStripMenuItem.Click += new System.EventHandler(this.forEachUpdateToolStripMenuItem_Click);
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.DataPropertyName = "Text";
            this.nodeTextBox1.IncrementalSearchEnabled = true;
            this.nodeTextBox1.LeftMargin = 3;
            this.nodeTextBox1.ParentColumn = null;
            // 
            // richTextBox1
            // 
            this.richTextBox1.ContextMenuStrip = this.contextMenuStrip2;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(480, 417);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(102, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.clearToolStripMenuItem.Text = "&Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(179, 6);
            // 
            // facetToolStripMenuItem
            // 
            this.facetToolStripMenuItem.Name = "facetToolStripMenuItem";
            this.facetToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.facetToolStripMenuItem.Text = "Facet";
            this.facetToolStripMenuItem.Click += new System.EventHandler(this.facetToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 417);
            this.Controls.Add(this.splitContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "ElasticSearch DataManager";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
		private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
		private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
		Aga.Controls.Tree.TreeViewAdv treeViewAdv1;
		private TreeColumn treeColumn1;
		private NodeTextBox nodeTextBox1;
		private ToolStripMenuItem deleteToolStripMenuItem;
		private ToolStripMenuItem aDDToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem viewTop5ToolStripMenuItem;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem refreshToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem newIndexToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripMenuItem searchToolStripMenuItem;
		private ToolStripMenuItem mappingToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem5;
		private ToolStripMenuItem analyzeTestToolStripMenuItem;
		private ToolStripMenuItem bulkIndexToolStripMenuItem;
		private ToolStripMenuItem createTypeMappingToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem modifyShardToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem forEachUpdateToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem facetToolStripMenuItem;
	}
}

