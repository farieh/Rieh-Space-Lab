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
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.ServiceModel.Channels;
using Lab_Window;
using System.IO;
using NAudio;



namespace Server_User
{
    public partial class ServerForm : Form
    {
        public ChannelFactory<IServiceBaru> remoteFactory;
        public IServiceBaru remoteProxy;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            remoteFactory = new ChannelFactory<IServiceBaru>("WcfConfig");
            remoteProxy = remoteFactory.CreateChannel();

            var sendData = remoteProxy.SendData(textBox1.Text);
            MessageBox.Show(sendData);

        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }

        private void bRecord_Click(object sender, EventArgs e)
        {
            
        }

        public class WcfPingTest : IWcfPingTest
        {
            public const string magicString = "djeut73bch58sb4"; // this is random, just to see if you get the right result
            public string Ping() { return magicString; }
        }

        public void WcfTestHost_Open()
        {
            string hostname = System.Environment.MachineName;
            var baseAddress = new UriBuilder("net.tcp", hostname, 9876, "WcfPing");
            MessageBox.Show(baseAddress.ToString());
            var h = new ServiceHost(typeof(ServiceBaru), baseAddress.Uri);

            // enable processing of discovery messages.  use UdpDiscoveryEndpoint to enable listening. use EndpointDiscoveryBehavior for fine control.
            h.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            h.AddServiceEndpoint(new UdpDiscoveryEndpoint());

            // enable wsdl, so you can use the service from WcfStorm, or other tools.
            var smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            h.Description.Behaviors.Add(smb);

            // create endpoint
            var binding = new NetTcpBinding();
            h.AddServiceEndpoint(typeof(IServiceBaru), binding, "");
            h.Open();
            MessageBox.Show("Host Open");
        }
    }
}
