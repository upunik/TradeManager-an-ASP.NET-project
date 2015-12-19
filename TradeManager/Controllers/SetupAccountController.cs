using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradeManager.Functions;
using TradeManager.Models;

namespace TradeManager.Controllers
{
    [Authorize]
    public class SetupAccountController : Controller
    {

        public static string SessionID = GetSessionID.getSessionID();
        // GET: SetupAccount
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult SessionIDPage()
        {
            ViewBag.SessionURL = GetSessionID.getAuthenticateUrl(SessionID);
            return View();
        }

        public ActionResult FetchTokenPage()
        {
            string UserToken = FetchToken.FetchUserToken(SessionID);
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserId);
            //currentUser.Email;
            var db = new ApplicationDbContext();
            var data = db.Users.Where(p => p.Email == currentUser.Email).Single();
            data.Token = UserToken;
            db.SaveChanges();
            return View();

        }
    }
}