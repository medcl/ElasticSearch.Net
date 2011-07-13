using System;

namespace ElasticSearch.DataManager.Dialogs
{
	public class Search:FormBase
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Label label5;


		private System.Windows.Forms.Label label1;

		public Search():base()
		{
			InitializeComponent();
		}
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.textBox4);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.textBox3);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.textBox2);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.textBox5);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.textBox1);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Size = new System.Drawing.Size(260, 145);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Query:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(54, 31);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(191, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "_type:tweet";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Sort:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(54, 57);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(191, 20);
			this.textBox2.TabIndex = 2;
			this.textBox2.Text = "_type:asc";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 86);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(33, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "From:";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(54, 83);
			this.textBox3.Name = "textBox3";
			this.textBox3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.textBox3.Size = new System.Drawing.Size(191, 20);
			this.textBox3.TabIndex = 3;
			this.textBox3.Text = "0";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 111);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(30, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Size:";
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(54, 108);
			this.textBox4.Name = "textBox4";
			this.textBox4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.textBox4.Size = new System.Drawing.Size(191, 20);
			this.textBox4.TabIndex = 4;
			this.textBox4.Text = "5";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(13, 8);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(34, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Type:";
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(54, 5);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(191, 20);
			this.textBox5.TabIndex = 1;
			// 
			// Search
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(284, 169);
			this.Name = "Search";
			this.Text = "Search";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}


		public string Query { get { return textBox1.Text.Trim(); } }
		public string IndexType { get { return textBox5.Text.Trim(); } }
		public string Sort { get { return textBox2.Text.Trim(); } }
		public int GetFrom { get { return Convert.ToInt32(textBox3.Text.Trim()); } }
		public int GetSize { get { return Convert.ToInt32(textBox4.Text.Trim()); } }
	}
}