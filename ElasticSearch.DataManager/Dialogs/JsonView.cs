using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EPocalipse.Json.Viewer;

namespace ElasticSearch.DataManager.Dialogs
{
	public partial class JsonView : Form
	{
		private EPocalipse.Json.Viewer.JsonViewer jsonViewer;
		public JsonView()
		{
			InitializeComponent();
			jsonViewer = new JsonViewer();
			jsonViewer.ShowTab(Tabs.Viewer);
			jsonViewer.Dock = DockStyle.Fill;
			this.Controls.Add(jsonViewer);
			
		}
		public void LoadJson(string json)
		{
			jsonViewer.Clear();
			jsonViewer.Json = json;
		}

	}
}
