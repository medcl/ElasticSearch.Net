using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElasticSearch.Client.Domain;

namespace ElasticSearch.DataManager.Dialogs
{
	public partial class BulkIndex : FormBase
	{
		private Button button3;
		private TextBox textBox1;
		private GroupBox groupBox1;
		private CheckBox checkBox1;
		private CheckBox checkBox4;
		private CheckBox checkBox3;
		private CheckBox checkBox2;
		private Label label2;
		private CheckBox checkBox5;
		private TextBox textBox2;
		private FolderBrowserDialog folderBrowserDialog1;
		private Label label1;
		private Button button4;
		private Client.ElasticSearchClient currentElasticSearchInstance;
	
		public BulkIndex()
		{
			InitializeComponent();
		}

		public BulkIndex(Client.ElasticSearchClient currentElasticSearchInstance, string indexName)
		{
			InitializeComponent();
			this.currentElasticSearchInstance = currentElasticSearchInstance;
			this.textBox2.Text = indexName;
			
		}

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.button4 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(569, 421);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(18, 382);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button4);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.button3);
			this.panel1.Controls.Add(this.textBox2);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Size = new System.Drawing.Size(632, 421);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "ChooseFile:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(93, 15);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(489, 20);
			this.textBox1.TabIndex = 1;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(588, 13);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(41, 23);
			this.button3.TabIndex = 2;
			this.button3.Text = "...";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(40, 28);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(70, 17);
			this.checkBox1.TabIndex = 3;
			this.checkBox1.Text = "FileName";
			this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkBox4);
			this.groupBox1.Controls.Add(this.checkBox3);
			this.groupBox1.Controls.Add(this.checkBox2);
			this.groupBox1.Controls.Add(this.checkBox5);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Location = new System.Drawing.Point(28, 68);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(554, 122);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Property";
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Checked = true;
			this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox4.Location = new System.Drawing.Point(40, 97);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(108, 17);
			this.checkBox4.TabIndex = 3;
			this.checkBox4.Text = "CreateLastModify";
			this.checkBox4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBox4.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox3.Location = new System.Drawing.Point(40, 74);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(103, 17);
			this.checkBox3.TabIndex = 3;
			this.checkBox3.Text = "CreateDateTime";
			this.checkBox3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Checked = true;
			this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox2.Location = new System.Drawing.Point(40, 51);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(62, 17);
			this.checkBox2.TabIndex = 3;
			this.checkBox2.Text = "FileSize";
			this.checkBox2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Checked = true;
			this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox5.Location = new System.Drawing.Point(160, 28);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(79, 17);
			this.checkBox5.TabIndex = 3;
			this.checkBox5.Text = "FileContent";
			this.checkBox5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBox5.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(50, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Index:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(93, 43);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(489, 20);
			this.textBox2.TabIndex = 1;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(507, 210);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 6;
			this.button4.Text = "ReIndex";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// BulkIndex
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(656, 445);
			this.Name = "BulkIndex";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		private void button3_Click(object sender, EventArgs e)
		{
			if(folderBrowserDialog1.ShowDialog()==DialogResult.OK)
			{
				textBox1.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			var files = Directory.GetFiles(textBox1.Text, "*.*", SearchOption.AllDirectories);

			foreach (var file in files)
			{
				using(var strs=File.Open(file,FileMode.Open,FileAccess.Read))
				{
					StreamReader str=new StreamReader(strs,Encoding.Default,true);
					var data= str.ReadToEnd();
					var indexItem = new IndexItem("File", Guid.NewGuid().ToString().Replace("-", ""));
					indexItem.Add("Content",data);
					currentElasticSearchInstance.Index(textBox2.Text.ToLower(), indexItem);
				}
			}
		}
	}
}
