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

        public ActionResult Full()
        {
            return View();
        }

        public ActionResult Breadcrumbs()
        {
            return View();
        }

        public ActionResult Child()
        {
            return View();
        }

        public ActionResult Custom()
        {
            var model = GetRandomNodes(3, 6, 3);
            return View(model);
        }

        private List<NavigationNode> GetRandomNodes(int min, int max, int maxDepth)
        {
            if(maxDepth == 0)
                return new List<NavigationNode>();
            var nodes = new List<NavigationNode>();
            var limit = Random.Next(min, max);
            for (var i = 0; i < limit; i++)
            {
                var item = new NavigationItem
                {
                    Title = RandomString(5),
                    Url = RandomString(10),
                    Description = RandomString(20)
                };
                var node = new NavigationNode(item, GetRandomNodes(min, max, maxDepth - 1)); 
                nodes.Add(node);
            }
            return nodes;
        }

        private string RandomString(int size)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

    }
}
