using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EPocalipse.Json.Viewer;
using ElasticSearch.Client;

namespace ElasticSearch.DataManager.Dialogs
{
	public partial class PostMapping : FormBase
	{
		private TextBox textBox1;
		private Panel panel2;
		private Label label1;
		private TextBox textBox2;
		private Label label2;
		private JsonViewer jsonViewer;
		private Button button3;
		private ElasticSearchClient currentElasticSearchInstance;
		public PostMapping(ElasticSearchClient currentElasticSearchInstance, string index, string type)
		{
			InitializeComponent();
		    jsonViewer = new JsonViewer();
			jsonViewer.ShowTab(Tabs.Viewer);
			jsonViewer.Dock = DockStyle.Fill;
			panel2.Controls.Add(jsonViewer);

			textBox1.Text = index;
			textBox2.Text = type;
			this.currentElasticSearchInstance = currentElasticSearchInstance;
		}

		protected override void button1_Click(object sender, EventArgs e)
		{
			currentElasticSearchInstance.PutMapping(textBox1.Text.Trim().ToLower(), textBox2.Text.Trim(), jsonViewer.Json);

			base.button1_Click(sender,e);
		}

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(423, 312);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(18, 273);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button3);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.textBox2);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Size = new System.Drawing.Size(486, 312);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "IndexName:";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(94, 7);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(362, 20);
			this.textBox1.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Location = new System.Drawing.Point(29, 60);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(427, 232);
			this.panel2.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(26, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "TypeName:";
			// 
			// textBox2
			// 
			this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox2.Location = new System.Drawing.Point(94, 32);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(362, 20);
			this.textBox2.TabIndex = 1;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3.Location = new System.Drawing.Point(3, 286);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "GetSample";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// PostMapping
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(510, 336);
			this.Name = "PostMapping";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		private void button3_Click(object sender, EventArgs e)
		{
			textBox2.Text = "Type";
			jsonViewer.Json = "{    \"Type\": {\"_source\": {\"compress\": true},\"properties\": {\"Filed\": {\"type\": \"string\",\"store\": \"no\",\"term_vector\": \"with_positions_offsets\",\"analyzer\": \"recruit_fulltext_analyzer\",\"include_in_all\": \"false\",\"boost\": 8}}}}";
		}
	}
}
