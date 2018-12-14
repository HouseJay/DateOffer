using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBMAP
{
    class MySqlConnector
    {
        MySqlConnection mysqlcon;

        //连接本地Mysql
        public void connection()
        {
            string strLocal = "server=localhost;User Id=house;password=house;Database=cdsidb";
            mysqlcon = new MySqlConnection(strLocal);

        }

        public MySqlDataReader selData()
        {
            MySqlDataReader data = null;
            mysqlcon.Open();
            MySqlCommand mysqlcmd_query = new MySqlCommand("SELECT a.`药店ID`,a.`维护对象`,b.`地址`,a.`预定时间` FROM `yw运维工作台账记录` a,`xt药店信息yd` b WHERE `预定时间`>\"2018-1-24\" AND `维护处理人`LIKE\"%房豪%\" AND a.`药店ID` = b.`药店ID` "+
                                                           "UNION ALL "+
                                                           "SELECT a.`药店ID`, a.`维护对象`, b.address_detail, a.`预定时间` FROM `yw运维工作台账记录` a, testdb.t_terminal b WHERE `预定时间`> \"2018-1-24\" AND `维护处理人`LIKE\"%房豪%\" AND a.`药店ID` = b.terminal_code " +
                                                           "ORDER BY `预定时间`", mysqlcon);
            data = mysqlcmd_query.ExecuteReader();
            return data;
        }
        public void closeConnection()
        {
            mysqlcon.Close();
            mysqlcon.Dispose();
            GC.Collect();
        }

    }
}
