using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MC.MvcQuickNav.ExampleWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Download",
                "download",
                new { controller = "Home", action = "Download", sublevel = UrlParameter.Optional });

            routes.MapRoute(
                "Controls",
                "controls",
                new { controller = "Controls", action = "Index" });

            routes.MapRoute(
                "DemoRoot",
                "demo",
                new { controller = "Demo", action = "Index" });

            routes.MapRoute(
                "Demo",
                "demo/{level}/{sublevel}",
                new { controller = "Demo", action = "Index", sublevel = UrlParameter.Optional });

            routes.MapRoute(
                "Home", // Route name
                "{action}", // URL with parameters
                new { controller = "Home" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        // in Global.asax.cs
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}