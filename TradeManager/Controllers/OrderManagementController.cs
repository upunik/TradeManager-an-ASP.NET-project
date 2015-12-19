using System;
using System.Web.Mvc;
using eBay.Service.Core.Soap;
using Microsoft.AspNet.Identity;
using TradeManager.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using TradeManager.Functions;

namespace FunctionGetOrders
{
    [Authorize]
    public class OrderManagementController : Controller
    {
        // GET: Stocks
        public ActionResult Index()
        {

            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserId);
            string Token = currentUser.Token;
            DateTime CreateTimeFrom = DateTime.Parse("2000.01.01");
            OrderTypeCollection Orders = GetOrders.GetMyOrders(Token, CreateTimeFrom);
            ViewBag.Orders = Orders;
            return View();
        }

        public ActionResult ShippingLabels()
        {
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserId);
            string Token = currentUser.Token;
            DateTime CreateTimeFrom = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString() + " 22:00:00");
            OrderTypeCollection ShippingOrders = GetOrders.GetMyOrders(Token, CreateTimeFrom);
            ViewBag.ShippingOrders = ShippingOrders;

            return View();
        }


    } 
}
