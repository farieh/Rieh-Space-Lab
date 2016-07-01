using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public string UserName { get; set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUserName.Text))
            {
                this.UserName = txtUserName.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please insert a username", "Error validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.UserName = String.Empty;
            this.Close();
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnOK_Click(sender, e);
            }


        }
    }
}
