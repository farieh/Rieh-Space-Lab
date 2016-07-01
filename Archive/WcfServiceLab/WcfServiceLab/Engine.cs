using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLab
{
    public class Engine
    {
        private List<ChatUser> connectedUsers = new List<ChatUser>();
        private Dictionary<string, List<ChatMessage>> incomingMessages = new Dictionary<string, List<ChatMessage>>();

        public List<ChatUser> ConnectedUsers
        {
            get { return connectedUsers; }
        }

        public ChatUser AddNewChatUser(ChatUser user)
        {
            var exists =
                from ChatUser e in this.ConnectedUsers
                where e.UserName == user.UserName
                select e;

            if (exists.Count() == 0)
            {
                this.ConnectedUsers.Add(user);
                incomingMessages.Add(user.UserName, new List<ChatMessage>() {
                    new ChatMessage() { User=user, Message="Welcome to WPF Chat", Date=DateTime.Now} });

                return user;
            }
            else
                return null;
        }


        public void AddNewMessage(ChatMessage newMessage)
        {
            foreach (var user in this.ConnectedUsers)
            {
                if (!newMessage.User.UserName.Equals(user.UserName))
                {
                    incomingMessages[user.UserName].Add(newMessage);
                }
            }
        }

        public List<ChatMessage> GetNewMessage(ChatUser user)
        {
            List<ChatMessage> myNewMessages = incomingMessages[user.UserName];

            incomingMessages[user.UserName] = new List<ChatMessage>();

            if (myNewMessages.Count > 0)
                return myNewMessages;
            else
                return null;

        }

        public void RemoveUser(ChatUser user)
        {
            this.ConnectedUsers.RemoveAll(u => u.UserName == user.UserName);
        }
    }
}
