using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Net;


namespace Lab_Window
{
    [ServiceContract]
    public interface IWcfPingTest
    {
        [OperationContract]
        string Ping();
    }


    [ServiceContract]
    public interface IStreaming
    {
        [OperationContract]
        Stream GetStream(string data);
        [OperationContract]
        bool UploadStream(Stream stream);
        [OperationContract]
        Stream EchoStream(Stream stream);
        [OperationContract]
        Stream GetReversedStream();

    }


    [ServiceContract]
    public interface IServiceBaru
    {
        [OperationContract]
        string GetData();

        [OperationContract]
        string SendData(string value);

        [OperationContract]
        byte GetByte(byte value);

        [OperationContract]
        User WhoAmI(User value);
    }
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        string AddUser(string value);

        // TODO: Add your service operations here
    }
    

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "WCFservice.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    [DataContract]
    public class User
    {
        string name;
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }

        }

        IPAddress ipAddress;
        [DataMember]
        public IPAddress IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
    }


}
