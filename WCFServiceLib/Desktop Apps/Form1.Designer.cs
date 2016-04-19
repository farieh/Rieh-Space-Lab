namespace Desktop_Apps
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
            this.tbPesan = new System.Windows.Forms.TextBox();
            this.bSend = new System.Windows.Forms.Button();
            this.rtbTampilan = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // tbPesan
            // 
            this.tbPesan.Location = new System.Drawing.Point(32, 200);
            this.tbPesan.Name = "tbPesan";
            this.tbPesan.Size = new System.Drawing.Size(219, 20);
            this.tbPesan.TabIndex = 0;
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(32, 226);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(75, 23);
            this.bSend.TabIndex = 2;
            this.bSend.Text = "Send";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // rtbTampilan
            // 
            this.rtbTampilan.Location = new System.Drawing.Point(32, 12);
            this.rtbTampilan.Name = "rtbTampilan";
            this.rtbTampilan.Size = new System.Drawing.Size(219, 171);
            this.rtbTampilan.TabIndex = 3;
            this.rtbTampilan.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.rtbTampilan);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.tbPesan);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbPesan;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.RichTextBox rtbTampilan;
    }
}

