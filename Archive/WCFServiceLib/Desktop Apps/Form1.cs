using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Apps
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            WCFServiceReference.Service1Client client = new WCFServiceReference.Service1Client();
            string rtString;

            rtString = client.GetData(tbPesan.Text);
            rtbTampilan.Text = rtString;
        }
    }
}
