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
            this.components = new System.ComponentModel.Container();
            ComponentOwl.BetterListView.BetterListViewItem betterListViewItem1;
            ComponentOwl.BetterListView.BetterListViewItem betterListViewItem2;
            ComponentOwl.BetterListView.BetterListViewItem betterListViewItem3;
            ComponentOwl.BetterListView.BetterListViewItem betterListViewItem4;
            ComponentOwl.BetterListView.BetterListViewItem betterListViewItem5;
            ComponentOwl.BetterListView.BetterListViewItem betterListViewItem6;
            this.betterListViewGroup2 = new ComponentOwl.BetterListView.BetterListViewGroup();
            this.betterListViewGroup1 = new ComponentOwl.BetterListView.BetterListViewGroup();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flowLayoutPanel127 = new System.Windows.Forms.FlowLayoutPanel();
            this.betterListView1 = new ComponentOwl.BetterListView.BetterListView();
            betterListViewItem1 = new ComponentOwl.BetterListView.BetterListViewItem();
            betterListViewItem2 = new ComponentOwl.BetterListView.BetterListViewItem();
            betterListViewItem3 = new ComponentOwl.BetterListView.BetterListViewItem();
            betterListViewItem4 = new ComponentOwl.BetterListView.BetterListViewItem();
            betterListViewItem5 = new ComponentOwl.BetterListView.BetterListViewItem();
            betterListViewItem6 = new ComponentOwl.BetterListView.BetterListViewItem();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.betterListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // betterListViewItem1
            // 
            betterListViewItem1.Group = this.betterListViewGroup2;
            betterListViewItem1.Name = "betterListViewItem1";
            betterListViewItem1.Text = "betterListViewItem1";
            // 
            // betterListViewGroup2
            // 
            this.betterListViewGroup2.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.betterListViewGroup2.Header = "betterListViewGroup2";
            this.betterListViewGroup2.Name = "betterListViewGroup2";
            // 
            // betterListViewItem2
            // 
            betterListViewItem2.Group = this.betterListViewGroup1;
            betterListViewItem2.Name = "betterListViewItem2";
            betterListViewItem2.Text = "betterListViewItem2";
            // 
            // betterListViewGroup1
            // 
            this.betterListViewGroup1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.betterListViewGroup1.Header = "betterListViewGroup1";
            this.betterListViewGroup1.Name = "betterListViewGroup1";
            // 
            // betterListViewItem3
            // 
            betterListViewItem3.Name = "betterListViewItem3";
            betterListViewItem3.Text = "betterListViewItem3";
            // 
            // betterListViewItem4
            // 
            betterListViewItem4.Group = this.betterListViewGroup2;
            betterListViewItem4.Name = "betterListViewItem4";
            betterListViewItem4.Text = "betterListViewItem4";
            // 
            // betterListViewItem5
            // 
            betterListViewItem5.Group = this.betterListViewGroup1;
            betterListViewItem5.Name = "betterListViewItem5";
            betterListViewItem5.Text = "betterListViewItem5";
            // 
            // betterListViewItem6
            // 
            betterListViewItem6.Name = "betterListViewItem6";
            betterListViewItem6.Text = "betterListViewItem6";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 100);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 126);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(100, 69);
            this.listBox1.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAllToolStripMenuItem,
            this.showNameToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 48);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deleteAllToolStripMenuItem.Text = "Change All Color";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.ChangeAllColorStripMenuItem_Click);
            // 
            // showNameToolStripMenuItem
            // 
            this.showNameToolStripMenuItem.Name = "showNameToolStripMenuItem";
            this.showNameToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.showNameToolStripMenuItem.Text = "Show Name";
            this.showNameToolStripMenuItem.Click += new System.EventHandler(this.showNameToolStripMenuItem_Click);
            // 
            // flowLayoutPanel127
            // 
            this.flowLayoutPanel127.AutoScroll = true;
            this.flowLayoutPanel127.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel127.ContextMenuStrip = this.contextMenuStrip1;
            this.flowLayoutPanel127.Location = new System.Drawing.Point(118, 12);
            this.flowLayoutPanel127.Name = "flowLayoutPanel127";
            this.flowLayoutPanel127.Size = new System.Drawing.Size(413, 137);
            this.flowLayoutPanel127.TabIndex = 4;
            this.flowLayoutPanel127.DoubleClick += new System.EventHandler(this.FlowLayoutPanel_DoubleClick);
            // 
            // betterListView1
            // 
            this.betterListView1.Groups.Add(this.betterListViewGroup1);
            this.betterListView1.Groups.Add(this.betterListViewGroup2);
            this.betterListView1.Items.Add(betterListViewItem2);
            this.betterListView1.Items.Add(betterListViewItem5);
            this.betterListView1.Items.Add(betterListViewItem1);
            this.betterListView1.Items.Add(betterListViewItem4);
            this.betterListView1.Items.Add(betterListViewItem3);
            this.betterListView1.Items.Add(betterListViewItem6);
            this.betterListView1.Location = new System.Drawing.Point(15, 155);
            this.betterListView1.Name = "betterListView1";
            this.betterListView1.Size = new System.Drawing.Size(516, 256);
            this.betterListView1.TabIndex = 5;
            this.betterListView1.TileSize = new System.Drawing.Size(150, 48);
            this.betterListView1.View = ComponentOwl.BetterListView.BetterListViewView.Tile;
            this.betterListView1.VScrollBarDisplayMode = ComponentOwl.BetterListView.BetterListViewScrollBarDisplayMode.ShowAlways;
            this.betterListView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.betterListView_click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 423);
            this.Controls.Add(this.betterListView1);
            this.Controls.Add(this.flowLayoutPanel127);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Lab Code";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.betterListView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel127;
        private ComponentOwl.BetterListView.BetterListView betterListView1;
        private ComponentOwl.BetterListView.BetterListViewGroup betterListViewGroup1;
        private ComponentOwl.BetterListView.BetterListViewGroup betterListViewGroup2;
        private System.Windows.Forms.ToolStripMenuItem showNameToolStripMenuItem;
    }
}

