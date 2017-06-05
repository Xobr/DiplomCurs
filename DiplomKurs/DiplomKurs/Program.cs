using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiplomKurs
{
    static class Program
    {

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
