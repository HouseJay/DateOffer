using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBMAP
{
    class Completed
    {
        private string strDate;                                  //日期
        private List<string> strName;                            //名称
        private List<string> strAddress;                         //地址
        private List<string> strlocal;                           //坐标
        private List<double> douPrice;                           //价格
        private List<string> strPriceDetail;                     //价格明细

        public string StrDate { get => strDate; set => strDate = value; }
        public List<string> StrName { get => strName; set => strName = value; }
        public List<string> StrAddress { get => strAddress; set => strAddress = value; }
        public List<string> Strlocal { get => strlocal; set => strlocal = value; }
        public List<double> DouPrice { get => douPrice; set => douPrice = value; }
        public List<string> StrPriceDetail { get => strPriceDetail; set => strPriceDetail = value; }

        public void toString()
        {
            Console.WriteLine("Completed:"+strDate+";"+strName + ";" + strAddress + ";" + Strlocal + ";" + DouPrice + ";" + StrPriceDetail);
        }

        public Completed(string strDate , List<string> strName , List<string> strAddress)
        {
            this.StrDate = strDate;
            this.StrName = strName;
            this.strAddress = strAddress;
            this.strlocal = new List<string>();
            this.douPrice = new List<double>();
            this.strPriceDetail = new List<string>();
        }
    }
}
