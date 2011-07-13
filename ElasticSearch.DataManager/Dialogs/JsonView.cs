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
	public partial class JsonView : FormBase
	{
		private EPocalipse.Json.Viewer.JsonViewer jsonViewer;
		public JsonView():base()
		{
			
			jsonViewer = new JsonViewer();
			jsonViewer.ShowTab(Tabs.Viewer);
			jsonViewer.Dock = DockStyle.Fill;
			panel1.Controls.Add(jsonViewer);
			InitializeComponent();
			
		}
		public void LoadJson(string json)
		{
			jsonViewer.Clear();
			jsonViewer.Json = json;
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// JsonView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.ClientSize = new System.Drawing.Size(284, 165);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "JsonView";
			this.ResumeLayout(false);

		}

	}
}
