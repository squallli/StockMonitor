using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;


namespace StockMonitor
{
    public class clsStockInfo
    {
        public delegate void dOnPriceChange(clsStockInfo info);
        public event dOnPriceChange OnPriceChange;

        public bool isStop = false;
        public string 股票代號 { set; get; }
        public string 成交 { set; get; }
        public string 總量 { set; get; }
        public string 當盤成交量 { set; get; }
        public string 開盤 { set; get; }
        public string 當日最高 { set; get; }
        public string 當日最低 { set; get; }
        public string 日期 { set; get; }
        public string 時間 { set; get; }


        public static void WriteCookiesToDisk(string file)
        {
            using (Stream stream = File.Create(file))
            {
                try
                {
                    CookieContainer cookieJar = new CookieContainer();

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://mis.twse.com.tw");
                    request.CookieContainer = cookieJar;
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                    }

                    Console.Out.Write("Writing cookies to disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cookieJar);
                    Console.Out.WriteLine("Done.");
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Problem writing cookies to disk: " + e.GetType());
                }
            }
        }

        public static CookieContainer ReadCookiesFromDisk(string file)
        {
            try
            {
                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    Console.Out.Write("Reading cookies from disk... ");
                    BinaryFormatter formatter = new BinaryFormatter();
                    Console.Out.WriteLine("Done.");
                    return (CookieContainer)formatter.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Problem reading cookies from disk: " + e.GetType());
                return new CookieContainer();
            }
        }  

        public string getJsonString(string url)
        {
            try
            {
                CookieContainer cookieJar = ReadCookiesFromDisk("d:\\test");
                
              
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://mis.twse.com.tw/stock/api/getStockInfo.jsp?ex_ch=" + 股票代號 + ".tw&json=1&delay=0");
                req.Method = "GET";
                req.Host = "mis.twse.com.tw";
                req.CookieContainer = cookieJar;
                req.KeepAlive = true;
                using (WebResponse wr = req.GetResponse())
                {
                    using (StreamReader myStreamReader = new StreamReader(wr.GetResponseStream(), Encoding.UTF8))
                    {
                        return myStreamReader.ReadToEnd();

                    }
                }
                
            }
            catch
            {
            }
            

            return "";
            
        }
        public void getStockInfo()
        {

            WriteCookiesToDisk("D:\\test");
            while (!isStop)
            {
                try
                {
                    
                    JObject o = Newtonsoft.Json.Linq.JObject.Parse(getJsonString("http://mis.twse.com.tw/stock/api/getStockInfo.jsp?ex_ch="+股票代號+".tw&json=1&delay=0"));
                    if (OnPriceChange != null)
                    {
                        if (this.總量 != o["msgArray"][0]["v"].ToString())
                        {
                            this.股票代號 = o["msgArray"][0]["c"].ToString();
                            this.日期 = o["msgArray"][0]["d"].ToString();
                            this.成交 = o["msgArray"][0]["z"].ToString();
                            this.當盤成交量 = o["msgArray"][0]["tv"].ToString();
                            this.開盤 = o["msgArray"][0]["o"].ToString();
                            this.當日最低 = o["msgArray"][0]["l"].ToString();
                            this.當日最高 = o["msgArray"][0]["h"].ToString();
                            this.總量 = o["msgArray"][0]["v"].ToString();
                            this.時間 = o["msgArray"][0]["t"].ToString();

                            OnPriceChange(this);
                        }
                    }
                   
                }
                catch
                {
                }
                finally
                {
                    System.Threading.Thread.Sleep(3000);
                }
            } 
        }
    }
}
