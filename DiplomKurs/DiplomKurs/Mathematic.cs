using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomKurs
{
    class Mathematic
    {
        public static double MathematicHope(double[] inpArr)
        {
            double sum = 0;
            foreach (var elem in inpArr) sum += elem;
            return (sum / inpArr.Length); 
        }

        public static double Dispersia(double[] inpArr, double mathHoupe = double.MinValue)
        { 
            if (mathHoupe == double.MinValue) mathHoupe = MathematicHope(inpArr);
            //var dict = new Dictionary<double, int>();
            //foreach (var elem in inpArr)
            //{
            //    if (dict.ContainsKey(elem)) dict[elem]++;
            //    else dict.Add(elem,1); 
            //}

            //foreach (var elem in inpArr)
            //{
            //    double one = (Math.Pow((elem - mathHoupe), 2));
            //    double two = (double)((double)dict[elem] / (double)inpArr.Length); 
            //    output += one * two; 
            //}

            double sumM = 0;
            foreach (var elem in inpArr) sumM += elem*elem;
            sumM = sumM / inpArr.Length; 

            return sumM - (mathHoupe*mathHoupe); 
        }

        public static double GetSqureError(double[] inpArr)
        {
            return Math.Sqrt(Dispersia(inpArr)); 
        }

        public static double[] Gause(double[] inpArr, double mathHope = double.MinValue, double disp = double.MinValue)
        {
            double[] output = new double[inpArr.Length]; 
            if (mathHope == double.MinValue) mathHope = MathematicHope(inpArr);
            if (disp == double.MinValue) disp = Dispersia(inpArr, mathHope);

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = gausFormula(inpArr[i], mathHope, disp); 
            }
            return output; 
        }

        private static double gausFormula(double x, double mathHope,double disp)
        { 
            double onePart1 = Math.Sqrt(disp * 2 * Math.PI) ;
            double one = Math.Pow(onePart1,-1);

            double twoPart1 = Math.Pow((x-mathHope),2);
            double twoPart2 = Math.Pow((2*disp),-1);
            double two = Math.Pow(Math.E, (-twoPart1 * twoPart2));

            return one * two; 
            
        }
    }
}
