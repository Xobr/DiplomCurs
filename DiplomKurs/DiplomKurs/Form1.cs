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
        ParseWordNGram pwSN;
        ParseWords pwN;
        List<TwoIntPair> dti_litelP;
        List<double> dti_bigP;

        //List<Tuple<int, double>> ti_litelP;
        //List<double> ti_bigP;
        //List<int> ti_average;

        List<double> newWords_P;

        List<int> countInInterval;

        public void NewWordsToFile(ParseWords pw, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine(string.Join("\t", "Possition", "Value", "P(x)"));
                if (pw == null)
                {
                    return;
                }
                var bigp = Imovirnist.GetBigPForNewWords(pw.ListOfNewWords);
                // var litp = Imovirnist.GetLitelP(pw.ListOfNewWords); 
                for (int i = 0; i < bigp.Count; i++)
                {
                    sw.WriteLine(string.Join("\t", i, pw.ListOfNewWords[i].GetInt(), bigp[i].ToString()));
                }
            }
        }

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

        private void Draw(Dictionary<string, ResearchElement> dictionary, int countOfWords)
        {
            chart1.Series[0].Points.Clear();
            var sl = new SortedList<int, string>();
            var ls = new List<StringIntPair>();
            List<double> CArr = new List<double>();
            List<double> FArr = new List<double>();

            foreach (var elem in dictionary)
            {
                dataGridView1.Rows.Add(elem.Key, ((double)elem.Value.Count), elem.Value.Frequecy);
                CArr.Add(elem.Value.Count);
                FArr.Add(elem.Value.Frequecy);
                ls.Add(new StringIntPair(elem.Key, elem.Value.Count));
                //sl.Add(elem.Value,elem.Key);
            }


            // dataGridView1.Rows.Add("Sum = ",countOfWords,"1");
            double oC = Math.Sqrt(Mathematic.Dispersia(CArr.ToArray()));
            double oF = Math.Sqrt(Mathematic.Dispersia(FArr.ToArray()));

            ls.Sort();
            int point = 0;
            for (int i = ls.Count - 1; i > ls.Count - 11; i--)
            {
                chart1.Series[0].Points.Add(ls[i].Int);
                chart1.Series[0].Points[point].Label = ls[i].String + String.Format(" ({0})", ls[i].Int);
                point++;
            }
        }

        private void DrawNG(Dictionary<string, ResearchElement> dictionary, int countOfWords)
        {
            chart33.Series[0].Points.Clear();
            var sl = new SortedList<int, string>();
            var ls = new List<StringIntPair>();
            List<double> CArr = new List<double>();
            List<double> FArr = new List<double>();

            foreach (var elem in dictionary)
            {
                dataGridView21.Rows.Add(elem.Key, ((double)elem.Value.Count), elem.Value.Frequecy);
                CArr.Add(elem.Value.Count);
                FArr.Add(elem.Value.Frequecy);
                ls.Add(new StringIntPair(elem.Key, elem.Value.Count));
            }

            double oC = Math.Sqrt(Mathematic.Dispersia(CArr.ToArray()));
            double oF = Math.Sqrt(Mathematic.Dispersia(FArr.ToArray()));

            ls.Sort();
            int point = 0;
            for (int i = ls.Count - 1; i > ls.Count - 11; i--)
            {
                chart33.Series[0].Points.Add(ls[i].Int);
                chart33.Series[0].Points[point].Label = ls[i].String + String.Format(" ({0})", ls[i].Int);
                point++;
            }
        }


        private void DrawN(string value, int count)
        {
            chart16.Series[0].Points.Clear();

            dataGridView16.Rows.Add(value, count, 1);
            chart25.Series[0].Points.Add(count);
            chart25.Series[0].Points[0].Label = value;
        }

        private void DrawF(Dictionary<string, ResearchElement> dictionary, int countOfWords, int F)
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

            double oC = Math.Sqrt(Mathematic.Dispersia(CArr.ToArray()));
            double oF = Math.Sqrt(Mathematic.Dispersia(FArr.ToArray()));

            ls.Sort();
            int point = 0;
            for (int i = ls.Count - 1; i > ls.Count - 11; i--)
            {
                chart9.Series[0].Points.Add(ls[i].Int);
                chart9.Series[0].Points[point].Label = ls[i].String + String.Format(" ({0})", ls[i].Int);
                point++;
            }

        }

        private void DrawSW(string w, int count)
        {
            chart1.Series[0].Points.Clear();

            dataGridView11.Rows.Add(w, count, 1);
            chart17.Series[0].Points.Add(count);
            chart17.Series[0].Points[0].Label = w;

        }

        private void DrawNewWrods()
        {
            var p = Imovirnist.GetBigPForNewWords(pw.ListOfNewWords);
            for (int i = 0; i < p.Count; i++)
            {
                chart7.Series[0].Points.AddXY(i, p[i]);
                if (Config.ShowNewWordsTable)
                    dataGridView4.Rows.Add(i, pw.ListOfNewWords[i].GetInt(), p[i]);
            }
            //newWords_P = p;

            var disp = Mathematic.Dispersia(p.ToArray());
            if (!Config.ShowNewWordsTable)
                dataGridView4.Enabled = false;
        }

        private void DrawNewWrodsNG()
        {
            var p = Imovirnist.GetBigPForNewWords(pwN.ListOfNewWords);
            for (int i = 0; i < p.Count; i++)
            {
                chart39.Series[0].Points.AddXY(i, p[i]);
                if (Config.ShowNewWordsTable)
                    dataGridView24.Rows.Add(i, pw.ListOfNewWords[i].GetInt(), p[i]);
            }
            //newWords_P = p;

            var disp = Mathematic.Dispersia(p.ToArray());
            if (!Config.ShowNewWordsTable)
                dataGridView24.Enabled = false;
        }

        private void DrawNewWrodsN()
        {
            var p = Imovirnist.GetBigPForNewWords(pwSN.ListOfNewWords);
            for (int i = 0; i < p.Count; i++)
            {
                chart31.Series[0].Points.AddXY(i, p[i]);
                if (Config.ShowNewWordsTable)
                    dataGridView19.Rows.Add(i, pw.ListOfNewWords[i].GetInt(), p[i]);
            }
            newWords_P = p;

            var disp = Mathematic.Dispersia(p.ToArray());
            if (!Config.ShowNewWordsTable)
                dataGridView4.Enabled = false;
        }

        private void DrawNewWrodsF()
        {
            var p = Imovirnist.GetBigPForNewWords(pwf.ListOfNewWords);
            for (int i = 0; i < p.Count; i++)
            {
                chart15.Series[0].Points.AddXY(i, p[i]);
                if (Config.ShowNewWordsTable)
                    dataGridView9.Rows.Add(i, pwf.ListOfNewWords[i].GetInt(), p[i]);
            }
            var disp = Mathematic.Dispersia(p.ToArray());
            if (!Config.ShowNewWordsTable)
                dataGridView9.Enabled = false;
        }


        private void DrawNewWrodsSW()
        {
            var p = Imovirnist.GetBigPForNewWords(pws.ListOfNewWords);
            for (int i = 0; i < p.Count; i++)
            {
                chart23.Series[0].Points.AddXY(i, p[i]);
                if (Config.ShowNewWordsTable)
                    dataGridView14.Rows.Add(i, pws.ListOfNewWords[i].GetInt(), p[i]);
            }
            if (!Config.ShowNewWordsTable)
                dataGridView14.Enabled = false;
        }

        private void DrawImovirn(ParseWords pw)
        {
            Imovirnist imovirnist = new Imovirnist(pw.newWords.ToArray(), pw.CountOfWords);
            var LitelP = Imovirnist.GetLitelP(pw.ListOfNewWords);
            var BigP = Imovirnist.GetBigP(LitelP);

            List<double> listDivSum = new List<double>();
            List<double> listLP = new List<double>();

            double sum = Imovirnist.GetSum(LitelP);

            dti_litelP = LitelP;
            dti_bigP = BigP;

            for (int i = 0; i < LitelP.Count; i++)
            {
                dataGridView2.Rows.Add(LitelP[i].First, LitelP[i].Second,
                    LitelP[i].Second / sum, BigP[i]);
                listDivSum.Add(LitelP[i].Second / sum);
                listLP.Add(LitelP[i].Second);
                chart3.Series[0].Points.AddXY(LitelP[i].First, BigP[i]);
                chart2.Series[0].Points.AddXY(LitelP[i].First, LitelP[i].Second);

            }


            DrawTi(pw.ListOfNewWords);
        }

        private void DrawImovirnNG(ParseWords pw)
        {
            Imovirnist imovirnist = new Imovirnist(pw.newWords.ToArray(), pw.CountOfWords);
            var LitelP = Imovirnist.GetLitelP(pw.ListOfNewWords);
            var BigP = Imovirnist.GetBigP(LitelP);

            List<double> listDivSum = new List<double>();
            List<double> listLP = new List<double>();

            double sum = Imovirnist.GetSum(LitelP);

            dti_litelP = LitelP;
            dti_bigP = BigP;

            for (int i = 0; i < LitelP.Count; i++)
            {
                dataGridView22.Rows.Add(LitelP[i].First, LitelP[i].Second,
                    LitelP[i].Second / sum, BigP[i]);
                listDivSum.Add(LitelP[i].Second / sum);
                listLP.Add(LitelP[i].Second);
                chart35.Series[0].Points.AddXY(LitelP[i].First, BigP[i]);
                chart34.Series[0].Points.AddXY(LitelP[i].First, LitelP[i].Second);
            }


            DrawTiNG(pw.ListOfNewWords);
        }

        private void DrawImovirnN(ParseWords pw)
        {
            var LitelP = Imovirnist.GetLitelP(pw.ListOfNewWords);
            var BigP = Imovirnist.GetBigP(LitelP);

            List<double> listDivSum = new List<double>();
            List<double> listLP = new List<double>();

            double sum = Imovirnist.GetSum(LitelP);

            dti_litelP = LitelP;
            dti_bigP = BigP;


            for (int i = 0; i < LitelP.Count; i++)
            {
                dataGridView17.Rows.Add(LitelP[i].First, LitelP[i].Second,
                    LitelP[i].Second / sum, BigP[i]);
                listDivSum.Add(LitelP[i].Second / sum);
                listLP.Add(LitelP[i].Second);
                chart27.Series[0].Points.AddXY(LitelP[i].First, BigP[i]);
                chart26.Series[0].Points.AddXY(LitelP[i].First, LitelP[i].Second);

            }

            DrawTiN(pwSN.ListOfNewWords);
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

            for (int i = 0; i < LitelP.Count; i++)
            {
                dataGridView7.Rows.Add(LitelP[i].First, LitelP[i].Second,
                    LitelP[i].Second / sum, BigP[i]);
                listDivSum.Add(LitelP[i].Second / sum);
                listLP.Add(LitelP[i].Second);
                chart11.Series[0].Points.AddXY(LitelP[i].First, BigP[i]);
                chart10.Series[0].Points.AddXY(LitelP[i].First, LitelP[i].Second);

            }


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


            for (int i = 0; i < LitelP.Count; i++)
            {
                dataGridView12.Rows.Add(LitelP[i].First, LitelP[i].Second,
                    LitelP[i].Second / sum, BigP[i]);
                listDivSum.Add(LitelP[i].Second / sum);
                listLP.Add(LitelP[i].Second);
                chart19.Series[0].Points.AddXY(LitelP[i].First, BigP[i]);
                chart18.Series[0].Points.AddXY(LitelP[i].First, LitelP[i].Second);

            }


            DrawTiSW(pws.ListOfNewWords);
        }

        private void DrawTi(List<bool> ls)
        {
            int countOfPart = int.Parse(textBox1.Text);
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
                chart6.Series[0].Points.AddXY(i, res[i]);
                chart6.Series[0].Points[i].Label = ((int)p[i].Second).ToString();
                chart4.Series[0].Points.AddXY(litelP[i].Item1, litelP[i].Item2);
                chart5.Series[0].Points.AddXY(i, bigP[i]);
                dataGridView3.Rows.Add(i, p[i].Second, average, litelP[i].Item2, bigP[i]);
                listAverages.Add((int)average);
                average += size;
            }

        }

        private void DrawTiNG(List<bool> ls)
        {
            int countOfPart = int.Parse(textBox27.Text);
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
                chart36.Series[0].Points.AddXY(i, res[i]);
                chart36.Series[0].Points[i].Label = ((int)p[i].Second).ToString();
                chart37.Series[0].Points.AddXY(litelP[i].Item1, litelP[i].Item2);
                chart38.Series[0].Points.AddXY(i, bigP[i]);
                dataGridView23.Rows.Add(i, p[i].Second, average, litelP[i].Item2, bigP[i]);
                listAverages.Add((int)average);
                average += size;
            }

        }

        private void DrawTiN(List<bool> ls)
        {
            int countOfPart = int.Parse(textBox18.Text);
            double size;
            var res = Imovirnist.GetCouOfPart(ls, countOfPart, out size);
            var p = res.GetTwoIntPair();
            var litelP = Imovirnist.GetLitelPWithDouble(p);
            var bigP = Imovirnist.GetBigP(p);
            List<int> listAverages = new List<int>();

            double average = size / 2;
            for (int i = 0; i < res.Count; i++)
            {
                chart28.Series[0].Points.AddXY(i, res[i]);
                chart28.Series[0].Points[i].Label = ((int)p[i].Second).ToString();
                chart29.Series[0].Points.AddXY(litelP[i].Item1, litelP[i].Item2);
                chart30.Series[0].Points.AddXY(i, bigP[i]);
                dataGridView18.Rows.Add(i, p[i].Second, average, litelP[i].Item2, bigP[i]);
                listAverages.Add((int)average);
                average += size;
            }

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
                chart12.Series[0].Points.AddXY(i, res[i]);
                chart12.Series[0].Points[i].Label = ((int)average).ToString();
                chart13.Series[0].Points.AddXY(litelP[i].Item1, litelP[i].Item2);
                chart14.Series[0].Points.AddXY(i, bigP[i]);
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
                chart20.Series[0].Points.AddXY(i, res[i]);
                chart20.Series[0].Points[i].Label = p[i].Second.ToString(); //((int)average).ToString();
                chart21.Series[0].Points.AddXY(litelP[i].Item1, litelP[i].Item2);
                chart22.Series[0].Points.AddXY(i, bigP[i]);
                dataGridView13.Rows.Add(i, p[i].Second, average, litelP[i].Item2, bigP[i]);
                listAverages.Add((int)average);
                average += size;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(Config.NotFileMessage);
                return;
            }
            if (!File.Exists(openFileDialog1.FileName))
            {
                MessageBox.Show(Config.NotFileMessage);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button16.Enabled = false;
            Config.RefreshStopChars(); 
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

        private void removeDirectory(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string folder = folderBrowserDialog1.SelectedPath;




            string pathNW = string.Format("{0}/{1}", folder, "NewWord");
            string pathF = string.Format("{0}/{1}", folder, "F");
            string pathSW = string.Format("{0}/{1}", folder, "SW");
            string pathSNG = string.Format("{0}/{1}", folder, "SelectedNGram");
            string pathSG = string.Format("{0}/{1}", folder, "NGram");


            removeDirectory(pathNW);
            removeDirectory(pathF);
            removeDirectory(pathSW);
            removeDirectory(pathSNG);
            removeDirectory(pathSG);

            Directory.CreateDirectory(pathNW);
            Directory.CreateDirectory(pathF);
            Directory.CreateDirectory(pathSW);
            Directory.CreateDirectory(pathSNG);
            Directory.CreateDirectory(pathSG);


            DataTableToCSV(GetDataTableFromDGV(dataGridView1), pathNW + "/AnalysisText.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView2), pathNW + "/dTi.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView3), pathNW + "/Ti.tsv");
            NewWordsToFile(pw, pathNW + "/New Words.tsv");
            //DataTableToCSV(GetDataTableFromDGV(dataGridView4), pathNW + "/New Words.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView5), pathNW + "/F.tsv");

            DataTableToCSV(GetDataTableFromDGV(dataGridView6), pathF + "/AnalysisText.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView7), pathF + "/dTi.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView8), pathF + "/Ti.tsv");
            NewWordsToFile(pwf, pathF + "/New Words.tsv");
            //DataTableToCSV(GetDataTableFromDGV(dataGridView9), pathF + "/New Words.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView10), pathF + "/F.tsv");


            DataTableToCSV(GetDataTableFromDGV(dataGridView11), pathSW + "/AnalysisText.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView12), pathSW + "/dTi.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView13), pathSW + "/Ti.tsv");
            NewWordsToFile(pws, pathSW + "/New Words.tsv");
            //DataTableToCSV(GetDataTableFromDGV(dataGridView14), pathSW + "/New Words.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView15), pathSW + "/F.tsv");

            DataTableToCSV(GetDataTableFromDGV(dataGridView16), pathSNG + "/AnalysisText.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView17), pathSNG + "/dTi.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView18), pathSNG + "/Ti.tsv");
            NewWordsToFile(pwSN, pathSNG + "/New Words.tsv");
            //DataTableToCSV(GetDataTableFromDGV(dataGridView4), pathNW + "/New Words.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView20), pathSNG + "/F.tsv");


            DataTableToCSV(GetDataTableFromDGV(dataGridView21), pathSG + "/AnalysisText.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView22), pathSG + "/dTi.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView23), pathSG + "/Ti.tsv");
            NewWordsToFile(pwN, pathSG + "/New Words.tsv"); // pathSG
            //DataTableToCSV(GetDataTableFromDGV(dataGridView4), pathNW + "/New Words.tsv");
            DataTableToCSV(GetDataTableFromDGV(dataGridView25), pathSG + "/F.tsv");


            //  SaveTextAn(folder+"/AnalysisText.txt"); 
            //SaveDTI(folder+"/dTI.txt");
            //SaveTI(folder + "/TI.txt"); 
            //SaveNewWords(folder + "/NewWord.txt");

            MessageBox.Show("Saved");
        }

        private void SaveTextAn(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("Wrod\tCount\tF");
                foreach (var item in dictAnlysText)
                {
                    sw.WriteLine(String.Format("{0}\t{1}\t{2}", item.Key, item.Value.Count, item.Value.Frequecy));
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
                        dti_litelP[i].First, dti_litelP[i].Second, p, dti_bigP[i]);
                }
            }
        }

        //private void SaveTI(string path)
        //{
        //    using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8))
        //    {
        //        var sum = Imovirnist.GetSum(dti_litelP);
        //        sw.WriteLine("X\tN\tAverage\tp(x)\tP(x)");
        //        for (int i = 0; i < ti_litelP.Count; i++)
        //        {
        //            double p = (double)dti_litelP[i].Second / (double)sum;
        //            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",
        //                i, ti_litelP[i].Item1, ti_average[i], ti_litelP[i].Item2, ti_bigP[i]);
        //        }
        //    }
        //}

        private void SaveNewWords(string path)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("Possition\tValue\tP(x)");
                for (int i = 0; i < pw.ListOfNewWords.Count; i++)
                {
                    sw.WriteLine(String.Format("{0}\t{1}\t{2}", i, pw.ListOfNewWords[i].GetInt(), newWords_P[i]));
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

            var list = p1.GetResultF();

            for (int i = 0; i < list.Item1.Count; i++)
            {
                dataGridView5.Rows.Add(list.Item1[i], list.Item2[i], list.Item3[i]);
                chart8.Series[0].Points.AddXY(list.Item1[i], list.Item3[i]);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(openFileDialog1.FileName))
                {
                    MessageBox.Show(Config.NotFileMessage);
                    return;
                }
                var cf = new FormConf();
                cf.ShowDialog();
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

                ParseWords parseWords = new ParseWords(openFileDialog1.FileName);
                parseWords.DoResult();
                var dictAnlysText = parseWords.GetWordsDictinary;


                int f = int.Parse(textBox11.Text);
                ParseWordForF pf = new ParseWordForF(openFileDialog1.FileName, f, dictAnlysText);

                this.pwf = pf;
                pwf.DoResult();


                DrawF(dictAnlysText, pwf.CountOfWords, f);
                DrawImovirnF(pwf);
                DrawNewWrodsF();
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                chart16.Series[0].Points.AddXY(list.Item1[i], list.Item3[i]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(openFileDialog1.FileName))
                {
                    MessageBox.Show(Config.NotFileMessage);
                    return;
                }
                var cf = new FormConf();
                cf.ShowDialog();
                string w = textBox12.Text;
                ParseWordSelectedWord pf = new ParseWordSelectedWord(openFileDialog1.FileName, w);
                pf.DoResult();
                if (!pf.ListOfNewWords.Contains(true))
                {
                    MessageBox.Show("Такого слова немає в тексті!");
                    return;
                }
                this.pws = pf;

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

                int count = 0;
                pf.ListOfNewWords.ForEach(x => { if (x) count++; });

                DrawSW(w, count);
                DrawImovirnSW(pws);
                DrawNewWrodsSW();
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView15.Rows.Clear();
            chart24.Series[0].Points.Clear();
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
            try
            {
                chart12.Series[0].Points.Clear();
                chart13.Series[0].Points.Clear();
                chart14.Series[0].Points.Clear();
                dataGridView8.Rows.Clear();
                DrawTiF(pwf.ListOfNewWords);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(openFileDialog1.FileName))
                {
                    MessageBox.Show(Config.NotFileMessage);
                    return;
                }
                string way = openFileDialog1.FileName;

                var cf = new FormConf();
                cf.ShowDialog();
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


                ParseWords pw = new ParseWords(way);
                this.pw = pw;
                pw.DoResult();
                Dictionary<string, ResearchElement> dict = pw.GetWordsDictinary;

                button2_Click(sender, e);
                //button4_Click(sender, e);
                //button5_Click(sender, e);
                dictAnlysText = dict;
                Draw(dict, pw.CountOfWords);
                DrawImovirn(pw);
                DrawNewWrods();
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(openFileDialog1.FileName))
                {
                    MessageBox.Show(Config.NotFileMessage);
                    return;
                }
                string way = openFileDialog1.FileName;


                chart25.Series[0].Points.Clear();
                chart26.Series[0].Points.Clear();
                chart27.Series[0].Points.Clear();
                chart29.Series[0].Points.Clear();
                chart30.Series[0].Points.Clear();
                chart28.Series[0].Points.Clear();
                chart31.Series[0].Points.Clear();

                dataGridView16.Rows.Clear();
                dataGridView17.Rows.Clear();
                dataGridView18.Rows.Clear();
                dataGridView19.Rows.Clear();

                int size;
                if (!int.TryParse(textBox23.Text, out size))
                {
                    MessageBox.Show("Розмір нграми повинен бути цілим чслом");
                    return;
                }

                if (textBox24.Text.Length != size)
                {
                    MessageBox.Show("Розмір нграми повинен бути рівний ввденому значенню розміру нграим!");
                    return;
                }
                var cf = new FormConf();
                cf.ShowDialog();

                ParseWordNGram pw = new ParseWordNGram(way, textBox24.Text, size);

                this.pwSN = pw;
                pw.DoResult();
                var count = pw.ListOfNewWords.Count(x => x);
                if (count <= 0)
                {
                    MessageBox.Show("Такої нграми немає у тексті");
                    return;
                }

                Dictionary<string, ResearchElement> dict = pw.GetWordsDictinary;

                button12_Click(sender, e);
                //button4_Click(sender, e);
                //button5_Click(sender, e);
                dictAnlysText = dict;
                DrawN(textBox24.Text, count);
                DrawImovirnN(pw);
                DrawNewWrodsN();
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dataGridView20.Rows.Clear();
            chart32.Series[0].Points.Clear();
            int c1 = int.Parse(textBox22.Text);
            int c2 = int.Parse(textBox21.Text);
            int c3 = int.Parse(textBox20.Text);
            int c4 = int.Parse(textBox19.Text);
            Part2 p1 = new Part2();

            p1.StartPlot = c1;
            p1.EndPlot = c2;
            p1.StepMove = c3;
            p1.StepGoBiger = c4;
            p1.list = pwSN.ListOfNewWords;

            var list = p1.GetResultF();

            //for (int i = 0; i < list.Item1.Count; i++)
            //{
            //    dataGridView20.Rows.Add(list.Item1[i], list.Item2[i]);
            //    chart32.Series[0].Points.AddXY(list.Item1[i], list.Item2[i]);
            //}

            for (int i = 0; i < list.Item1.Count; i++)
            {
                dataGridView20.Rows.Add(list.Item1[i], list.Item2[i], list.Item3[i]);
                chart32.Series[0].Points.AddXY(list.Item1[i], list.Item3[i]);
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                chart28.Series[0].Points.Clear();
                chart29.Series[0].Points.Clear();
                chart30.Series[0].Points.Clear();
                dataGridView18.Rows.Clear();
                DrawTiN(pwSN.ListOfNewWords);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            //int size = int.Parse(textBox23.Text);
            //if (textBox24.Text.Length != size)
            //{
            //    MessageBox.Show("Розмін нграми повинен бути рівний значеню введеному у полі"); 
            //}
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            int size;
            if (!int.TryParse(textBox23.Text, out size))
            {
                MessageBox.Show("Розмір нграми повинен бути цілим чслом");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(openFileDialog1.FileName))
                {
                    MessageBox.Show(Config.NotFileMessage);
                    return;
                }
                ParseWords pw;
                int size;
                if (!int.TryParse(textBox26.Text, out size))
                {
                    MessageBox.Show("Розмір нграми повинен бути цілим чслом");
                    return;
                }

                switch (comboBox1.SelectedIndex)
                {
                    case (2): pw = new ParseWordLatterNgram(openFileDialog1.FileName, size); break;
                    case (0): pw = new ParseWordNGramChars(openFileDialog1.FileName, size, true); break;
                    case (1): pw = new ParseWordNGramChars(openFileDialog1.FileName, size, false); break;
                    default: MessageBox.Show("Проблеми із вибором режиму роботи нграм"); ; return;
                }

                var cf = new FormConf();
                cf.ShowDialog();
                chart33.Series[0].Points.Clear();
                chart34.Series[0].Points.Clear();
                chart35.Series[0].Points.Clear();
                chart37.Series[0].Points.Clear();
                chart38.Series[0].Points.Clear();
                chart36.Series[0].Points.Clear();
                chart39.Series[0].Points.Clear();

                dataGridView21.Rows.Clear();
                dataGridView22.Rows.Clear();
                dataGridView23.Rows.Clear();
                dataGridView24.Rows.Clear();

                this.pwN = pw;
                pw.DoResult();
                Dictionary<string, ResearchElement> dict = pw.GetWordsDictinary;

                button15_Click(sender, e);
                //button4_Click(sender, e);
                //button5_Click(sender, e);
                dictAnlysText = dict;
                DrawNG(dict, pw.CountOfWords);
                DrawImovirnNG(pw);
                DrawNewWrodsNG();
                MessageBox.Show("Done");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 2)
                button16.Enabled = true;
            else
                button16.Enabled = false;

        }

        private void button15_Click(object sender, EventArgs e)
        {
            dataGridView25.Rows.Clear();
            chart40.Series[0].Points.Clear();
            int c1 = int.Parse(textBox31.Text);
            int c2 = int.Parse(textBox30.Text);
            int c3 = int.Parse(textBox29.Text);
            int c4 = int.Parse(textBox28.Text);
            Part2 p1 = new Part2();

            p1.StartPlot = c1;
            p1.EndPlot = c2;
            p1.StepMove = c3;
            p1.StepGoBiger = c4;
            p1.list = pwN.ListOfNewWords;

            var list = p1.GetResultF();

            for (int i = 0; i < list.Item1.Count; i++)
            {
                dataGridView25.Rows.Add(list.Item1[i], list.Item2[i], list.Item3[i]);
                chart40.Series[0].Points.AddXY(list.Item1[i], list.Item3[i]);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                chart36.Series[0].Points.Clear();
                chart37.Series[0].Points.Clear();
                chart38.Series[0].Points.Clear();
                dataGridView23.Rows.Clear();
                DrawTiNG(pwN.ListOfNewWords);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            StopSymbolsChracters ss = new StopSymbolsChracters();
            if (comboBox1.SelectedIndex == 2)
                ss.Path = Config.SSWord;
            else if (comboBox1.SelectedIndex == 0)
                ss.Path = Config.SSLatter;
            else
            {
                MessageBox.Show("System error");
                return; 
            }

            ss.ShowDialog();
            Config.RefreshStopChars(); 
        }
    }
}