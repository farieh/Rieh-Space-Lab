using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabCodeLearn
{
    public partial class UserControl127 : UserControl
    {

        public string hostName;
        public int indeks;


        public UserControl127()
        {

            InitializeComponent();
        }

        private void userControl127_OnLoad(object sender, EventArgs e)
        {
            //var senderAsControl = sender as Control;
            var name = this.TabIndex;

            labelHostName.Text = string.Format("Computer {0}",indeks);
            labelUserName.Text = string.Format("{0}",name);
            
        }
        
    }
}
