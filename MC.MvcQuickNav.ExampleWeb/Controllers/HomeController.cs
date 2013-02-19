using System.Web.Mvc;

namespace MC.MvcQuickNav.ExampleWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Walkthrough()
        {
            return View();
        }
    }
}