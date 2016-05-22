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

namespace Lab_Window
{
    public partial class Form1 : Form
    {
        public string ID_Form;
        List<string> nama = new List<string>();
        Dictionary<string, int> mydic = new Dictionary<string, int>();
        public string itemSelected;


        public Form1()
        {
            InitializeComponent();
            
        }


        private void add_Nama(string nm1)
        {
            
            nama.Add(nm1);
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

        int nomor;
        private Baru_Form baru;
        private void lbNama_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            
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
                        isFormFound = false;
                    }


                }
                


                //Application.OpenForms[index].Show();
                //nomor++;


                /*
                Baru_Form baru = new Baru_Form();

                baru.Start_Form(text);
                nomor++;
                */

                //if(baru != null)
                //{

                //}

                //Baru_Form baru = new Baru_Form();
                //baru.Start_Form(lbNama.SelectedItem.ToString());

            }
        }
        private void bNama_Click(object sender, EventArgs e)
        {

            //Open All form active Form_Baru

            for (int i = Application.OpenForms.Count - 1; i >= 0; i += -1)
            {
                if (!object.ReferenceEquals(Application.OpenForms[i], this))
                {

                    var br = Application.OpenForms[i];
                    br.Show();
                }
            }


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
            //Baru_Form br = new Baru_Form();
            //br.Show();

            foreach (Form br in Application.OpenForms.Cast<Form>())
            {
                MessageBox.Show(br.Name);

            }
        }
    }
}
