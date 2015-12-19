using System.Web.Mvc;

namespace TradeManager.Controllers
{
    public class HomeController : Controller
    {
       
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}