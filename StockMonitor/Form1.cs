using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clsStockAnalyze analyze = new clsStockAnalyze();
            analyze.股票代號 = "otc_5478";

            System.Threading.Thread t = new System.Threading.Thread(analyze.Start);
            t.Start();
        }
    }
}
