using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_Window
{
    

    public partial class Baru_Form : Form
    {
        public string ID_Form;
        public static Baru_Form Instance;
       
        public Baru_Form()
        {
            InitializeComponent();
            
        }

        public void Start_Form (string user)
        {
            this.Name = user;
            this.Show();
            
            
        }

        private void Baru_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Baru_Form_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Baru_Form_Load(object sender, EventArgs e)
        {
            
            this.Text = ID_Form;
            
        }
    }
}
