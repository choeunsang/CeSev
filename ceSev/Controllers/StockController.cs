using ceSev.Models;
using ceSev.Task;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace ceSev.Controllers
{
    public class StockController : Controller
    {
        public String xmlData;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Korea()
        {
            return View();
        }

        public ActionResult GetTest(string Con, string Don)
        {
            InitDbSet();
            var list = GetGisList();

            return Json(list);
        }

        public ActionResult GetKospiData(string Con, string Don)
        {
            InitDbSet();
            var list = GetKospiData();

            return Json(list);
        }

        public ActionResult GetKosdaqData(string Con, string Don)
        {
            InitDbSet();
            var list = GetKoreaData("KOSDAQ");

            return Json(list);
        }
        
        public ActionResult GetExchageData(string Con, string Don)
        {
            InitDbSet();
            var list = GetKoreaData("EXCHANGE");

            return Json(list);
        }

        private SqlConnectionStringBuilder _builder;

        private void InitDbSet()
        {
            _builder = new SqlConnectionStringBuilder();
            //_builder.DataSource = "cedevdb.database.windows.net";
            //_builder.UserID = "cedevadmin";

            //_builder.DataSource = "DESKTOP-B7FUQ04\\SQLEXPRESS";
            _builder.DataSource = @"DESKTOP-B7FUQ04\SQLEXPRESS";
            _builder.UserID = "sa";
            _builder.Password = "!1ge1046417";
            _builder.InitialCatalog = "cedevdb";
        }

        private List<ContItem> GetGisList()
        {
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                //Console.WriteLine("\nQuery data example:");
                //Console.WriteLine("=========================================\n");

                connection.Open();
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT ID ");
                sb.Append(", CONT_NAME as CONT_NAME");
                sb.Append(", POPU_CNT ");
                sb.Append(", Latitude ");
                sb.Append(", Longitude ");
                sb.Append("FROM CONT_INFO ");
                sb.Append("WHERE 1=1 ");

                //if ((!string.IsNullOrEmpty(pId)) && (pId != "전체"))
                //{
                //    sb.Append("AND ID = '" + pId + "' ");
                //}

                String sql = sb.ToString();
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.SelectCommand = new SqlCommand(sql, connection);
                adapter.Fill(ds);

                //DataRow row = ds.Tables[0].Rows[0];
                List<ContItem> list = new List<ContItem>();

                list.Add(new ContItem() { ID = "", etc = "전체" });

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ContItem item = new ContItem();

                    item.ID = row["ID"].ToString();
                    item.CONT_NAME = row["CONT_NAME"].ToString();
                    item.POPU_CNT = row["POPU_CNT"].ToString();
                    item.Latitude = row["Latitude"].ToString();
                    item.Longitude = row["Longitude"].ToString();
                    item.etc = row["CONT_NAME"].ToString();

                    list.Add(item);
                }

                return list;
            }
        }

        private List<StockItem> GetKospiData()
        {
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                //Console.WriteLine("\nQuery data example:");
                //Console.WriteLine("=========================================\n");

                connection.Open();
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT KIND ");
                sb.Append(", TIME ");
                sb.Append(", VALUE ");
                sb.Append(", CREATE_DATE ");
                
                sb.Append("FROM STOCK_IDX ");
                sb.Append("WHERE 1=1 ");
                sb.Append("ORDER BY TIME ASC ");

                //if ((!string.IsNullOrEmpty(pId)) && (pId != "전체"))
                //{
                //    sb.Append("AND ID = '" + pId + "' ");
                //}

                String sql = sb.ToString();
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.SelectCommand = new SqlCommand(sql, connection);
                adapter.Fill(ds);

                //DataRow row = ds.Tables[0].Rows[0];
                List<StockItem> list = new List<StockItem>();

                //list.Add(new StockItem() { ID = "", etc = "전체" });

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    StockItem item = new StockItem();


                    //sb.Append("SELECT KIND ");
                    //sb.Append(", TIME ");
                    //sb.Append(", VALUE ");
                    //sb.Append(", CREATE_DATE ");

                    item.KIND = row["KIND"].ToString();
                    item.TIME = row["TIME"].ToString();
                    item.VALUE = row["VALUE"].ToString();
                    item.CREATE_DATE = row["CREATE_DATE"].ToString();
                    

                    list.Add(item);
                }

                return list;
            }
        }

        private List<StockItem> GetKoreaData(string pKind)
        {
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            {
                //Console.WriteLine("\nQuery data example:");
                //Console.WriteLine("=========================================\n");

                connection.Open();
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT KIND ");
                sb.Append(", TIME ");
                sb.Append(", VALUE ");
                sb.Append(", CREATE_DATE ");

                sb.Append("FROM KOREA_INFO ");
                sb.Append("WHERE 1=1 ");
                sb.Append("AND KIND='" + pKind + "' ");

                sb.Append("ORDER BY TIME ASC ");

                //if ((!string.IsNullOrEmpty(pId)) && (pId != "전체"))
                //{
                //    sb.Append("AND ID = '" + pId + "' ");
                //}

                String sql = sb.ToString();
                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();

                adapter.SelectCommand = new SqlCommand(sql, connection);
                adapter.Fill(ds);

                //DataRow row = ds.Tables[0].Rows[0];
                List<StockItem> list = new List<StockItem>();

                //list.Add(new StockItem() { ID = "", etc = "전체" });

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    StockItem item = new StockItem();


                    //sb.Append("SELECT KIND ");
                    //sb.Append(", TIME ");
                    //sb.Append(", VALUE ");
                    //sb.Append(", CREATE_DATE ");

                    item.KIND = row["KIND"].ToString();
                    item.TIME = row["TIME"].ToString();
                    item.VALUE = row["VALUE"].ToString();
                    item.CREATE_DATE = row["CREATE_DATE"].ToString();


                    list.Add(item);
                }

                return list;
            }
        }

        public ActionResult China()
        {
            return View();
        }

        public ActionResult Vietnam()
        {
            return View();
        }

        public ActionResult USA()
        {
            return View();
        }        

        public ActionResult Japan()
        {
            return View();
        }

        public ActionResult RawMate()
        {
            return View();
        }

        public ActionResult ExchRate()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult WorkSchedule()
        {            
            TaskWork taskwork = new TaskWork();
            taskwork.CreteData();

            //ViewBag.Message = "Work Schedule!!";
            string strMsg = "Work Schedule!!";

            //return View();
            return Json(new { Response = strMsg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult WorkSchedule2()
        {
            TaskWork taskwork = new TaskWork();
            taskwork.CreteStockData();

            string strMsg = "Work Schedule2!!";

            //return View();
            return Json(new { Response = strMsg }, JsonRequestBehavior.AllowGet);
        }

        //private List<EconomicIdxItem> GetData()
        public JsonResult GetInfo()
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

            //return items;
            return Json(new { Response = items }, JsonRequestBehavior.AllowGet);
        }

    }


}