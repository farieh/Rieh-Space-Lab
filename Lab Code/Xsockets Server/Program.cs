using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xsockets_Server;
using XSockets.Core.Common.Socket;
using XSockets.Plugin.Framework;

namespace Xsockets_Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var container = Composable.GetExport<IXSocketServerContainer>())
            {

                container.Start();
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Server());
            }
                
        }
    }
}
