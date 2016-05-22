using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Window
{
    class Engine
    {
        private List<User> connected_user = new List<User>();

        public List<User> connected_User
        {
            get { return connected_user; }
        }
        
        public User add_User (User user)
        {
            var exists =
                from User e in this.connected_User
                where e.Name == user.Name
                select e;

            this.connected_User.Add(user);
            return user;

            

        }

        
    }
}
