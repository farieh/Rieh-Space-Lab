using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.ServiceModel;
using System.Media;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectSound;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Globalization;
using System.Diagnostics;
using Lab_Window;



namespace Lab_Window
{
    
    public partial class Form1 : Form
    {
        public ChannelFactory<IServiceBaru> remoteFactory;
        public IServiceBaru remoteProxy;
        public string ID_Form;
        List<string> nama = new List<string>();
        Dictionary<string, int> mydic = new Dictionary<string, int>();
        public string itemSelected;
        

        

        public void EndPointList()
        {

            /*
            var address = findResult.Endpoints.First(ep => ep.Address.Uri.Scheme == "tcp.net").Address;

            rtbMain.AppendText(address.ToString());
            var factory = new ChannelFactory<IServiceBaru>(new NetTcpBinding(), address);
            var proxy = factory.CreateChannel();


            rtbMain.AppendText(proxy.SendData("Baru Send aja"));

            ((ICommunicationObject)proxy).Close();
            */
        }

        public Form1()
        {
            InitializeComponent();
            
        }


        private void add_Nama(string nm1)
        {
            nama.Add(nm1);
        }

        private void test_Listener ()
        {
            
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        public bool isFormFound;

        //int nomor;
        private Baru_Form baru;
        private void lbNama_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            /*
            itemSelected = lbNama.SelectedItem.ToString();
            Form fc = Application.OpenForms["Baru_Form"];
            
            int idx = this.lbNama.IndexFromPoint(e.Location);
            if (idx != System.Windows.Forms.ListBox.NoMatches)
            {
                string text = string.Format(itemSelected + nomor.ToString());

                isFormFound = false;
                
                if (baru == null)
                {
                    baru = new Baru_Form();
                    baru.Start_Form(itemSelected);
                }
                else
                {
                    
                    foreach(Form br in Application.OpenForms.Cast<Form>())
                    {
                        
                        if(itemSelected == br.Name)
                        {
                            br.Show();
                            isFormFound = true;
                        }

                    }
                    
                    if(!isFormFound)
                    {
                        baru = new Baru_Form();
                        baru.Start_Form(itemSelected);
                    }


                }
            }*/

        }
        private void bNama_Click(object sender, EventArgs e)
        {
            clientDiscovery();

            //Open All form active Form_Baru


            //add_Nama(tbNama.Text);
            //nama.Add("asdas");
            //nama.Add("asdqw");
            //nama.Add("weqds");

            //tbNama.Clear();
            //lbNama.DataSource = nama;

            //MessageBox.Show(nama.Count.ToString());


            /*
            mydic.Add("First", 1);
            mydic.Add("Second", 2);
            mydic.Add("Third", 3);
            mydic.Add("Fourth", 4);

            var result = mydic["Fourth"].ToString();
            */

        }
        private void button1_Click(object sender, EventArgs e)
        {
            int nm = Application.OpenForms.OfType<Baru_Form>().Count();

            MessageBox.Show(nm.ToString());
            

            /*
            // Create an instance of a form and assign it the currently active form.
            Form currentForm = Baru_Form.ActiveForm;

            // Loop through all the controls on the active form.
            for (int i = 0; i < currentForm.Controls.Count; i++)
            {
                // Disable each control in the active form's control collection.
                currentForm.Controls[i].Show();
            }

            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            remoteFactory = new ChannelFactory<IServiceBaru>("WcfConfig");
            remoteProxy = remoteFactory.CreateChannel();

            MessageBox.Show(remoteProxy.SendData(tbNama.Text));

        }

        private void tbNama_TextChanged(object sender, EventArgs e)
        {

        }

        private IWcfPingTest channel;
        public Uri WcfTestClient_DiscoverChannel()
        {
            var dc = new DiscoveryClient(new UdpDiscoveryEndpoint());
            FindCriteria fc = new FindCriteria(typeof(IWcfPingTest));
            fc.Duration = TimeSpan.FromSeconds(5);
            FindResponse fr = dc.Find(fc);
            foreach (EndpointDiscoveryMetadata edm in fr.Endpoints)
            {
                Console.WriteLine("uri found = " + edm.Address.Uri.ToString());
            }
            // here is the really nasty part
            // i am just returning the first channel, but it may not work.
            // you have to do some logic to decide which uri to use from the discovered uris
            // for example, you may discover "127.0.0.1", but that one is obviously useless.
            // also, catch exceptions when no endpoints are found and try again.
            return fr.Endpoints[0].Address.Uri;
        }

        public void WcfTestClient_SetupChannel()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            var factory = new ChannelFactory<IWcfPingTest>(binding);
            var uri = WcfTestClient_DiscoverChannel();
            Console.WriteLine("creating channel to " + uri.ToString());
            EndpointAddress ea = new EndpointAddress(uri);
            channel = factory.CreateChannel(ea);
            Console.WriteLine("channel created");
            //Console.WriteLine("pinging host");
            //string result = channel.Ping();
            //Console.WriteLine("ping result = " + result);
        }
        public void WcfTestClient_Ping()
        {
            Console.WriteLine("pinging host");
            string result = channel.Ping();
            Console.WriteLine("ping result = " + result);
        }


        public class WebServiceEndpointBehavior : IEndpointBehavior
        {
            public void Validate(ServiceEndpoint endpoint)
            {

            }

            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {

            }

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            {

            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                Uri serviceAddress = new Uri("http://sometempuri.org ");   //dynamically load URL here
                endpoint.Address = new EndpointAddress(serviceAddress);
            }
        }

        public void Testing()
        {

            

            remoteFactory = new ChannelFactory<IServiceBaru>("WcfConfig");
            remoteProxy = remoteFactory.CreateChannel();
            

            string str = remoteProxy.SendData("asdasdasd");


            MessageBox.Show(str);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public void clientDiscovery()
        {
            string hostName = Dns.GetHostName();
            User userclient = new User();
            userclient.Name = tbNama.Text;
            foreach (var ipadd in Dns.GetHostAddresses(hostName))
            {
                if(ipadd.AddressFamily.ToString() == "InterNetwork")
                {
                    userclient.IpAddress = ipadd.MapToIPv4();
                    
                }
            }    
            

            var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            var findCriteria = FindCriteria.CreateMetadataExchangeEndpointCriteria(typeof(IServiceBaru));
            findCriteria.MaxResults = 1;

            Debug.Assert(findCriteria.Scopes != null, "findCriteria.Scopes != null");
            findCriteria.Scopes.Add(new Uri("ldap:///ou=people,o=rashim"));
            var findResponse = discoveryClient.Find(findCriteria);

            if (findResponse != null)
            {
                if (findResponse.Endpoints != null)
                {
                    if (findResponse.Endpoints.Count > 0)
                    {
                        var endpoints = MetadataResolver.Resolve(typeof(IServiceBaru),
                                                                findResponse.Endpoints[0].Address);
                        var factory = new ChannelFactory<IServiceBaru>(endpoints[0].Binding, endpoints[0].Address);
                        var channel = factory.CreateChannel();

                        var iamclient = channel.WhoAmI(userclient);

                        MessageBox.Show(iamclient.Name);
                        MessageBox.Show(iamclient.IpAddress.ToString());
                        


                    }
                }
            }

        }
    }
}
