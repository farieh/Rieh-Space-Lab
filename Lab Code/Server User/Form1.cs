using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_User
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ServiceReference1.DataUser> User = new List<ServiceReference1.DataUser>();
            

            for (int i = 0;i<5;i++)
            {
                ServiceReference1.DataUser teks = new ServiceReference1.DataUser();
                teks.UserName = string.Format("ke {0}", i);
                User.Add(teks);
            }

            listBox1.DataSource = User;
            listBox1.DisplayMember = "UserName";
        }
    }
}
