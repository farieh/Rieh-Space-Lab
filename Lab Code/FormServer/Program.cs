using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.ServiceModel.Channels;
using SuperSocket;
using SuperSocket.Common;
using SuperSocket.Facility;
using SuperSocket.SocketBase;


namespace FormServer
{
    static class  Program
    {


        public class StringRequestInfo
        {
            public string Key { get; }

            public string Body { get; }

            public string[] Parameters { get; }

            /*
            Other properties and methods
            */
        }
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
