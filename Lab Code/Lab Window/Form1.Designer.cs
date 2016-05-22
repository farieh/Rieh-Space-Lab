namespace Lab_Window
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Timer timer1;
            this.lbNama = new System.Windows.Forms.ListBox();
            this.tbNama = new System.Windows.Forms.TextBox();
            this.bNama = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbNama
            // 
            this.lbNama.FormattingEnabled = true;
            this.lbNama.Items.AddRange(new object[] {
            "Aliya",
            "Bubu",
            "Dudi",
            "Eni",
            "Fani",
            "Gina",
            "Hani",
            "Jono"});
            this.lbNama.Location = new System.Drawing.Point(120, 12);
            this.lbNama.Name = "lbNama";
            this.lbNama.Size = new System.Drawing.Size(120, 160);
            this.lbNama.TabIndex = 0;
            this.lbNama.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbNama_MouseDoubleClick);
            // 
            // tbNama
            // 
            this.tbNama.Location = new System.Drawing.Point(120, 178);
            this.tbNama.Name = "tbNama";
            this.tbNama.Size = new System.Drawing.Size(120, 20);
            this.tbNama.TabIndex = 1;
            // 
            // bNama
            // 
            this.bNama.Location = new System.Drawing.Point(165, 204);
            this.bNama.Name = "bNama";
            this.bNama.Size = new System.Drawing.Size(75, 23);
            this.bNama.TabIndex = 2;
            this.bNama.Text = "Add Nama";
            this.bNama.UseVisualStyleBackColor = true;
            this.bNama.Click += new System.EventHandler(this.bNama_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(100, 160);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 204);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(89, 233);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 297);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.bNama);
            this.Controls.Add(this.tbNama);
            this.Controls.Add(this.lbNama);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbNama;
        private System.Windows.Forms.TextBox tbNama;
        private System.Windows.Forms.Button bNama;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

