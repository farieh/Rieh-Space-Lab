namespace ServerLab
{
    partial class fServer
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
            this.lbUser = new System.Windows.Forms.ListBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.bRegister = new System.Windows.Forms.Button();
            this.lUser = new System.Windows.Forms.Label();
            this.timerUser = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lbUser
            // 
            this.lbUser.FormattingEnabled = true;
            this.lbUser.Location = new System.Drawing.Point(12, 12);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(260, 199);
            this.lbUser.TabIndex = 0;
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(12, 229);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(100, 20);
            this.tbUser.TabIndex = 1;
            // 
            // bRegister
            // 
            this.bRegister.Location = new System.Drawing.Point(187, 226);
            this.bRegister.Name = "bRegister";
            this.bRegister.Size = new System.Drawing.Size(75, 23);
            this.bRegister.TabIndex = 2;
            this.bRegister.Text = "Register";
            this.bRegister.UseVisualStyleBackColor = true;
            this.bRegister.Click += new System.EventHandler(this.bRegister_Click);
            // 
            // lUser
            // 
            this.lUser.AutoSize = true;
            this.lUser.Location = new System.Drawing.Point(12, 213);
            this.lUser.Name = "lUser";
            this.lUser.Size = new System.Drawing.Size(35, 13);
            this.lUser.TabIndex = 3;
            this.lUser.Text = "label1";
            // 
            // timerUser
            // 
            this.timerUser.Enabled = true;
            this.timerUser.Interval = 1000;
            this.timerUser.Tick += new System.EventHandler(this.timerUser_Tick);
            // 
            // fServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.lUser);
            this.Controls.Add(this.bRegister);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.lbUser);
            this.Name = "fServer";
            this.Text = "Server Form";
            this.Load += new System.EventHandler(this.fServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbUser;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Button bRegister;
        private System.Windows.Forms.Label lUser;
        private System.Windows.Forms.Timer timerUser;
    }
}

