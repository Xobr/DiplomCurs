using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomKurs
{
    class Imovirnist
    {
        public int[] n;
        public double[] p;
        public double[] bigP; 
        //double[] P;
        public int count; 

        public Imovirnist(int[] list,int countOfwords)
        {
            count = countOfwords;
            n = list;
            p = fillp(list.Length);
            bigP = fillBigP(list.Length); 
        }

        private double[] fillp(int lenght)
        {
            double[] output = new double[count];
            for (int i = 0; i < lenght; i++)
            {
                output[i] =(double)n[i]/(double)count ;//Math.Pow(n[i], -1);
            }
            return output;
        }

        private double[] fillBigP(int lenght)
        {
            double[] output = new double[count];
            output[0] = 1; 
            for (int i = 1; i < lenght; i++)
            {
                output[i] = output[i - 1] - p[i]; 
            }
            return output; 
        }

        public static List<TwoIntPair> GetLitelP(List<bool> ls)
        {
            List<int> dti = Get_dti(ls);

            Dictionary<int, int> dct = new Dictionary<int, int>();

            foreach (var item in dti)
            {
                if (dct.ContainsKey(item))
                    dct[item]++;
                else dct.Add(item,1); 
            }

            var res = dct.GetListOfTwoIntPair(); 

            return res; 
        }

        public static List<Tuple<int, double>> GetLitelPWithDouble(List<TwoIntPair> ls)
        {
            var res = new List<Tuple<int, double>>(); 
            var sum = GetSum(ls);
            for (int i = 0; i < ls.Count; i++)
            {
                Tuple<int, double> current = new Tuple<int, double>(ls[i].First, (double)ls[i].Second / sum);
                res.Add(current); 
            }

            return res; 
        }

        public static int GetSum(List<TwoIntPair> ls)
        {
            int sum = 0;
            foreach (var item in ls) sum += item.Second;

            return sum; 
        }

        public static List<double> GetBigPForNewWords(List<bool> ls)
        {
            var res = new List<double>();
            int sum = 0;
            foreach (var item in ls) if (item) sum++;

            double value = (double)1 / (double)sum;
            double curent = 0; 
            foreach (var item in ls)
            {
                if(item) 
                    curent += value; 
                res.Add(curent); 
            }

            return res;
        }

        public static List<double> GetBigP(List<TwoIntPair> ls)
        {
            List<double> res = new List<double>();
            double sum = (double)GetSum(ls); 
             

            double currentP = 1;

            foreach (var item in ls)
            {
                res.Add(currentP);
                currentP = currentP - item.Second/sum; 
            }

            return res; 
        }

        public static List<int> Get_dti(List<bool> ls)
        {
            List<int> dti = new List<int>();

            int fn = 1; int st = 0;

            for (int i = 1; i < ls.Count; i++)
            {
                fn = i;
                if (ls[i])
                { 
                    dti.Add(fn - st - 1);
                    st = i;
                }
            }

            return dti; 
        }

        public static List<int> GetCouOfPart(List<bool> ls,int countOfPart,out double size)
        {
            var res = new List<int>();

            size = (ls.Count / countOfPart); 
            
            int count = 0;
            int currentCount = 0; 
            for (int i = 0; i < ls.Count; i++)
            {
                if (currentCount == size)
                {
                    res.Add(count);
                    count = 0;
                    currentCount = 0; 
                }
                currentCount++;
                if (ls[i])
                    count++; 
            }

            return res; 
        }
           

        
        
    }
}
