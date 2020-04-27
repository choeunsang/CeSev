using ceSev.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ceSev
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ExcuteJobSchedule();
        }

        private void ExcuteJobSchedule()
        {
            System.Timers.Timer timScheduledTask = new System.Timers.Timer();

            //timScheduledTask.Interval = 6 * 1000; //1분마다 실행

            //timScheduledTask.Interval = 60 * 1000; //1분마다 실행
            //timScheduledTask.Interval = 600 * 1000; //10분마다 실행
            //timScheduledTask.Interval = (600 * 6) * 1000; //1시간 마다 실행
            timScheduledTask.Interval = ((600 * 6) * 12) * 1000; //12시간마다 실행
            //timScheduledTask.Interval = ((600 * 6) * 24)  * 1000 ; //1일마다 실행
            //timScheduledTask.Interval = TimeSpan.FromHours(1).TotalSeconds();
            timScheduledTask.Enabled = true;
            
            timScheduledTask.Elapsed += new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);

        
        }

        private void timScheduledTask_Elapsed(object sender, ElapsedEventArgs e)
	    {
            TaskWork taskwork = new TaskWork();
            taskwork.CreteDataTryHis();
            taskwork.CreteData();
        }

    }
}
