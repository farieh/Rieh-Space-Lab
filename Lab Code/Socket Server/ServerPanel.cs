using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace Socket_Server
{
    public partial class ServerPanel : Form
    {
        TcpListener server;
        TcpClient client;
        NetworkStream ns;
        Thread thread;
        string clientIPAddress;

        public ServerPanel()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            byte[] dataKirim = new byte[1024];

            server = new TcpListener(int.Parse(txtPort.Text));
            server.Start();

            client = server.AcceptTcpClient();
            ns = client.GetStream();
            clientIPAddress = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString())+"";
            richTextBox1.Text += clientIPAddress + "=&gt; Connectedn";
            dataKirim = Encoding.ASCII.GetBytes("Hy , Kamu telah terkoneksi ke server ini ");
            ns.Write(dataKirim, 0, dataKirim.Length);
             

        }

        private void bacaData()
        {
            while (true)
            {
                byte[] dataTerima = new byte[1024];
                if (ns.Read(dataTerima, 0, dataTerima.Length) != 0)
                {
                    
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }
    }
}
