﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DiplomKurs
{
    class StringIntPair : IComparable
    {
        public string String { get; set; }
        public int Int { get; set; } 

        public StringIntPair(string s, int i)
        {
            String = s;
            Int = i; 
        }



        public int CompareTo(object obj)
        {
            int i = ((StringIntPair) obj).Int;
            if (i > Int) return -1;
            if (i < Int) return 1;
            return 0; 
        }
    }

    class TwoIntPair : IComparable
    {
        public int First;
        public int Second;

        public TwoIntPair(int first, int second)
        {
            First = first;
            Second = second; 
        }

        public int CompareTo(object obj)
        {
            int i = ((TwoIntPair)obj).First;
            if (i > First) return -1;
            if (i < First) return 1;
            return 0;
        }

        public override bool Equals(object obj)
        {
            var elem = ((TwoIntPair)obj);
            if (elem.First != First) return false;
            if (elem.Second != Second) return false;

            return true; 
        }

    }

    class ResearchElement
    {
        public int Count;
        public double Frequecy;

        public ResearchElement(int count,double frequency)
        {
            Count = count;
            Frequecy = frequency; 
        }
    }

    class ParseWords
    {
        protected string connectionString;
        protected int currentIndex = 0;

        protected Dictionary<string, ResearchElement> wordsDictionary = new Dictionary<string, ResearchElement>(); 

        public List<int> newWords = new List<int>();

        public List<bool> ListOfNewWords = new List<bool>();

        public int CountOfWords { get; set; }

        public ParseWords(string connectionStr)
        {
            this.connectionString = connectionStr; 
        }

        private Encoding getEncoding(string fileName)
        {
            using (var reader = new System.IO.StreamReader(fileName))
            {
                reader.Peek(); // you need this!
                var encoding = reader.CurrentEncoding;
                return encoding; 
            }
        }

        public void DoResult()
        {
            var enc = getEncoding(connectionString); 
            using (var sw = new System.IO.StreamReader(connectionString, Encoding.UTF8))//GetEncoding(1251)))
            {
                while (sw.Peek() >= 0)
                {
                    GetCountsOfWords(sw.ReadLine());
                }
            } 
            FillFrequecy(wordsDictionary);
            //newWords.RemoveAt(0);
        }

        public Dictionary<string, ResearchElement> GetWordsDictinary()
        {
            return wordsDictionary; 
        }

        public virtual void GetCountsOfWords(string inputText)
        {
            string[] strArr = inputText.Split(' ');
            
           // var output = new Dictionary<string,ResearchElement>();
            string current = GetWord(strArr[0]);
            foreach (var s in strArr)
            {
                currentIndex++;
                string newStr = GetWord(s);
                if(newStr==null) continue;
                if (wordsDictionary.ContainsKey(newStr))
                {
                    wordsDictionary[newStr].Count++;
                    ListOfNewWords.Add(false); 
                }
                else
                {
                    newWords.Add(currentIndex);
                    currentIndex = 0;
                    wordsDictionary.Add(newStr, new ResearchElement(1, 0));
                    ListOfNewWords.Add(true);
                } 
                CountOfWords++;
                
                //if(!newStr.Equals(current))
                //{
                //    newWords.Add(currentIndex);
                //    current = (string)newStr.Clone(); 
                //    currentIndex = 0; 
                //}
            }
           // newWords.RemoveAt(0);
           // FillFrequecy(wordsDictionary);
        }

        protected void FillFrequecy(Dictionary<string, ResearchElement> dict)
        {
            var newDict = new Dictionary<string, ResearchElement>(); 
            var output = new Dictionary<string, ResearchElement>();
            foreach (var element in dict)
            {
                double f = (double)element.Value.Count / (double)CountOfWords;
                newDict.Add(element.Key, new ResearchElement(element.Value.Count, f));
            }
            wordsDictionary = newDict; 
        }

        protected string GetWord(string InputString)
        {
            string output;
            string s = InputString.ToLower();
            if (s == " " | s.Length < 1) return null;
            if(s.Length>1)if (s[0] == 13 | s[1] ==10) return null;
            int i = s.Length - 1;
            if (s[i] == '.' | s[i] == ',' | s[i] == ':' | s[i] == ';' | s[i] == ')' | s[i] == '(') 
                output = (new StringBuilder(s).Remove(i,1).ToString());
            if (s[0] == '.' | s[0] == ',' | s[0] == ':' | s[0] == ';' | s[0] == ')' | s[0] == '(')
                output = (new StringBuilder(s).Remove(0, 1).ToString());
            else output = s;
            return output; 
        }
    }

    class ParseWordForF: ParseWords 
    {
        public List<string> wordsF;

        public ParseWordForF(string path, int F, Dictionary<string, ResearchElement> dict)
            : base(path)
        {
            GetWordsWithF(F, dict); 
        }

        public virtual void GetWordsWithF(int F,Dictionary<string, ResearchElement> dict)
        {
            List<string> res = new List<string>();
            foreach (var item in dict)
            {
                if (item.Value.Count == F)
                    res.Add(item.Key); 
            }
            wordsF = res; 
        }

        public override void GetCountsOfWords(string inputText)
        {
            string[] strArr = inputText.Split(' ');

            // var output = new Dictionary<string,ResearchElement>();
             
            foreach (var s in strArr)
            {
                currentIndex++;
                string newStr = GetWord(s);
                if (newStr == null) continue;
                if (wordsF.Contains(newStr))
                    ListOfNewWords.Add(true);
                else
                    ListOfNewWords.Add(false);
                CountOfWords++;
            }
        }
    }

    class ParseWordSelectedWord : ParseWords
    {
        string word;

        public ParseWordSelectedWord(string path, string word) :
            base(path)
        {
            this.word = word; 
        }

        public override void GetCountsOfWords(string inputText)
        {
            string[] strArr = inputText.Split(' ');

            // var output = new Dictionary<string,ResearchElement>();

            foreach (var s in strArr)
            {
                currentIndex++;
                string newStr = GetWord(s);
                if (newStr == null) continue;
                if (word== newStr )
                    ListOfNewWords.Add(true);
                else
                    ListOfNewWords.Add(false);
                CountOfWords++;
            }
        }
    }

    class ParseWordSelectedL : ParseWordForF
    {
        int l;

        public ParseWordSelectedL(string path, int F, Dictionary<string, ResearchElement> dict)
            : base(path,F,dict)
        {
            GetWordsWithF(F, dict);
        }


        public override void GetWordsWithF(int F, Dictionary<string, ResearchElement> dict)
        {
            List<string> res = new List<string>();
            foreach (var item in dict)
            {
                if (item.Key.Length == F)
                    res.Add(item.Key);
            }
            wordsF = res;
        }
    }
}
