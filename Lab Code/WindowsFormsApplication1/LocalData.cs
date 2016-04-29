using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCodeLearn
{
    public class LocalData
    {
        private string hostname;
        private int indeks;
        private string username;

        public string HostName
        {
            get
            {
                return hostname;
            }

            set
            {
                hostname = value;
            }
        }


        public string UserName
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }


        public int Index
        {
            get
            {
                return indeks;
            }

            set
            {
                indeks = value;
            }
        }

    }
}
