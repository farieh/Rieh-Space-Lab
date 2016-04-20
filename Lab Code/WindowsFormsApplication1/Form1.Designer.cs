namespace WindowsFormsApplication1
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
            this.checkTalk = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkTalk
            // 
            this.checkTalk.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkTalk.FlatAppearance.BorderSize = 0;
            this.checkTalk.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.checkTalk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkTalk.Image = global::WindowsFormsApplication1.Properties.Resources.Circled_Play_48;
            this.checkTalk.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkTalk.Location = new System.Drawing.Point(12, 12);
            this.checkTalk.Name = "checkTalk";
            this.checkTalk.Size = new System.Drawing.Size(58, 85);
            this.checkTalk.TabIndex = 0;
            this.checkTalk.Text = "check";
            this.checkTalk.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkTalk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.checkTalk.UseVisualStyleBackColor = true;
            this.checkTalk.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 327);
            this.Controls.Add(this.checkTalk);
            this.Name = "Form1";
            this.Text = "Lab Code";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkTalk;
    }
}

