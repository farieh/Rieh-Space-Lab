using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabCodeLearn;

namespace WindowsFormsApplication1
{

    

    public partial class Form1 : Form
    {

        

        public Form1()
        {
            InitializeComponent();
            
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = new System.Windows.Forms.CheckBox();
            checkBox.Appearance = System.Windows.Forms.Appearance.Button;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WcfServiceLibrary7.Service1 client = new WcfServiceLibrary7.Service1();
            string returnString;

            returnString = client.GetData(textBox1.Text);
            label1.Text = returnString;
            string itemTest = "asadsd";
            //List<string> listTest;

            //listTest.Add();
                
             

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        int i;
        
        public void FlowLayoutPanel_DoubleClick (object sender, EventArgs e)
        {
            i++;
            

            UserControl127 userCtrl127 = new UserControl127();
            userCtrl127.indeks = i;
            flowLayoutPanel127.Controls.Add(userCtrl127);
            
            
            
        }

        private void ChangeAllColorStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeAllColor(flowLayoutPanel127);
        }

        private void ChangeAllColor(Control c)
        {
            c.BackColor = Color.White;
            foreach(Control ctrl in c.Controls)
            {
                ChangeAllColor(ctrl);
            }
        }

        private void showNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void betterListView_click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
                //MessageBox.Show(string.Format("{0}",betterListView1.FocusedItem));
            }

        }
    }
}
