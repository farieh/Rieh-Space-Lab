using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LabSpace
{
    class Program
    {

        interface IDimensions
        {
            int x { get; set; }
            int y { get; set; }
        }

        class Dimensi4
        {
            
        }


        abstract class TaxCalculator
        {

        }

        class SalesTaxCalculator
        {

        }



        internal enum SortCriteria
        {
            ByName,
            ByType,
            BySize,
            ByDate
        }

        

        struct Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;

            }

            public override string ToString()
            {
                return "(" + x + ", " + y + ")";
            }

            

        }


        class Siswa : Student
        {
            // Kelas di dalam kelas program
            internal string nama; //property kelas yang bisa diakses kelas lain
            int usia; //tidak bisa di akses kelas lain
            protected string proteksi;
            private string privasi;
            internal string alamat;

            public void Sort(SortCriteria criteria)
            {
                switch (criteria)
                {
                    case SortCriteria.ByName:

                        Console.WriteLine("Diurutkan berdasar nama");
                        break;
                    case SortCriteria.ByDate:
                        Console.WriteLine("Diurutkan berdasar tanggal");
                        break;

                    case SortCriteria.BySize:
                        Console.WriteLine("Diurutkan berdasar ukuran ");
                        break;
                    case SortCriteria.ByType:
                        Console.WriteLine("Diurutkan berdasar jenis");

                        break;

                }

            }


        }

        class Bentuk
        {
            public virtual void Gambar()
            {
                Console.WriteLine("Gambar Bentuk apa aja..");
            }
        }

        class Kotak : Bentuk
        {
            public override void Gambar()
            {
                Console.WriteLine("Gambar bentuk kotak..");
            }

        }

        class Bulet : Bentuk
        {
            public override void Gambar()
            {
                Console.WriteLine("Gambar bentuk bulet..");
            }
        }

        class Segitiga : Bentuk
        {
            public override void Gambar()
            {
                Console.WriteLine("Gambar bentuk segitiga..");
            }
        }

        static void Main(string[] args)
        {


            double persentase;
            int markMath;
            int markLang;
            int markSci;
            //Student stJhon = new Student();
            //Siswa siswaBaru = new Siswa();
            //Siswa siswaLama = new Siswa();

            //Bentuk[] bentuk2 = { new Kotak(), new Bulet(), new Segitiga() };


            //Kotak rect = (Kotak) bentuk2[0];

            //Console.WriteLine(bentuk2[0] is Segitiga);

            /*Random random = new Random();

            for (int i=0;i<5;i++)
            {
                int randNum = random.Next(0, 3);
                bentuk2[randNum].Gambar();
            }
            */
            //Bentuk Sembarang = new Segitiga();
            //Sembarang.Gambar();


            /*
            stJhon.name = "Jhono Oppo";
            siswaBaru.nama = "Ahmad Suwano";
            stJhon.InputMarks();
            stJhon.FindSum();
            persentase = stJhon.GetPercentage();

            Console.WriteLine("Persentasi nilai {0} adalah {1} %", stJhon.name, persentase);

            //Console.WriteLine("Nilai Math : ");
            //stJhon.MarkInMath = Convert.ToInt16(Console.ReadLine());
            //Console.WriteLine("Nilainya adalah {0}", stJhon.MarkInMath);

            stJhon.DoWork(ref persentase);

            Console.WriteLine("Nilai persentase : {0} ", persentase);
            */
            /*
            MuridKu MuridRajin = new MuridKu();
            MuridRajin.age = 15;
            */


            /*
            struct Point
            {

                public double x;
                public double y;


                public Point(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                
                }
                
                public override string ToString()
                {
                    return "(" + x + ", " + y + ")";
                }
            }
            */

            //Belajar struct 
            /*
            Point pt = new Point();
            Point pt1 = new Point(12,16);
            Point pt2;
            pt2.x = 982;
            pt2.y = 123;


            Console.WriteLine("pt = {0} ", pt);
            Console.WriteLine("pt1 = {0} ", pt1);
            Console.WriteLine("pt2 = {0} ", pt2);

            Console.WriteLine("Pt = {0}", pt1.ToString());*/

            /*
            Siswa SiswaUrut = new Siswa();
            SiswaUrut.Sort(SortCriteria.BySize);
            SiswaUrut.Sort(SortCriteria.ByDate);
            */

            /*
            Console.WriteLine("Masukan Usia : ");
            int usia = Convert.ToInt16(Console.ReadLine());
            string IsCukupUmur;
                        
            IsCukupUmur = (usia >= 17) ? "Selamat, anda sudah cukup umur":"Masih belum cukup umur.";

            Console.WriteLine("{0}", IsCukupUmur);

            */

            /*
            Console.WriteLine("Tulisa nama ?");
            string namaKamu = Console.ReadLine();

            string kasihTahuGakYa = (string.IsNullOrEmpty(namaKamu)) ? "Stringnya null" : "stringnya ada nama Kamu nya";

            Console.WriteLine("{0}", kasihTahuGakYa);
            */

            /*
            object a = null;
            object b = null;
            object c = 9;
            object d = a??b??c;

            Console.WriteLine("a = {0} , b = {1}, c = {2}, d = {3}", a, b, c, d);
            */


            /*
            ArrayList List = new ArrayList();

            List.Add(12);
            List.Add(42);

            foreach(int num in List)
            {
                Console.WriteLine(num);
            }
            */
            /*

            Stack stack = new Stack(12);

            stack.Push(98);
            stack.Push(21);
            while (stack.Count != 0)
            {
                Console.WriteLine(stack.Pop());
            }
            */



            Queue antri = new Queue(12);

            antri.Enqueue(12);
            antri.Enqueue(87);
            antri.Enqueue(76);

            while (antri.Count != 0)
            {
                Console.WriteLine(antri.Dequeue());
            }



            StringBuilder namaku = new StringBuilder("Teh Manis Sekali");

            //namaku.Insert("Juugo sakito");
            Console.WriteLine(namaku);
            







            //keluar dari aplikasi
            Console.WriteLine("Tekan sembarang untuk keluar!");
            Console.ReadKey();
        }
    }

    

    internal class Student
    {


        internal string name;
        public int age;
        private int marksInMath;
        public int MarkInMath
        {
            set
            {
                if (value >=0 && value <=100)
                {
                    marksInMath = value;
                }
                else
                {
                    marksInMath = 0;
                }
            }
            get
            {
                return marksInMath;
            }
        }


        private int marksInLang;
        private int marksInSci;
        private int totalMarks = 300;
        private int obtainedMarks;
        private double percentage;

        internal void InputMarks()
        {
            Console.WriteLine("Mark of Math : ");
            marksInMath = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Mark of Language : ");
            marksInLang = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Mark of Science : ");
            marksInSci = Convert.ToInt16(Console.ReadLine());
        }

        public void  FindSum()
        {
            obtainedMarks = marksInLang +  marksInSci + marksInMath;
            Console.WriteLine("Total nilai nya {0} adalah {1}", name, obtainedMarks);

        }

        
        public double GetPercentage()
        {
            percentage =  obtainedMarks * 100 / totalMarks;
            return percentage;
        }

        internal void DoWork(ref double i)
        {
            i=i+10;
        }



    }

    public class MuridKu
    {

        private int _position = 0;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

    }

}


