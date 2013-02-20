using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MC.MvcQuickNav.ExampleWeb.Controllers
{
    public class ControlsController : Controller
    {
        //
        // GET: /Contacts/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult Sitemap()
        {
            return View();
        }

        public ActionResult Breadcrumbs()
        {
            return View();
        }

        public ActionResult InThisSection()
        {
            return View();
        }

        public ActionResult Custom()
        {
            return View();
        }

        public ActionResult Manual()
        {
            var model = GetRandomTree(3);
            model.First().Value.IsActive = true;
            return View(model);
        }

        private IEnumerable<NavigationNode> GetRandomTree(int maxDepth)
        {
            if(maxDepth == 0) 
                return new List<NavigationNode>();

            return Enumerable.Range(3, 5).Select(i =>
                new NavigationNode 
                {
                    Value = new NavigationItem
                    {
                        Title = new String((char)('a' + Random.Next(26)), Random.Next(1, 6)), 
                        Url = Request.RawUrl
                    },
                    Children = GetRandomTree(maxDepth - 1)
                }
            );
        }

        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        public ActionResult NavigationManager()
        {
            return View();
        }
    }
}
