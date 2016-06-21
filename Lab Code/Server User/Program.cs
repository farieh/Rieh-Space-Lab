using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;
using System.ServiceModel.Activation;
using Lab_Window;
using System.Net;

namespace Server_User
{
    static class Program
    {
        public class MyCustomHost : ServiceHostFactoryBase
        {
            public override System.ServiceModel.ServiceHostBase
                           CreateServiceHost(string constructorString,
                           Uri[] baseAddresses)
            {
                if (String.IsNullOrEmpty(constructorString))
                    throw new ApplicationException("...");

                // Value of attribute 'Service' in SVC file. 
                string myConfig = constructorString;

                ServiceHost serviceBusHost =
                new ServiceHost(typeof(ServiceBaru), baseAddresses);

                return serviceBusHost;
            }
        }


        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            
            using (ServiceHost host = new ServiceHost(typeof(ServiceBaru)))
            {
                
                host.Open();
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ServerForm());
            }

                
        }
        



    

    }

}
