using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElasticSearch.Client.Config;

namespace ElasticSearch.DataManager.Dialogs
{
	public partial class Connect : Form
	{
		public Connect()
		{
			InitializeComponent();
			comboBox1.SelectedIndex = 0;
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void textBox2_TextChanged(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		public string Host { get { return textBox1.Text; }}
		public int Port { get { return int.Parse(textBox2.Text); } }
		public TransportType Type { get { return (TransportType)comboBox1.SelectedIndex; } }
	}
}
