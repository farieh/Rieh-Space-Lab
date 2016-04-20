using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_Lab
{
    class Program
    {
       
        interface IWindow
        {
            int x
            {
                get;set;
            }
            int y
            {
                get;set;
            }

            void Position();

        }

        class Window :IWindow
        {
            
            public int y
            {
                get { return y; }
                set { y = value; }
            }

            public int x
            {
                get;
                set;
            }

            public void Position()
            {
                Console.WriteLine("Posisi window ada di x {0} dan y {1}",x,y);
            }

        }

        static void Main(string[] args)
        {
        }
    }
}
