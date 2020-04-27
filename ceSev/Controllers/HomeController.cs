using ceSev.Models;
using ceSev.Task;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace ceSev.Controllers
{
    public class HomeController : Controller
    {
        public String xmlData;

        public ActionResult Index()
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