using MySql.Data.MySqlClient;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEBMAP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Completed> allCompleted = new List<Completed>();

        //连接数据库，获取相关的数据，再按时间分开，当天开始位置为我家（磨子桥），结束位置是也是最后到我家，要以最近的方式排序，并用高德计算出出行的价格。
        //然后用excel。生成一个对应的excel。
        private void butAction_Click(object sender, EventArgs e)
        {

            //List<string> name = new List<string>();
            //List<string> address = new List<string>();
            //name.Add("家");
            //name.Add("点1");
            //name.Add("点1");
            //name.Add("点2");
            //name.Add("点2");
            //name.Add("家");
            //address.Add("成都市武侯区磨子桥");
            //address.Add("成都市龙泉驿区华信南路99号附近");
            //address.Add("成都市龙泉驿区华信南路99号附近");
            //address.Add("成都市武侯区益州大道1999号");
            //address.Add("成都市武侯区益州大道1999号");
            //address.Add("成都市武侯区磨子桥");
            //Completed completed = new Completed("2018-11-1",name,address);
            //allCompleted.Add(completed);

            MySqlConnector conn = new MySqlConnector();
            conn.connection();
            MySqlDataReader data = conn.selData();

            Completed comp = null;
            string date = string.Empty;
            List<string> liName = null;
            List<string> liAddress = null;
            data.Read();
            do
            {
                if (!data[3].ToString().Equals(date))
                {
                    if (date != string.Empty)
                    {
                        liName.Add("家");
                        liAddress.Add("成都市武侯区磨子桥");
                        comp = new Completed(date, liName, liAddress);
                        allCompleted.Add(comp);

                        comp = null;
                        liName = new List<string>();
                        liAddress = new List<string>();

                        date = data[3].ToString();
                        liName.Add("家");
                        liAddress.Add("成都市武侯区磨子桥");
                        liName.Add(data[1].ToString());
                        liAddress.Add(data[2].ToString());
                        liName.Add(data[1].ToString());
                        liAddress.Add(data[2].ToString());
                    }
                    else
                    {
                        date = data[3].ToString();
                        liName = new List<string>();
                        liAddress = new List<string>();
                        liName.Add("家");
                        liAddress.Add("成都市武侯区磨子桥");
                        liName.Add(data[1].ToString());
                        liAddress.Add(data[2].ToString());
                        liName.Add(data[1].ToString());
                        liAddress.Add(data[2].ToString());
                    }
                }
                else
                {
                    liName.Add(data[1].ToString());
                    liAddress.Add(data[2].ToString());
                    liName.Add(data[1].ToString());
                    liAddress.Add(data[2].ToString());
                }
            } while (data.Read());
            liName.Add("家");
            liAddress.Add("成都市武侯区磨子桥");
            comp = new Completed(date, liName, liAddress);
            allCompleted.Add(comp);
            //关闭
            conn.closeConnection();
            //获取坐标
            foreach(Completed com in allCompleted)
            {
                getCompletedLocal(com);
            }
            //排序。
            foreach (Completed com in allCompleted)
            {
                rank(com);
            }

            //计算路费
            foreach (Completed com in allCompleted)
            {
                AddGold(com);
            }

            //MessageBox.Show("第一个点："+completed.StrName[0]+";对应的地址："+completed.StrAddress[0]+";对应的坐标："+completed.Strlocal[0]+";对应的价格："+ completed.DouPrice[0] + ";对应的路线：" + completed.StrPriceDetail[0] + "\n" +
            //    "第二个点：" + completed.StrName[1] + ";对应的地址：" + completed.StrAddress[1] + ";对应的坐标：" + completed.Strlocal[1] + ";对应的价格：" + completed.DouPrice[1] + ";对应的路线：" + completed.StrPriceDetail[1] + "\n" +
            //    "第三个点：" + completed.StrName[2] + ";对应的地址：" + completed.StrAddress[2] + ";对应的坐标：" + completed.Strlocal[2] + ";对应的价格：" + completed.DouPrice[2] + ";对应的路线：" + completed.StrPriceDetail[2] + "\n" +
            //    "第四个点：" + completed.StrName[3] + ";对应的地址：" + completed.StrAddress[3] + ";对应的坐标：" + completed.Strlocal[3] + ";对应的价格：" + completed.DouPrice[3] + ";对应的路线：" + completed.StrPriceDetail[3] + "\n" +
            //    "第五个点：" + completed.StrName[4] + ";对应的地址：" + completed.StrAddress[4] + ";对应的坐标：" + completed.Strlocal[4] + ";对应的价格：" + completed.DouPrice[4] + ";对应的路线：" + completed.StrPriceDetail[4] + "\n" +
            //    "第六个点：" + completed.StrName[5] + ";对应的地址：" + completed.StrAddress[5] + ";对应的坐标：" + completed.Strlocal[5] + ";对应的价格：" + completed.DouPrice[5] + ";对应的路线：" + completed.StrPriceDetail[5] 
            //    );
            //写入excel表
            createExcel();
        }
        string key = "559a8bdfe351bf02c250f9f6fd2abfa9";

        //获取地理位置
        private Completed getCompletedLocal(Completed completed)
        {
            string rember = string.Empty;
            for(int i = 0; i < completed.StrAddress.Count; i++)
            {
                string address = completed.StrAddress[i];
                if (rember.Equals(address))
                {
                    completed.Strlocal.Add(completed.Strlocal[completed.Strlocal.Count-1]);
                    continue;
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://restapi.amap.com/v3/geocode/geo?key="+key+"&address="+address);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string strJsonLocal = myStreamReader.ReadToEnd();
                //字符串截取
                int index01 = strJsonLocal.IndexOf("location");
                if(index01 == -1)
                {
                    completed.Strlocal.Add("");
                }
                else
                {
                    strJsonLocal = strJsonLocal.Substring(index01);
                    int index02 = strJsonLocal.IndexOf(",\"level\"");
                    strJsonLocal = strJsonLocal.Remove(index02);
                    strJsonLocal = strJsonLocal.Substring(11);
                    strJsonLocal = strJsonLocal.Remove(strJsonLocal.Length - 1);
                    completed.Strlocal.Add(strJsonLocal);
                }
                myStreamReader.Close();
                myResponseStream.Close();
                rember = address;
            }
            return completed;
        }

        private void createExcel()
        {
            int intRow = 1;//记录已用的行数
            //生成一个对应的excel
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("sheet1");
            IRow row = sheet.CreateRow(0);
            row.HeightInPoints = 25;
            ICell cell = row.CreateCell(0);
            cell.SetCellValue("日期");

            cell = row.CreateCell(1);
            cell.SetCellValue("出发地点");

            cell = row.CreateCell(2);
            cell.SetCellValue("到达地点");

            cell = row.CreateCell(3);
            cell.SetCellValue("车费");

            cell = row.CreateCell(4);
            cell.SetCellValue("车费明细");

            sheet.SetColumnWidth(0, 20 * 256);
            sheet.SetColumnWidth(1, 60 * 256);
            sheet.SetColumnWidth(2, 60 * 256);
            sheet.SetColumnWidth(3, 10 * 256);
            sheet.SetColumnWidth(4, 60 * 256);

            IRow row1 = null;

            //写入相应的数据。
            foreach (Completed com in allCompleted)
            {
                for (int i = 0; i < com.StrName.Count; i++)
                {
                    
                    if (i % 2 == 0)
                    {
                        row1 = sheet.CreateRow(intRow);
                        intRow++;
                        ICell cherCell = row1.CreateCell(1);
                        cherCell.SetCellValue(com.StrAddress[i]);
                    }
                    else
                    {
                        ICell cherCell = row1.CreateCell(2);
                        cherCell.SetCellValue(com.StrAddress[i]);
                        cherCell = row1.CreateCell(3);
                        cherCell.SetCellValue(com.DouPrice[i]);
                        cherCell = row1.CreateCell(4);
                        cherCell.SetCellValue(com.StrPriceDetail[i]);
                        row1 = null;
                        cherCell = null;
                    }
                    if (i == 0)
                    {
                        row1.CreateCell(0).SetCellValue(com.StrDate + "");
                    }

                }
            }
            FileStream sw = File.Create("E:\\Myself\\" + "日报.xls");

            book.Write(sw);
            sw.Close();
            XSSFWorkbook book2 = new XSSFWorkbook();
        }
        /// <summary>
        /// 计算两点之间的距离,
        /// </summary>
        /// <param name="lat1">第一个点的纬度</param>
        /// <param name="lng1">第一个点的经度</param>
        /// <param name="lat2">第二个点的纬度</param>
        /// <param name="lng2">第二个点的经度</param>
        private double GetDistance(double lat1 , double lng1 , double lat2, double lng2)
        {
            double radLat1 = douRad(lat1);
            double radLng1 = douRad(lng1);
            double radLat2 = douRad(lat2);
            double radLng2 = douRad(lng2);

            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * 6378137;

            return result;
        }
        private double douRad(double d)
        {
            return (double)d * Math.PI /180d;
        }
        /// <summary>
        /// 计算completed中两组点的距离。
        /// </summary>
        /// <param name="completed"></param>
        /// <param name="frist"></param>
        /// <param name="second"></param>
        /// <returns>返回两点之间的距离</returns>
        private double getDistance(Completed completed,int frist , int second)
        {
            if(completed.Strlocal[frist] == "" || completed.Strlocal[second] == "")
            {
                return 1d;
            }
            int index = completed.Strlocal[frist].IndexOf(",");
            string strlng1 = completed.Strlocal[frist].Remove(index);
            string strlat1 = completed.Strlocal[frist].Substring(index + 1);

            double lat1 = Double.Parse(strlat1);
            double lng1 = Double.Parse(strlng1);

            int index2 = completed.Strlocal[second].IndexOf(",");
            string strlng2 = completed.Strlocal[second].Remove(index2);
            string strlat2 = completed.Strlocal[second].Substring(index2 + 1);

            double lat2 = Double.Parse(strlat2);
            double lng2 = Double.Parse(strlng2);

            double result = GetDistance(lat1, lng1, lat2, lng2);
            return result;
        }

        private Completed exchange(Completed completed,int frist,int second)
        {
            Completed comTemporary = new Completed(completed.StrDate, new List<string>(), new List<string>());
            comTemporary.StrName.Add(completed.StrName[frist]);
            comTemporary.StrAddress.Add(completed.StrAddress[frist]);
            comTemporary.Strlocal.Add(completed.Strlocal[frist]);

            completed.StrName[frist] = completed.StrName[second];
            completed.StrAddress[frist] = completed.StrAddress[second];
            completed.Strlocal[frist] = completed.Strlocal[second];

            completed.StrName[second] = comTemporary.StrName[0];
            completed.StrAddress[second] = comTemporary.StrAddress[0];
            completed.Strlocal[second] = comTemporary.Strlocal[0];
            return completed;
        }
        /// <summary>
        /// 给Completed排序
        /// </summary>
        /// <param name="completed">放入completed排序</param>
        /// <returns>返回已经排序好的Completed</returns>
        private Completed rank(Completed completed)
        {
            for (int i = 0; i < completed.StrAddress.Count - 1; i++)
            {
                double recent = Double.MaxValue;
                int record = 0;
                for (int x = i + 1; x < completed.StrAddress.Count - 1; x++)
                {
                    double douRec = getDistance(completed, i, x);
                    if (recent > douRec)
                    {
                        recent = douRec;
                        record = x;
                    }
                }
                if (record != 0 && record != i + 1)
                {
                    completed = exchange(completed, i + 1, record);
                    completed = exchange(completed, record+1, i+2);
                    i++;
                }
            }
            return completed;
        }

        private Completed AddGold(Completed completed)
        {
            foreach(string str in completed.StrAddress)
            {
                completed.DouPrice.Add(1d);
                completed.StrPriceDetail.Add("");
            }

            for(int i = completed.StrAddress.Count -1 ; i >= 1; i--)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://restapi.amap.com/v3/direction/transit/integrated?key=" + key + "&origin="+ completed.Strlocal[i-1]+ "&destination="+completed.Strlocal[i]+ "&city=成都市");
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string strJsonPrice = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                string status = truncate(strJsonPrice, "status", 1);
                if(int.Parse(status) != 1)
                {
                    continue;
                }
                string strPrice = "0";
                string strPriceDetail = "";

                string count = truncate(strJsonPrice, "count", "route");
                string strTemporary = strJsonPrice;

                strTemporary = mySubString(strTemporary, "transits");
                for (int x = 0;x < int.Parse(count); x++)
                {
                    string price = truncate(strTemporary, "cost", "duration");
                    if(price == "")
                    {
                        continue;
                    }
                    if(double.Parse(strPrice) < double.Parse(price))
                    {
                        strPrice = price;
                        string segments = truncate(strTemporary, "segments", "cost");
                        strPriceDetail = getBus(segments);
                    }
                    if (x != int.Parse(count) - 1)
                    {
                        strTemporary = mySubString(strTemporary, "cost");
                    }
                }
                completed.DouPrice[i] = double.Parse(strPrice);
                completed.StrPriceDetail[i] = strPriceDetail;
                i--;
            }
            return completed;
        }
        /// <summary>
        /// json数据字段值截取。
        /// </summary>
        /// <param name="primary">原json数据</param>
        /// <param name="value">取值key</param>
        /// <param name="length">取值长度</param>
        /// <returns>已经取的值</returns>
        private string truncate(string primary,string value,int length)
        {
            int index = primary.IndexOf(value);
            if(index == -1)
            {
                return primary;
            }
            primary = primary.Substring(index);
            primary = primary.Remove(value.Length+3+length);
            primary = primary.Substring(value.Length + 3);
            return primary;
        }
        /// <summary>
        /// json数据字段值截取。
        /// </summary>
        /// <param name="primary">原json数据</param>
        /// <param name="value">取值key</param>
        /// <param name="nextValue">下一个取值key</param>
        /// <returns>已经取的值</returns>
        private string truncate(string primary, string value,string nextValue)
        {
            int index = primary.IndexOf(value);
            if (index == -1)
            {
                return primary;
            }
            primary = primary.Substring(index);
            int index2 = primary.IndexOf(nextValue);
            if (index2 == -1)
            {
                return primary;
            }
            primary = primary.Remove(index2-3);
            primary = primary.Substring(value.Length + 3);
            return primary;
        }
        /// <summary>
        /// 取公交名称专用
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="value"></param>
        /// <param name="nextValue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string getBus(string primary)
        {
            string redPrimary = primary;
            string roed = string.Empty;
            do
            {
                int index = primary.IndexOf("bus\"");
                primary = primary.Substring(index);
                index = primary.IndexOf("start_time");
                if(index == -1)
                {
                    redPrimary = mySubString(redPrimary, "bus\"");
                    continue;
                }
                primary = primary.Remove(index);

                string startStand = truncate(primary, "name", "id");
                primary =mySubString(primary, "name");
                string stopStand = truncate(primary, "name", "id");
                primary = mySubString(primary, "name");

                if(roed != string.Empty)
                {
                    string bus = truncate(primary, "name", "id");
                    int index1 = bus.IndexOf("(");
                    if(index1 != -1)
                    {
                        bus = bus.Remove(index1);
                    }
                    roed += bus + "("+startStand+"--"+stopStand+")";
                }
                else
                {
                    string bus = truncate(primary, "name", "id");
                    int index1 = bus.IndexOf("(");
                    if (index1 != -1)
                    {
                        bus = bus.Remove(index1);
                    }
                    roed = bus + "(" + startStand + "--" + stopStand + ")";
                }
                redPrimary = mySubString(redPrimary, "bus\"");
                primary = redPrimary;
            } while (redPrimary.IndexOf("bus\"") != -1);
            return roed;
        }

        private string mySubString(string primary,string value)
        {
            int index = primary.IndexOf(value);
            primary = primary.Substring(index + value.Length);
            index = primary.IndexOf(value);
            if(index != -1)
            {
                primary = primary.Substring(index);
            }
            return primary;
        }
    }
}
