using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ElasticSearch.DataManager.Dialogs
{
	public partial class Export : Form
	{
		public bool ShowLog{get { return checkBox3.Checked; }}

		public Export()
		{
			InitializeComponent();
		}
		public string IndexName { get { return textBox1.Text; } }
		/// <summary>
		/// TotalDocLimit
		/// </summary>
		public int LimitSize { get { return int.Parse(textBox2.Text); } }
		/// <summary>
		/// For Search
		/// </summary>
		public int BufferSize { get { return int.Parse(textBox3.Text); } }
		/// <summary>
		/// BulkSizeLimit
		/// </summary>
		public int BulkSize { get { return int.Parse(textBox4.Text); } }
        public int SkipCount { get { return int.Parse(textBox5.Text); } }

		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
		public bool ComplicatedSource{get { return checkBox1.Checked; }}
		public bool ResolveTenant{get { return checkBox2.Checked; }}
	}
}
