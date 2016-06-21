using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;

namespace Lab_Window
{
    
    

    public class ServiceBaru : IServiceBaru
    {
        Engine egn = new Engine();

        public string GetData ()
        {
            return string.Format("Barusan reuquest data ya.. ?");
        }

        public string SendData (string value)
        {
            return string.Format("Yoo, enter something like {0} ?", value);
        }

        public byte GetByte (byte value)
        {
            return value;
        }
        public User WhoAmI (User value)
        {
            
            return value;
        }

    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string AddUser (string value)
        {
            
            return value;
        } 
    }
}
