using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MC.MvcQuickNav.ExampleWeb.Models;

namespace MC.MvcQuickNav.ExampleWeb.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/

        public ActionResult Index(string level, string sublevel)
        {
            var model = new DemoModel
            {
                Level = level,
                SubLevel = sublevel
            };
            return View(model);
        }

        public ActionResult NewWindow()
        {
            return View();
        }

    }
}
