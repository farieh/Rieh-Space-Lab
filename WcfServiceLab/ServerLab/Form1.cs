using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WcfServiceLab;
using System.ServiceModel;


namespace ServerLab
{
    public partial class fServer : Form
    {
        public string Name { get; set; }

        private ChannelFactory<IServiceLab> remoteFactory;
        private IServiceLab remoteProxy;
        private ChatUser clientUser;

        public fServer()
        {
            InitializeComponent();

            remoteFactory = new ChannelFactory<IServiceLab>("ChatLab");
            remoteProxy = remoteFactory.CreateChannel();
            
        }

        
        private void bRegister_Click(object sender, EventArgs e)
        {
            Name = tbUser.Text;
            clientUser = remoteProxy.ClientConnect(Name);
            lUser.Text = Name;
            
        }

        private void timerUser_Tick(object sender, EventArgs e)
        {
            List<ChatUser> listUsers = remoteProxy.GetAllUsers();
            if (listUsers == null)
            {
                lbUser.DataSource = "sdasd";
            }
            else
            {
                lbUser.DataSource = listUsers;
            }

            
        }

        private void fServer_Load(object sender, EventArgs e)
        {

        }
    }
}
