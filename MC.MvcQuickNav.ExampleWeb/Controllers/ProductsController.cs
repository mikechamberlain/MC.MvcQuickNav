using System.Web.Mvc;
using MC.MvcQuickNav.ExampleWeb.Models;

namespace MC.MvcQuickNav.ExampleWeb.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string id, string sublevel)
        {
            return View(new ProductModel { Name = id, Sublevel = sublevel });
        }
    }
}
