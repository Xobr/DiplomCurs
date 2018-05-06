using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DiplomKurs
{
    public partial class StopSymbolsChracters : Form
    {
        public StopSymbolsChracters()
        {
            InitializeComponent();
        }

        public string Path { get; set; }

        private void FillDG()
        {
            dataGridView25.Rows.Clear();
            Program.GetStringListFromFile(Path).ForEach(x => dataGridView25.Rows.Add(x));
        }

        private void StopSymbolsChracters_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            if (!File.Exists(Path))
            {
                File.Create(Path);
            }
            Program.GetStringListFromFile(Path).ForEach(x => dataGridView25.Rows.Add(x));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string vale = textBox1.Text;
            var ls = Program.GetStringListFromFile(Path);
            ls.Add(vale);
            Program.RevriteSymbols(ls, Path);
            MessageBox.Show("Done");
            FillDG();
        }

        private void dataGridView25_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var ls = Program.GetStringListFromFile(Path);
                ls.Remove(dataGridView25.SelectedCells[0].Value.ToString());
                Program.RevriteSymbols(ls, Path);
                MessageBox.Show("Done");
                FillDG();
                button2.Enabled = false;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void dataGridView25_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button2.Enabled = true;
        }
    }
}
