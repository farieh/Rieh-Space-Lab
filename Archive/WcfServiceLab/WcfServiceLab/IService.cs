using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLab
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceLab
    {
        [OperationContract]
        ChatUser ClientConnect(string userName);

        [OperationContract]
        List<ChatMessage> GetNewMessages(ChatUser user);

        [OperationContract]
        void SendMessages(ChatMessage newMessage);

        [OperationContract]
        List<ChatUser> GetAllUsers();

        [OperationContract]
        void RemoveUser(ChatUser user);

    }

    [DataContract]
    public class ChatUser
    {
        private string userName, ipAddress, hostName;
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [DataMember]
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        [DataMember]
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    

    [DataContract]
    public class ChatMessage
    {
        private ChatUser user;
        [DataMember]
        public ChatUser User
        {
            get { return user; }
            set { user = value; }
        }

        private string message;
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private DateTime date;
        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

    }



}
