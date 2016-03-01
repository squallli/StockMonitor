using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMonitor
{
    public class clsStockAnalyze
    {
        public string 股票代號 { set; get; }
        private string 前波最高價 { set; get; }

        public void Start()
        {
            clsStockInfo stockInfo = new clsStockInfo();
            stockInfo.股票代號 = 股票代號;
            stockInfo.OnPriceChange += stockInfo_OnPriceChange;
            System.Threading.Thread t = new System.Threading.Thread(stockInfo.getStockInfo);
            t.Start();
        }

        void stockInfo_OnPriceChange(clsStockInfo info)
        {
            string s = "";
        }
    }
}
