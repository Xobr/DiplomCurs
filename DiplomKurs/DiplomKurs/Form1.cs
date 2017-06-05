using System; 
using System.IO; 
using System.Windows.Forms;
using System.Linq; 
using System.Collections.Generic;
using System.Data;
using System.Text; 

namespace DiplomKurs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<string, ResearchElement> dictAnlysText;

        ParseWordSelectedWord pws; 
        ParseWordForF pwf; 
        ParseWords pw;
        List<TwoIntPair> dti_litelP;
        List<double> dti_bigP;

        List<Tuple<int, double>> ti_litelP;
        List<double> ti_bigP;
        List<int> ti_average;

        List<double> newWords_P; 

        List<int> countInInterval;

        public static bool DataTableToCSV(DataTable dtSource, string path)//, StreamWriter writer, bool includeHeader)
        {
            var writer = new StreamWriter(path, true, Encoding.UTF8);

            string[] columnNames = dtSource.Columns.Cast<DataColumn>().Select(column => column.ToString()).ToArray(); //"\"" + column.ColumnName.Replace("\"", "\"\"") + "\"").ToArray<string>();
            writer.WriteLine(String.Join("\t", columnNames));
            writer.Flush();

            foreach (DataRow row in dtSource.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray(); // "\"" + field.ToString().Replace("\"", "\"\"") + "\"").ToArray<string>();
                writer.WriteLine(String.Join("\t", fields));
                writer.Flush();
            }

            return true;
        }

        private DataTable GetDataTableFromDGV(DataGridView dgv)
        {
            var dt = new DataTable();
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    // You could potentially name the column based on the DGV column name (beware of dupes)
                    // or assign a type based on the data type of the data bound to this DGV column.
                    dt.Columns.Add(column.HeaderText);
                }
            }

            object[] cellValues = new object[dgv.Columns.Count];
            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    cellValues[i] = row.Cells[i].Value;
                }
                dt.Rows.Add(cellValues);
            }

            return dt;
        }

        private void Draw(Dictionary<string, ResearchElement> dictionary,int countOfWords)
        {
            chart1.Series[0].Points.Clear();
            var sl = new SortedList<int, string>();
            var ls = new List<StringIntPair>();
            List<double> CArr = new List<double>();
            List<double> FArr = new List<double>(); 
            
            foreach (var elem in dictionary)
            {
                dataGridView1.Rows.Add(elem.Key, ((double)elem.Value.Count),elem.Value.Frequecy);
                CArr.Add(elem.Value.Count);
                FArr.Add(elem.Value.Frequecy); 
                ls.Add(new StringIntPair(elem.Key,elem.Value.Count));
                //sl.Add(elem.Value,elem.Key);
            }


            dataGridView1.Rows.Add("Sum = ",countOfWords,"1");
            double oC = Math.Sqrt(Mathematic.Dispersia(CArr.ToArray()));
            double oF = Math.Sqrt(Mathematic.Dispersia(FArr.ToArray()));
            dataGridView1.Rows.Add("O",(int)oC,oF); 

            ls.Sort();
            int point = 0; 
            for(int i = ls.Count-1 ; i > ls.Count-11;i--)
            { 
                chart1.Series[0].Points.Add(ls[i].Int);
                chart1.Series[0].Points[point].Label = ls[i].String + String.Format(" ({0})",ls[i].Int);
                point++; 
            }
            string s = null;
            foreach (char d in ls[ls.Count - 1].String)
            {
                s += (int)d;
            }
        }

        private void DrawF(Dictionary<string, ResearchElement> dictionary, int countOfWords,int F)
        {
            chart1.Series[0].Points.Clear();
            var sl = new SortedList<int, string>();
            var ls = new List<StringIntPair>();
            List<double> CArr = new List<double>();
            List<double> FArr = new List<double>();

            foreach (var elem in dictionary)
            {
                if (elem.Value.Count != F) continue; 
                dataGridView6.Rows.Add(elem.Key, elem.Value.Count, elem.Value.Frequecy);
                CArr.Add(elem.Value.Count);
                FArr.Add(elem.Value.Frequecy);
                ls.Add(new StringIntPair(elem.Key, elem.Value.Count));
                //sl.Add(elem.Value,elem.Key);
            }


            dataGridView6.Rows.Add("Sum = ", countOfWords, "1");
            double oC = Math.Sqrt(Mathematic.Dispersia(CArr.ToArray()));
            double oF = Math.Sqrt(Mathematic.Dispersia(FArr.ToArray()));
            dataGridView6.Rows.Add("O", oC, oF);

            ls.Sort();
            int point = 0;
            for (int i = ls.Count - 1; i > ls.Count - 11; i--)
            {
                chart9.Series[0].Points.Add(ls[i].Int);
                chart9.Series[0].Points[point].Label = ls[i].String + String.Format(" ({0})", ls[i].Int);
                point++;
            }
             
        }

        private void DrawSW(Dictionary<string, ResearchElement> dictionary, int countOfWords, string w)
        {
            chart1.Series[0].Points.Clear();
            var sl = new SortedList<int, string>();
            var ls = new List<StringIntPair>();
            List<double> CArr = new List<double>();
            List<double> FArr = new List<double>();

            foreach (var elem in dictionary)
            {
                if (elem.Key != w) continue;
                dataGridView11.Rows.Add(elem.Key, elem.Value.Count, elem.Value.Frequecy);
                CArr.Add(elem.Value.Count);
                FArr.Add(elem.Value.Frequecy);
                ls.Add(new StringIntPair(elem.Key, elem.Value.Count));
                //sl.Add(elem.Value,elem.Key);
            }


            dataGridView11.Rows.Add("Sum = ", countOfWords, "1");
            double oC = Math.Sqrt(Mathematic.Dispersia(CArr.ToArray()));
            double oF = Math.Sqrt(Mathematic.Dispersia(FArr.ToArray()));
            dataGridView11.Rows.Add("O", oC, oF);

            //ls.Sort();
            //int point = 0;
            //for (int i = ls.Count - 1; i > ls.Count - 11; i--)
            //{
            //    chart17.Series[0].Points.Add(ls[i].Int);
            //    chart17.Series[0].Points[point].Label = ls[i].String + String.Format(" ({0})", ls[i].Int);
            //    point++;
            //}
        }

        private void DrawNewWrods()
        {
            try
            {
                var p = Imovirnist.GetBigPForNewWords(pw.ListOfNewWords);
                for (int i = 0; i < p.Count; i++)
                {
                    chart7.Series[0].Points.AddXY(i, p[i]);
                    dataGridView4.Rows.Add(i, pw.ListOfNewWords[i].GetInt(), p[i]);
                }
                newWords_P = p;
                //THIS PLACE
                var disp = Mathematic.Dispersia(p.ToArray());
                dataGridView4.Rows.Add("O", " ", Math.Sqrt(disp)); 
            }
            catch { }
        }

        private void DrawNewWrodsF()
        {
            try
            {
                var p = Imovirnist.GetBigPForNewWords(pwf.ListOfNewWords);
                for (int i = 0; i < p.Count; i++)
                {
                    chart15.Series[0].Points.AddXY(i, p[i]);
                    dataGridView9.Rows.Add(i, pwf.ListOfNewWords[i].GetInt(), p[i]);
                }
                //newWords_P = p;
                //THIS PLACE
                var disp = Mathematic.Dispersia(p.ToArray());
                dataGridView9.Rows.Add("O", " ", Math.Sqrt(disp));
            }
            catch { }
        }


        private void DrawNewWrodsSW()
        {
            try
            {
                var p = Imovirnist.GetBigPForNewWords(pws.ListOfNewWords);
                for (int i = 0; i < p.Count; i++)
                {
                    chart23.Series[0].Points.AddXY(i, p[i]);
                    dataGridView14.Rows.Add(i, pws.ListOfNewWords[i].GetInt(), p[i]);
                }
                //newWords_P = p;
                //THIS PLACE
                var disp = Mathematic.Dispersia(p.ToArray());
                dataGridView14.Rows.Add("O", " ", Math.Sqrt(disp));
            }
            catch { }
        }

        private void DrawImovirn(ParseWords pw)
        {
            Imovirnist imovirnist = new Imovirnist(pw.newWords.ToArray(),pw.CountOfWords);
            var LitelP = Imovirnist.GetLitelP(pw.ListOfNewWords);
            var BigP = Imovirnist.GetBigP(LitelP);

            List<double> listDivSum = new List<double>();
            List<double> listLP = new List<double>();

            double sum = Imovirnist.GetSum(LitelP);

            dti_litelP = LitelP;
            dti_bigP = BigP; 

            //double[] gauspx = Mathematic.Gause(imovirnist.p);
            //double[] gausBigpx = Mathematic.Gause(BigP.ToArray());

            for (int i = 0; i < LitelP.Count; i++)
            {
                dataGridView2.Rows.Add(LitelP[i].First, LitelP[i].Second, 
                    LitelP[i].Second/sum, BigP[i]);
                listDivSum.Add(LitelP[i].Second / sum);
                listLP.Add(LitelP[i].Second); 
                chart3.Series[0].Points.AddXY(LitelP[i].First, BigP[i]); 
                chart2.Series[0].Points.AddXY(LitelP[i].First, LitelP[i].Second);

                //chart5.Series[0].Points.AddXY(i, gausBigpx[i]); 
            }
            var d1 = Mathematic.GetSqureError(listLP.ToArray());
            var d2 = Mathematic.GetSqureError(listDivSum.ToArray());
            var d3 = Mathematic.GetSqureError(BigP.ToArray());

            dataGridView2.Rows.Add("O",d1,d2,d3);
 
            //for (int i = 0; i < imovirnist.n.Length;i++ )
            //{
            //    dataGridView2.Rows.Add(i,imovirnist.n[i],imovirnist.p[i],imovirnist.bigP[i]);
            //    //chart2.Series[0].Points.AddXY(i, imovirnist.p[i]);
            //    //chart3.Series[0].Points.AddXY(i, imovirnist.bigP[i]);
            //    //chart4.Series[0].Points.AddXY(i, gauspx[i]);
            //    //chart5.Series[0].Points.AddXY(i, gausBigpx[i]); 
            //}

            DrawTi(pw.ListOfNewWords);
        }

        private void DrawImovirnF(ParseWords pw)
        {
            Imovirnist imovirnist = new Imovirnist(pwf.newWords.ToArray(), pwf.CountOfWords);
            var LitelP = Imovirnist.GetLitelP(pwf.ListOfNewWords);
            var BigP = Imovirnist.GetBigP(LitelP);

            List<double> listDivSum = new List<double>();
            List<double> listLP = new List<double>();

            double sum = Imovirnist.GetSum(LitelP);

            dti_litelP = LitelP;
            dti_bigP = BigP;

            //double[] gauspx = Mathematic.Gause(imovirnist.p);
            //double[] gausBigpx = Mathematic.Gause(BigP.ToArray());

            for (int i = 0; i < LitelP.Count; i++)
            {
                dataGridView7.Rows.Add(LitelP[i].First, LitelP[i].Second,
                    LitelP[i].Second / sum, BigP[i]);
                listDivSum.Add(LitelP[i].Second / sum);
                listLP.Add(LitelP[i].Second);
                chart11.Series[0].Points.AddXY(LitelP[i].First, BigP[i]);
                chart10.Series[0].Points.AddXY(LitelP[i].First, LitelP[i].Second);

                //chart5.Series[0].Points.AddXY(i, gausBigpx[i]); 
            }
            var d1 = Mathematic.GetSqureError(listLP.ToArray());
            var d2 = Mathematic.GetSqureError(listDivSum.ToArray());
            var d3 = Mathematic.GetSqureError(BigP.ToArray());

            dataGridView7.Rows.Add("O", d1, d2, d3);

            //for (int i = 0; i < imovirnist.n.Length;i++ )
            //{
            //    dataGridView2.Rows.Add(i,imovirnist.n[i],imovirnist.p[i],imovirnist.bigP[i]);
            //    //chart2.Series[0].Points.AddXY(i, imovirnist.p[i]);
            //    //chart3.Series[0].Points.AddXY(i, imovirnist.bigP[i]);
            //    //chart4.Series[0].Points.AddXY(i, gauspx[i]);
            //    //chart5.Series[0].Points.AddXY(i, gausBigpx[i]); 
            //}

            DrawTiF(pwf.ListOfNewWords);
        }


        private void DrawImovirnSW(ParseWords pws)
        { 
            Imovirnist imovirnist = new Imovirnist(pws.newWords.ToArray(), pws.CountOfWords);
            var LitelP = Imovirnist.GetLitelP(pws.ListOfNewWords);
            var BigP = Imovirnist.GetBigP(LitelP);

            List<double> listDivSum = new List<double>();
            List<double> listLP = new List<double>();

            double sum = Imovirnist.GetSum(LitelP);

            dti_litelP = LitelP;
            dti_bigP = BigP;

            //double[] gauspx = Mathematic.Gause(imovirnist.p);
            //double[] gausBigpx = Mathematic.Gause(BigP.ToArray());

            for (int i = 0; i < LitelP.Count; i++)
            {
                dataGridView12.Rows.Add(LitelP[i].First, LitelP[i].Second,
                    LitelP[i].Second / sum, BigP[i]);
                listDivSum.Add(LitelP[i].Second / sum);
                listLP.Add(LitelP[i].Second);
                chart19.Series[0].Points.AddXY(LitelP[i].First, BigP[i]);
                chart18.Series[0].Points.AddXY(LitelP[i].First, LitelP[i].Second);

                //chart5.Series[0].Points.AddXY(i, gausBigpx[i]); 
            }
            var d1 = Mathematic.GetSqureError(listLP.ToArray());
            var d2 = Mathematic.GetSqureError(listDivSum.ToArray());
            var d3 = Mathematic.GetSqureError(BigP.ToArray());

            dataGridView7.Rows.Add("O", d1, d2, d3);

            //for (int i = 0; i < imovirnist.n.Length;i++ )
            //{
            //    dataGridView2.Rows.Add(i,imovirnist.n[i],imovirnist.p[i],imovirnist.bigP[i]);
            //    //chart2.Series[0].Points.AddXY(i, imovirnist.p[i]);
            //    //chart3.Series[0].Points.AddXY(i, imovirnist.bigP[i]);
            //    //chart4.Series[0].Points.AddXY(i, gauspx[i]);
            //    //chart5.Series[0].Points.AddXY(i, gausBigpx[i]); 
            //}

            DrawTiSW(pws.ListOfNewWords);
        }

        private void DrawTi(List<bool> ls)
        {
            int countOfPart = int.Parse(textBox1.Text);
            double size; 
            var res = Imovirnist.GetCouOfPart(ls,countOfPart,out size);
            var p = res.GetTwoIntPair();
            var litelP = Imovirnist.GetLitelPWithDouble(p);
            var bigP = Imovirnist.GetBigP(p);
            List<int> listAverages = new List<int>(); 

            //foreach (var item in res)
            double average = size/2;  
            for (int i = 0; i < res.Count; i++) 
            {
                chart6.Series[0].Points.AddY(res[i]);
                chart6.Series[0].Points[i].Label = ((int)p[i].Second).ToString(); 
                chart4.Series[0].Points.AddXY(litelP[i].Item1,litelP[i].Item2);
                chart5.Series[0].Points.AddY(bigP[i]);
                dataGridView3.Rows.Add(i,p[i].Second,average,litelP[i].Item2,bigP[i]);
                listAverages.Add((int)average); 
                average += size; 
            }

            ti_average = listAverages;
            ti_litelP = litelP;
            ti_bigP = bigP;
        }

        private void DrawTiF(List<bool> ls)
        {
            int countOfPart = int.Parse(textBox6.Text);
            double size;
            var res = Imovirnist.GetCouOfPart(ls, countOfPart, out size);
            var p = res.GetTwoIntPair();
            var litelP = Imovirnist.GetLitelPWithDouble(p);
            var bigP = Imovirnist.GetBigP(p);
            List<int> listAverages = new List<int>();

            //foreach (var item in res)
            double average = size / 2;
            for (int i = 0; i < res.Count; i++)
            {
                chart12.Series[0].Points.AddY(res[i]);
                chart12.Series[0].Points[i].Label = ((int)average).ToString();
                chart13.Series[0].Points.AddXY(litelP[i].Item1, litelP[i].Item2);
                chart14.Series[0].Points.AddY(bigP[i]);
                dataGridView8.Rows.Add(i, p[i].Second, average, litelP[i].Item2, bigP[i]);
                listAverages.Add((int)average);
                average += size;
            } 
        }


        private void DrawTiSW(List<bool> ls)
        {
            int countOfPart = int.Parse(textBox13.Text);
            double size;
            var res = Imovirnist.GetCouOfPart(ls, countOfPart, out size);
            var p = res.GetTwoIntPair();
            var litelP = Imovirnist.GetLitelPWithDouble(p);
            var bigP = Imovirnist.GetBigP(p);
            List<int> listAverages = new List<int>();

            //foreach (var item in res)
            double average = size / 2;
            for (int i = 0; i < res.Count; i++)
            {
                chart20.Series[0].Points.AddY(res[i]);
                chart20.Series[0].Points[i].Label = ((int)average).ToString();
                chart21.Series[0].Points.AddXY(litelP[i].Item1, litelP[i].Item2);
                chart12.Series[0].Points.AddY(bigP[i]);
                dataGridView13.Rows.Add(i, p[i].Second, average, litelP[i].Item2, bigP[i]);
                listAverages.Add((int)average);
                average += size;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            chart5.Series[0].Points.Clear();
            chart6.Series[0].Points.Clear();
            chart7.Series[0].Points.Clear();

            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear(); 
            
            if(openFileDialog1.ShowDialog() != DialogResult.OK) return;
            string way = openFileDialog1.FileName;
            //using (var sw = new StreamReader(way))
            //{
            //    string input = sw.ReadToEnd();
            //    ParseWords pw = new ParseWords(input);
            //    Dictionary<string, ResearchElement> dict = pw(); 
            //    Draw(dict,pw.CountOfWords);
            //    DrawImovirn(pw); 
            //}

            ParseWords pw = new ParseWords(way);
            this.pw = pw; 
            pw.DoResult();
            Dictionary<string, ResearchElement> dict = pw.GetWordsDictinary();

            button2_Click(sender, e); 
            //button4_Click(sender, e);
            //button5_Click(sender, e);
            dictAnlysText = dict;
            Draw(dict, pw.CountOfWords);
            DrawImovirn(pw);
            DrawNewWrods();
            MessageBox.Show("Done"); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                chart6.Series[0].Points.Clear();
                chart4.Series[0].Points.Clear();
                chart5.Series[0].Points.Clear();
                dataGridView3.Rows.Clear(); 
                DrawTi(pw.ListOfNewWords);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string folder = folderBrowserDialog1.SelectedPath;

            


            string pathNW = string.Format("{0}/{1}",folder,"NewWord");
            string pathF = string.Format("{0}/{1}",folder,"F");
            Directory.CreateDirectory(pathNW); 
            Directory.CreateDirectory(pathF);



            DataTableToCSV(GetDataTableFromDGV(dataGridView1), pathNW + "/AnalysisText.txt");
            DataTableToCSV(GetDataTableFromDGV(dataGridView2), pathNW + "/dTi.txt");
            DataTableToCSV(GetDataTableFromDGV(dataGridView3), pathNW + "/Ti.txt"); 
            DataTableToCSV(GetDataTableFromDGV(dataGridView4), pathNW + "/New Words.txt");
            DataTableToCSV(GetDataTableFromDGV(dataGridView5), pathNW + "/F.txt");

            DataTableToCSV(GetDataTableFromDGV(dataGridView6), pathF + "/AnalysisText.txt");
            DataTableToCSV(GetDataTableFromDGV(dataGridView7), pathF + "/dTi.txt");
            DataTableToCSV(GetDataTableFromDGV(dataGridView8), pathF + "/Ti.txt");
            DataTableToCSV(GetDataTableFromDGV(dataGridView9), pathF + "/New Words.txt");
            DataTableToCSV(GetDataTableFromDGV(dataGridView10), pathF + "/F.txt");

            //  SaveTextAn(folder+"/AnalysisText.txt"); 
            //SaveDTI(folder+"/dTI.txt");
            //SaveTI(folder + "/TI.txt"); 
            //SaveNewWords(folder + "/NewWord.txt");

            MessageBox.Show("Saved");
        }

        private void SaveTextAn(string path)
        {
            using (StreamWriter sw = new StreamWriter(path,true,System.Text.Encoding.UTF8))
            {
                sw.WriteLine("Wrod\tCount\tF");
                foreach (var item in dictAnlysText)
                {
                    sw.WriteLine(String.Format("{0}\t{1}\t{2}",item.Key,item.Value.Count,item.Value.Frequecy)); 
                }
	
            }
        }

        private void SaveDTI(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8))
            {
                var sum = Imovirnist.GetSum(dti_litelP);
                sw.WriteLine("X\tN\tp(x)\tP(x)");
                for (int i = 0; i < dti_litelP.Count; i++)
                {
                    double p = (double)dti_litelP[i].Second / (double)sum;
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}",
                        dti_litelP[i].First,dti_litelP[i].Second,p,dti_bigP[i]); 
                }
            }
        }

        private void SaveTI(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8))
            {
                var sum = Imovirnist.GetSum(dti_litelP);
                sw.WriteLine("X\tN\tAverage\tp(x)\tP(x)");
                for (int i = 0; i < ti_litelP.Count; i++)
                {
                    double p = (double)dti_litelP[i].Second / (double)sum;
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",
                        i, ti_litelP[i].Item1, ti_average[i], ti_litelP[i].Item2, ti_bigP[i]);
                }
            }
        }

        private void SaveNewWords(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("Possition\tValue\tP(x)");
                for(int i = 0 ; i < pw.ListOfNewWords.Count ; i++)
                {
                    sw.WriteLine(String.Format("{0}\t{1}\t{2}", i, pw.ListOfNewWords[i].GetInt(),newWords_P[i]));
                }

            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView5.Rows.Clear();
            chart8.Series[0].Points.Clear(); 
            int c1 = int.Parse(textBox2.Text);
            int c2 = int.Parse(textBox3.Text);
            int c3 = int.Parse(textBox4.Text);
            int c4 = int.Parse(textBox5.Text);
            Part2 p1 = new Part2();

            p1.StartPlot = c1;
            p1.EndPlot = c2;
            p1.StepMove = c3;
            p1.StepGoBiger = c4;
            p1.list = pw.ListOfNewWords; 

            var list = p1.GetResult();

            for (int i = 0; i < list.Item1.Count; i++)
            {
                dataGridView5.Rows.Add(list.Item1[i], list.Item2[i]);
                chart8.Series[0].Points.AddXY(list.Item1[i], list.Item2[i]); 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int f = int.Parse(textBox11.Text); 
            ParseWordForF pf = new ParseWordForF(openFileDialog1.FileName, f, dictAnlysText);

            this.pwf = pf;
            pwf.DoResult();

           // DrawF(dictAnlysText, pf.CountOfWords, f);

            chart8.Series[0].Points.Clear();
            chart9.Series[0].Points.Clear();
            chart10.Series[0].Points.Clear();
            chart11.Series[0].Points.Clear();
            chart12.Series[0].Points.Clear();
            chart13.Series[0].Points.Clear();
            chart14.Series[0].Points.Clear();
            chart15.Series[0].Points.Clear(); 

            dataGridView5.Rows.Clear();
            dataGridView6.Rows.Clear();
            dataGridView7.Rows.Clear();
            dataGridView8.Rows.Clear();
            dataGridView9.Rows.Clear(); 

         

           
            Dictionary<string, ResearchElement> dict = pw.GetWordsDictinary();


            dictAnlysText = dict;
            DrawF(dict, pwf.CountOfWords,f);
            DrawImovirnF(pwf);
            DrawNewWrodsF();
            MessageBox.Show("Done"); 
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView10.Rows.Clear();
            chart16.Series[0].Points.Clear();
            int c1 = int.Parse(textBox10.Text);
            int c2 = int.Parse(textBox9.Text);
            int c3 = int.Parse(textBox8.Text);
            int c4 = int.Parse(textBox7.Text);
            Part2 p1 = new Part2();

            p1.StartPlot = c1;
            p1.EndPlot = c2;
            p1.StepMove = c3;
            p1.StepGoBiger = c4;
            p1.list = pwf.ListOfNewWords;

            var list = p1.GetResultF();

            for (int i = 0; i < list.Item1.Count; i++)
            {
                dataGridView10.Rows.Add(list.Item1[i], list.Item2[i], list.Item3[i]);
                chart16.Series[0].Points.AddXY(list.Item1[i],list.Item3[i]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string w = textBox12.Text;
            //ParseWordForF pf = new ParseWordForF(openFileDialog1.FileName, f, dictAnlysText);
            ParseWordSelectedWord pf = new ParseWordSelectedWord(openFileDialog1.FileName,w);

            pf.DoResult();
            this.pws = pf; 

            DrawSW(dictAnlysText, pf.CountOfWords, w);

            chart17.Series[0].Points.Clear();
            chart18.Series[0].Points.Clear();
            chart19.Series[0].Points.Clear();
            chart20.Series[0].Points.Clear();
            chart21.Series[0].Points.Clear();
            chart22.Series[0].Points.Clear();
            chart23.Series[0].Points.Clear();
            chart24.Series[0].Points.Clear();

            dataGridView11.Rows.Clear();
            dataGridView12.Rows.Clear();
            dataGridView13.Rows.Clear();
            dataGridView14.Rows.Clear();
            dataGridView15.Rows.Clear();




            Dictionary<string, ResearchElement> dict = pw.GetWordsDictinary();


            dictAnlysText = dict;
            //DrawF(dict, pwf.CountOfWords, f);
            DrawImovirnSW(pws);
            DrawNewWrodsSW();
            MessageBox.Show("Done");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView10.Rows.Clear();
            chart16.Series[0].Points.Clear();
            int c1 = int.Parse(textBox17.Text);
            int c2 = int.Parse(textBox16.Text);
            int c3 = int.Parse(textBox15.Text);
            int c4 = int.Parse(textBox14.Text);
            Part2 p1 = new Part2();

            p1.StartPlot = c1;
            p1.EndPlot = c2;
            p1.StepMove = c3;
            p1.StepGoBiger = c4;
            p1.list = pws.ListOfNewWords;

            var list = p1.GetResultF();

            for (int i = 0; i < list.Item1.Count; i++)
            {
                dataGridView15.Rows.Add(list.Item1[i], list.Item2[i], list.Item3[i]);
                chart24.Series[0].Points.AddXY(list.Item1[i], list.Item3[i]);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                chart20.Series[0].Points.Clear();
                chart21.Series[0].Points.Clear();
                chart22.Series[0].Points.Clear();
                dataGridView13.Rows.Clear();
                DrawTiSW(pws.ListOfNewWords);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
