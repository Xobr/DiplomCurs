using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DiplomKurs
{
    public enum StopCharsMode
    {
        Latter = 2,
        Char = 0
    }

    public class Config
    {
        public static string SSWord = "StopSC.txt";
        public static string SSLatter = "StopSL.txt";

        public static bool ShowNewWordsTable = false;
        public static readonly string NotFileMessage = "Ви не вибрали жодного файлу. Будь ласка, для продовження роботи з програмо виберіть файл.";
        public static readonly char[] StopChars = new char[] { ',', '.', ':', ';', '"', "'"[0], '-', '(', ')' };

        public static List<string> StopSymbolLatter { get; private set; }
        public static List<string> StopSymbolWord { get; private set; }

        public static void RefreshStopChars()
        {
            if (!File.Exists(SSWord))
            {
                var fs = File.Create(SSWord);
                fs.Close(); 
            }
            if (!File.Exists(SSLatter))
            {
                var fs = File.Create(SSLatter);
                fs.Close(); 
            }
            StopSymbolLatter = Program.GetStringListFromFile(SSLatter);
            StopSymbolWord = Program.GetStringListFromFile(SSWord); 
        }
    }
}
