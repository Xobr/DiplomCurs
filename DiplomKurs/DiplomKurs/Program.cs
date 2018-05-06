using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; 

namespace DiplomKurs
{
    static class Program
    {
        public static List<string> GetStringListFromFile(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var res = new List<string>();
                while (sr.Peek() >= 0)
                    res.Add(sr.ReadLine());
                return res; 
            }
        }

        public static void RevriteSymbols(List<string> ls,string path)
        {
            using (var sw = new StreamWriter(path, false))
            {
                foreach (var item in ls)
                {
                    sw.WriteLine(item); 
                }
            }
        }

        public static double Round(this double number, int countOfCHarakter = 1)
        {
            double intPart = (int)number;
            double doublePart = number - intPart;

            for (int i = 0; i < countOfCHarakter; i++)
            {
                doublePart *= 10;
            }

            doublePart = (int)doublePart;

            for (int i = 0; i < countOfCHarakter; i++)
            {
                doublePart *= 0.1;
            }

            double res = intPart + doublePart;
            return res;

        }

        public static byte GetInt(this bool b)
        {
            if (b) return 1;
            return 0; 
        }

        public static List<TwoIntPair> GetTwoIntPair(this List<int> ls)
        {
            var res = new List<TwoIntPair>();
            for (int i = 0; i < ls.Count; i++)
            {
                res.Add(new TwoIntPair(i,ls[i]));
            }

            return res; 
        }

        public static List<TwoIntPair> GetListOfTwoIntPair(this Dictionary<int, int> dct)
        {
            var res = new List<TwoIntPair>();
            foreach (var item in dct)
            { 
                res.Add(new TwoIntPair(item.Key,item.Value)); 
            }

            res.Sort();

            return res; 
        }

        public static string RemoveStopChars(this string str)
        {
            string res = str;
            foreach (var elem in Config.StopChars)
            {
                res = res.Replace(elem.ToString(),""); 
            }
            return res; 
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
