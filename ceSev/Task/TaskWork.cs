using ceSev.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ceSev.Task
{
    public class TaskWork
    {
        private SqlConnectionStringBuilder _builder;
        private List<TaskMasterModel> tasklist;
        public String xmlData;

        public TaskWork()
        {
            InitDbSet();
            //var ddd = "";
            //ddd = GetData();
        }

        private void InitDbSet()
        {
            _builder = new SqlConnectionStringBuilder();
            //_builder.DataSource = "cedevdb.database.windows.net";
            //_builder.UserID = "cedevadmin";
            //_builder.Password = "!1ge1046417";
            //_builder.InitialCatalog = "cedevdb";

            _builder.DataSource = @"DESKTOP-B7FUQ04\SQLEXPRESS";
            _builder.UserID = "sa";
            _builder.Password = "!1ge1046417";
            _builder.InitialCatalog = "cedevdb";
        }

        public void CreteDataTryHis()
        {

            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {

                connection.Open();
                StringBuilder sb = new StringBuilder();
                SqlCommand cmd;

                String sql = sb.ToString();

                sb = new StringBuilder();

                string _kind = "KOREA_ECONOMIC_IDX";
                string _name = "한국경제지표";

                string _date = System.DateTime.Now.Year.ToString()
                             + string.Format("{0:D2}", System.DateTime.Now.Month)
                             + string.Format("{0:D2}", System.DateTime.Now.Day)
                             + string.Format("{0:D2}", System.DateTime.Now.Hour);



                sb.Append("INSERT INTO TaskTryHis  ");
                sb.Append("(  ");
                sb.Append("KIND  ");
                sb.Append(",NAME  ");
                sb.Append(",TRY_DATE  ");

                sb.Append(")  ");
                sb.Append("VALUES ");
                sb.Append("(  ");
                sb.Append("'" + _kind + "',");
                sb.Append("'" + _name + "', ");
                sb.Append("'" + _date + "' ");
                sb.Append(")  ");

                sql = sb.ToString();

                cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

            }
        }

        public void CreteDataWorkHis()
        {
            int workCnt = GetWorkCnt();

            if (workCnt != 0)
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {

                connection.Open();
                StringBuilder sb = new StringBuilder();
                SqlCommand cmd;

                String sql = sb.ToString();

                sb = new StringBuilder();

                string _kind = "KOREA_ECONOMIC_IDX";
                string _name = "한국경제지표";
                string _date = System.DateTime.Now.Year.ToString()
                                + string.Format("{0:D2}", System.DateTime.Now.Month)
                                + string.Format("{0:D2}", System.DateTime.Now.Day);

                sb.Append("INSERT INTO TASKMASTER  ");
                sb.Append("(  ");
                sb.Append("KIND  ");
                sb.Append(",NAME  ");
                sb.Append(",WORK_DATE  ");

                sb.Append(")  ");
                sb.Append("VALUES ");
                sb.Append("(  ");
                sb.Append("'" + _kind + "',");
                sb.Append("'" + _name + "', ");
                sb.Append("'" + _date + "' ");
                sb.Append(")  ");

                sql = sb.ToString();

                cmd = new SqlCommand(sql, connection);
                cmd.ExecuteNonQuery();

            }

        }

        public void CreteData()
        {
            int workCnt = GetWorkCnt();

            if (workCnt != 0)
            {
                return;
            }

            CreteDataWorkHis();


            var newData = GetData();

            if (newData == null)
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                connection.Open();
                StringBuilder sb = new StringBuilder();
                SqlCommand cmd;
                String sql = sb.ToString();

                foreach (var item in newData)
                {
                    sb = new StringBuilder();

                    sb.Append("INSERT INTO ECONOMICIDX  ");
                    sb.Append("(  ");
                    sb.Append("ID  ");
                    sb.Append(",CLASS_NAME  ");
                    sb.Append(",KEYSTAT_NAME  ");
                    sb.Append(",DATA_VALUE  ");
                    sb.Append(",CYCLE  ");
                    sb.Append(",UNIT_NAME  ");
                    sb.Append(",CREATE_DATE  ");
                    sb.Append(")  ");
                    sb.Append("VALUES ");
                    sb.Append("(  ");
                    sb.Append("" + item.ID + ",");
                    sb.Append("'" + item.CLASS_NAME + "', ");
                    sb.Append("'" + item.KEYSTAT_NAME + "', ");
                    sb.Append("'" + item.DATA_VALUE + "', ");
                    sb.Append("'" + item.CYCLE + "', ");
                    sb.Append("'" + item.UNIT_NAME + "', ");
                    sb.Append("'" + item.CREATE_DATE + "' ");
                    sb.Append(")  ");

                    sql = sb.ToString();

                    cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        private List<EconomicIdxItem> GetData()
        {
            List<EconomicIdxItem> items = new List<EconomicIdxItem>();

            //CSYPZ37CMH7K4J9XVALS
            //서비스명 text    Y
            //인증키 text Y
            //요청타입 text    Y
            //언어  text Y
            //요청시작건수 text    Y
            //요청종료건수  text Y

            //한국 경제지표
            String strUrl = "http://ecos.bok.or.kr/api/KeyStatisticList/CSYPZ37CMH7K4J9XVALS/xml/kr/1/100/";
            //String strUrl = "http://ecos.bok.or.kr/api/KeyStatisticList/CSYPZ37CMH7K4J9XVALS/json/kr/1/100/";


            WebRequest request = HttpWebRequest.Create(strUrl);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String result = reader.ReadToEnd();
            xmlData = result.ToString();

            XDocument doc = XDocument.Parse(xmlData);

            if (doc.Root != null)
            {
                string _createDate = System.DateTime.Now.Year.ToString()
                                  + string.Format("{0:D2}", System.DateTime.Now.Month)
                                  + string.Format("{0:D2}", System.DateTime.Now.Day);


                items = (from r in doc.Root.Elements("row")
                         select new EconomicIdxItem
                         {
                             CLASS_NAME = (string)r.Element("CLASS_NAME"),
                             KEYSTAT_NAME = (string)r.Element("KEYSTAT_NAME"),
                             DATA_VALUE = (string)r.Element("DATA_VALUE"),
                             CYCLE = (string)r.Element("CYCLE"),
                             UNIT_NAME = (string)r.Element("UNIT_NAME"),
                             //CREATE_DATE = System.DateTime.Today.ToShortDateString().Replace("-", "")
                             CREATE_DATE = _createDate
                         }).ToList();

                int num = 1;
                foreach (var item in items)
                {
                    item.ID = num.ToString();
                    num++;
                }

                //dataGrid1.ItemsSource = items;

            }

            return items;
        }

        public void CreteStockData()
        {
            //int workCnt = GetWorkCnt();

            //if (workCnt != 0)
            //{
            //    return;
            //}

            //CreteDataWorkHis();


            var newData = GetStockData();

            if (newData == null)
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                connection.Open();
                StringBuilder sb = new StringBuilder();
                SqlCommand cmd;
                String sql = sb.ToString();


                //CLEAR - STOCK_IDX
                sb = new StringBuilder();

                sb.Append("DELETE FROM STOCK_IDX  ");

                cmd = new SqlCommand(sb.ToString(), connection);
                cmd.ExecuteNonQuery();

                //INSERT
                foreach (var item in newData)
                {
                    sb = new StringBuilder();

                    sb.Append("INSERT INTO STOCK_IDX  ");
                    sb.Append("(  ");
                    sb.Append("KIND  ");
                    sb.Append(",TIME  ");
                    sb.Append(",VALUE  ");             
                    sb.Append(",CREATE_DATE  ");
                    sb.Append(")  ");
                    sb.Append("VALUES ");
                    sb.Append("(  ");
                    sb.Append("'" + item.KIND + "',");
                    sb.Append("'" + item.TIME + "', ");
                    sb.Append("'" + item.VALUE + "', ");
                    sb.Append("'" + item.CREATE_DATE + "' ");
                    sb.Append(")  ");

                    sql = sb.ToString();

                    cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void CreteKoreaData()
        {
            //int workCnt = GetWorkCnt();

            //if (workCnt != 0)
            //{
            //    return;
            //}

            //CreteDataWorkHis();


            //var newData = GetStockData();            
            var newData = GetKoreaData("KOSDAQ");

            if (newData == null)
            {
                return;
            }

            //CLEAR - STOCK_IDX

            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                connection.Open();
                StringBuilder sb = new StringBuilder();
                SqlCommand cmd;
                String sql = sb.ToString();

                //CLEAR - KOREA_INFO
                sb = new StringBuilder();

                sb.Append("DELETE FROM KOREA_INFO  ");

                cmd = new SqlCommand(sb.ToString(), connection);
                cmd.ExecuteNonQuery();

                //INSERT
                foreach (var item in newData)
                {
                    sb = new StringBuilder();

                    sb.Append("INSERT INTO KOREA_INFO  ");
                    sb.Append("(  ");
                    sb.Append("KIND  ");
                    sb.Append(",TIME  ");
                    sb.Append(",VALUE  ");
                    sb.Append(",CREATE_DATE  ");
                    sb.Append(")  ");
                    sb.Append("VALUES ");
                    sb.Append("(  ");
                    sb.Append("'" + item.KIND + "',");
                    sb.Append("'" + item.TIME + "', ");
                    sb.Append("'" + item.VALUE + "', ");
                    sb.Append("'" + item.CREATE_DATE + "' ");
                    sb.Append(")  ");

                    sql = sb.ToString();

                    cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<StockItem> GetStockData()
        {
            List<StockItem> items = new List<StockItem>();

            //CSYPZ37CMH7K4J9XVALS
            //서비스명 text    Y
            //인증키 text Y
            //요청타입 text    Y
            //언어  text Y
            //요청시작건수 text    Y
            //요청종료건수  text Y

            //주가 - 코스피
            //String strUrl = "http://ecos.bok.or.kr/api/StatisticSearch/CSYPZ37CMH7K4J9XVALS/xml/kr/1/1000/064Y001/DD/20190101/20191231/0001000/";
            //String strUrl = "http://ecos.bok.or.kr/api/StatisticSearch/CSYPZ37CMH7K4J9XVALS/xml/kr/1/1000/064Y001/DD/20200101/20200331/0001000/";
            //String strUrl = "http://ecos.bok.or.kr/api/StatisticSearch/CSYPZ37CMH7K4J9XVALS/xml/kr/1/1000/064Y001/DD/20200401/20200431/0001000/";

            
            string fromDt = System.DateTime.Today.AddYears(-3).ToShortDateString().Replace("-", "");
            string toDt = System.DateTime.Today.ToShortDateString().Replace("-", "");


            String strUrl = "http://ecos.bok.or.kr/api/StatisticSearch/CSYPZ37CMH7K4J9XVALS/xml/kr/1/1000/064Y001/DD/" + fromDt  + "/" + toDt + "/0001000/";



            WebRequest request = HttpWebRequest.Create(strUrl);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String result = reader.ReadToEnd();
            xmlData = result.ToString();

            XDocument doc = XDocument.Parse(xmlData);

            if (doc.Root != null)
            {
                string _createDate = System.DateTime.Now.Year.ToString()
                                  + string.Format("{0:D2}", System.DateTime.Now.Month)
                                  + string.Format("{0:D2}", System.DateTime.Now.Day);

                items = (from r in doc.Root.Elements("row")
                         select new StockItem
                         {
                             KIND = "KOSPI",
                             TIME = (string)r.Element("TIME"),
                             VALUE = (string)r.Element("DATA_VALUE"),                                                          
                             CREATE_DATE = _createDate
                         }).ToList();
            }

            return items;
        }

        public List<StockItem> GetKoreaData(string pKind)
        {
            List<StockItem> items = new List<StockItem>();

            //CSYPZ37CMH7K4J9XVALS
            //서비스명 text    Y
            //인증키 text Y
            //요청타입 text    Y
            //언어  text Y
            //요청시작건수 text    Y
            //요청종료건수  text Y

            //주가 - 코스피
            //String strUrl = "http://ecos.bok.or.kr/api/StatisticSearch/CSYPZ37CMH7K4J9XVALS/xml/kr/1/1000/064Y001/DD/20190101/20191231/0001000/";
            //String strUrl = "http://ecos.bok.or.kr/api/StatisticSearch/CSYPZ37CMH7K4J9XVALS/xml/kr/1/1000/064Y001/DD/20200101/20200331/0001000/";

            //코스닥
            //String strUrl = "http://ecos.bok.or.kr/api/StatisticSearch/CSYPZ37CMH7K4J9XVALS/xml/kr/1/1000/064Y001/DD/20190101/20200421/0089000/";


            string fromDt = System.DateTime.Today.AddYears(-3).ToShortDateString().Replace("-", "");
            string toDt = System.DateTime.Today.ToShortDateString().Replace("-", "");

            String strUrl = "http://ecos.bok.or.kr/api/StatisticSearch/CSYPZ37CMH7K4J9XVALS/xml/kr/1/1000/064Y001/DD/" + fromDt + "/" + toDt +  "/0089000/";

            WebRequest request = HttpWebRequest.Create(strUrl);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String result = reader.ReadToEnd();
            xmlData = result.ToString();

            XDocument doc = XDocument.Parse(xmlData);

            if (doc.Root != null)
            {
                string _createDate = System.DateTime.Now.Year.ToString()
                                  + string.Format("{0:D2}", System.DateTime.Now.Month)
                                  + string.Format("{0:D2}", System.DateTime.Now.Day);

                items = (from r in doc.Root.Elements("row")
                         select new StockItem
                         {
                             KIND = pKind,
                             TIME = (string)r.Element("TIME"),
                             VALUE = (string)r.Element("DATA_VALUE"),
                             CREATE_DATE = _createDate
                         }).ToList();
            }

            return items;
        }

        public int GetWorkCnt()
        {
            //string reVal = string.Empty;
            int reVal = 0;

            try
            {
                tasklist = new List<TaskMasterModel>();

                using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
                {
                    //Console.WriteLine("\nQuery data example:");
                    //Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    string _kind = "KOREA_ECONOMIC_IDX";
                    //string _date = System.DateTime.Today.ToShortDateString().Replace("-", "");
                    string _date = System.DateTime.Now.Year.ToString()
                                   + string.Format("{0:D2}", System.DateTime.Now.Month)
                                   + string.Format("{0:D2}", System.DateTime.Now.Day);

                    sb.Append("SELECT * ");
                    sb.Append("FROM TASKMASTER ");
                    sb.Append("WHERE 1=1 ");
                    sb.Append("AND KIND = '" + _kind + "' ");
                    sb.Append("AND WORK_DATE = '" + _date + "' ");

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows == true)
                            {
                                while (reader.Read())
                                {
                                    TaskMasterModel item = new TaskMasterModel();

                                    item.KIND = reader.GetString(reader.GetOrdinal("KIND"));
                                    item.NAME = reader.GetString(reader.GetOrdinal("NAME"));
                                    item.WORK_DATE = reader.GetString(reader.GetOrdinal("WORK_DATE"));


                                    tasklist.Add(item);
                                }

                                //dataGrid1.ItemsSource = datalist;
                                //reVal = "조회되었습니다.";
                                reVal = tasklist.Count;
                            }

                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //Console.WriteLine(ex.ToString());
                //reVal = ex.ToString();
            }
            //Console.ReadLine();

            return reVal;
        }

    }


}
