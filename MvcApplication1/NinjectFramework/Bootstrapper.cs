using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TruckTrackerWeb.NinjectFramework
{
	public class Bootstrapper
	{
		public HttpContextBase HttpContext { get; private set; }
		public RouteCollection Routes { get; private set; }

		public Bootstrapper(HttpContextBase httpContext, RouteCollection routes)
		{
			HttpContext = httpContext;
			Routes = routes;
		}

		public void RegisterRoutes()
		{
			Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

		    Routes.MapRoute("Truck", "truck/{truckId}/{action}", new {controller = "Truck", action = "Index", truckId=0});
			Routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "" });		
		}
	}
}