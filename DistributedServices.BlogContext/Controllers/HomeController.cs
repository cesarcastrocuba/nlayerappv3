using Microsoft.AspNetCore.Mvc;

namespace NLayerApp.DistributedServices.BlogBoundedContext.Controllers
{
    //Home Controller
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("index", "swagger");
        }
    }
}
