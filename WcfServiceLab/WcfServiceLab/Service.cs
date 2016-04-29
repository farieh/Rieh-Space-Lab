using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLab
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceLab : IServiceLab
    {
        private Engine mainEngine = new Engine();

        public ChatUser ClientConnect(string userName)
        {
            return mainEngine.AddNewChatUser(new ChatUser() { UserName = userName });
        }

        public List<ChatMessage> GetNewMessages(ChatUser user)
        {
            return mainEngine.GetNewMessage(user);
        }

        public void SendMessages(ChatMessage newMessage)
        {
            mainEngine.AddNewMessage(newMessage);
        }

        public List<ChatUser> GetAllUsers()
        {
            return mainEngine.ConnectedUsers;
        }

        public void RemoveUser(ChatUser user)
        {
            mainEngine.RemoveUser(user);
        }
    }
}
