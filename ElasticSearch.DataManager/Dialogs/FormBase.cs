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
	public partial class FormBase : Form
	{
		public FormBase()
		{
			InitializeComponent();
		}

		protected virtual void button1_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		protected virtual void button2_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
