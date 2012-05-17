using System;

namespace ElasticSearch.DataManager.Dialogs
{
    public class ModifyShard:FormBase
    {
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        public int Replica { get { return Convert.ToInt32(textBox1.Text.ToString()); } }


        public ModifyShard()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(180, 60);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 21);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Size = new System.Drawing.Size(243, 60);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shard:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(62, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(140, 20);
            this.textBox1.TabIndex = 1;
            // 
            // ModifyShard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(267, 84);
            this.Name = "ModifyShard";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
    
        
    }

    
}