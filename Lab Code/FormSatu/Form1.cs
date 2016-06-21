using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.ServiceModel.Channels;

namespace FormSatu
{
    public partial class Form1 : Form
    {
        //private ChannelFactory<IService1> remoteFactory;
        //private IService1 remoteProxy;


        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            //remoteFactory = new ChannelFactory<IService1>("DiscoService");
            //remoteProxy = remoteFactory.CreateChannel();
            //int a = 12;
            //MessageBox.Show(remoteProxy.GetData("asdasd"));
        }
    }
}
