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
	public partial class NewIndex : Form
	{
		public NewIndex()
		{
			InitializeComponent();
		}

		public string IndexName{get { return textBox1.Text.Trim(); }}
		public int Shard{get { return Convert.ToInt32(maskedTextBox1.Text); }}
		public int Replica{get { return Convert.ToInt32(maskedTextBox2.Text); }}

		void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
