using System.Web.Http;

namespace Geolaxia
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //JobManager.Initialize(new MyRegistry());
        }

        
    }

    //public class MyRegistry : Registry
    //{
    //    public MyRegistry()
    //    {
    //        // Schedule an IJob to run at an interval
    //        Schedule<MyJob>().ToRunNow().AndEvery(5).Seconds();

    //        // Schedule an IJob to run once, delayed by a specific time interval
    //        //Schedule<MyJob>().ToRunOnceIn(5).Seconds();
    //    }
    //}

    //public class MyJob : IJob
    //{
    //    public void Execute()
    //    {
    //        string a = "prueba";
    //    }
    //}
}
