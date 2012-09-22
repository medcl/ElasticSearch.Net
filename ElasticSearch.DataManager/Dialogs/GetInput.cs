namespace ElasticSearch.DataManager.Dialogs
{
    public class GetInput : FormBase
    {
        public GetInput(string text,string title)
        {
            InitializeComponent();
            this.Text = title;
            textBox1.Text = text;
        }

        private System.Windows.Forms.TextBox textBox1;

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // GetInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(284, 165);
            this.Name = "GetInput";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        public string Input{get { return textBox1.Text; }}
    }
}