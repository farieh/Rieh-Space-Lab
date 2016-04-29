using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfChat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService :IChatService
    {
        private ChatEngine mainEngine=new ChatEngine();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ChatUser ClientConnect(string userName)
        {
            return mainEngine.AddNewChatUser(new ChatUser() { UserName = userName });        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<ChatMessage> GetNewMessages(ChatUser user)
        {
            return mainEngine.GetNewMessages(user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newMessage"></param>
        public void SendNewMessage(ChatMessage newMessage)
        {
            mainEngine.AddNewMessage(newMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ChatUser> GetAllUsers()
        {
            return mainEngine.ConectedUsers;
        }

        public void RemoveUser(ChatUser user)
        {
            mainEngine.RemoveUser(user);
        }

 
    }
}
