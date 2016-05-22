using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_Window
{
    public class FormProvider
    {
        

        private static Baru_Form _startClient;
        public static Baru_Form StartClient
        {
            get
            {
                if (_startClient == null )
                {
                    _startClient = new Baru_Form();
                }
                else
                {
                    _startClient.Show();
                }
                return _startClient;
            }

            
        }


    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
