using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomKurs
{
    public class Part2
    {
        public int StartPlot;
        public int EndPlot;
        public int StepMove;
        public int StepGoBiger; 
        public List<bool> list;

        private int GetStartCount(int size)
        {
            int sum = 0;
            for (int i = 0; i < size; i++)
                sum += list[i].GetInt();

            return sum; 
        }

        private int GetSum(int start, int end)
        {
            int sum = 0;
            for (int i = start; i <= end; i++)
                sum += list[i].GetInt();

            return sum; 
        }

        public double GetSqure(int size)
        {
            int sum = 0; 
            int currentStart = 0 ;
            int count = 0; 
            while(currentStart + size < list.Count)
            { 
                sum += GetSum(currentStart, currentStart + size); 
                currentStart += StepMove;
                count++; 
            }

            return (sum/count); 
        }

        public Tuple<double,double> GetSqureAndD(int size)
        {
            int sum = 0;
            int currentStart = 0;
            int count = 0;
            List<double> ls = new List<double>(); 
            while (currentStart + size < list.Count)
            {
                var crrSum = GetSum(currentStart, currentStart + size);
                sum += crrSum;
                currentStart += StepMove;
                count++;
                ls.Add(crrSum); 
            }

            return new Tuple<double,double>((sum / count),Mathematic.GetSqureError(ls.ToArray()));
        }

        public Tuple<List<int>,List<int>> GetResult()
        {
            List<int> w = new List<int>(); 
            List<int> p = new List<int>();
            for (int i = StartPlot; i <= EndPlot; i+=StepGoBiger)
            {
                w.Add(i);
                p.Add((int)GetSqure(i)); 
            }
            return new Tuple<List<int>, List<int>>(w, p); 
        }

        public Tuple<List<int>, List<double>,List<double>> GetResultF()
        {
            List<int> w = new List<int>();
            List<double> p = new List<double>();
            List<double> dp = new List<double>();
            for (int i = StartPlot; i <= EndPlot; i += StepGoBiger)
            {
                w.Add(i);
                var sq = GetSqureAndD(i); 
                p.Add(sq.Item1);
                dp.Add(sq.Item2);
            }
            return new Tuple<List<int>, List<double>,List<double>>(w, p,dp);
        }
    }
}
