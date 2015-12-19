using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TradeManager.Functions;
using TradeManager.Models;

namespace TradeManager.Controllers
{
    [Authorize]
    public class MarketResearchModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MarketResearchModels
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            return View(db.MarketResearch.Where(m=>m.UserID == currentUserId).ToList());
        }

        // GET: MarketResearchModels/Details/5
        public ActionResult Refresh(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketResearchModel marketResearchModel = db.MarketResearch.Find(id);
            if (marketResearchModel == null)
            {
                return HttpNotFound();
            }
            string Keyword = marketResearchModel.Keyword;
            double PriceFrom = marketResearchModel.PriceFrom;
            double PriceTo = marketResearchModel.PriceTo;
            string pageSource = GetWeb.GetPageHtml("http://shop.sandbox.ebay.com/i.html?_nkw=" + Keyword + "&_mPrRngCbx=1&_udlo=" + PriceFrom + "&_udhi=" + PriceTo + "&_trksid=p3286.c0.m14&_sop=12&_sc=1");
            MatchCollection m1 = Regex.Matches(pageSource, @"g-b"">\$(\d+.\d+)", RegexOptions.Singleline);
            double sum = 0;
            double average = 0;
            foreach (Match item in m1)
            {
                sum += Convert.ToDouble(item.Groups[1].Value);
            }
            average = sum / m1.Count;
            marketResearchModel.AveragePrice = average;
            db.Entry(marketResearchModel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: MarketResearchModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MarketResearchModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Keyword,PriceFrom,PriceTo,SearchLink")] MarketResearchModel marketResearchModel)
        {
            if (ModelState.IsValid)
            {
                string Keyword = Request["Keyword"];
                double PriceFrom = Convert.ToDouble(Request["PriceFrom"]);
                double PriceTo = Convert.ToDouble(Request["PriceTo"]);
                string pageSource = GetWeb.GetPageHtml("http://shop.sandbox.ebay.com/i.html?_nkw=" + Keyword + "&_mPrRngCbx=1&_udlo=" + PriceFrom + "&_udhi=" + PriceTo + "&_trksid=p3286.c0.m14&_sop=12&_sc=1");

                var currentUserId = User.Identity.GetUserId();
                marketResearchModel.UserID = currentUserId;
                MatchCollection m1 = Regex.Matches(pageSource, @"g-b"">\$(\d+.\d+)",
                    RegexOptions.Singleline);
                double sum = 0;
                double average = 0;
                foreach (Match item in m1)
                {
                    //int sum =  item.Groups[1].Value;

                    sum += Convert.ToDouble(item.Groups[1].Value);

                }
                average = sum / m1.Count;
                marketResearchModel.AveragePrice = average;
                db.MarketResearch.Add(marketResearchModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marketResearchModel);
        }

        // GET: MarketResearchModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketResearchModel marketResearchModel = db.MarketResearch.Find(id);
            var currentUserId = User.Identity.GetUserId();
            if (marketResearchModel == null || marketResearchModel.UserID != currentUserId)
            {
                return HttpNotFound();
            }
            return View(marketResearchModel);
        }

        // POST: MarketResearchModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Keyword,PriceFrom,PriceTo,SearchLink")] MarketResearchModel marketResearchModel)
        {
            if (ModelState.IsValid)
            {
                string Keyword = Request["Keyword"];
                double PriceFrom = Convert.ToDouble(Request["PriceFrom"]);
                double PriceTo = Convert.ToDouble(Request["PriceTo"]);
                string pageSource = GetWeb.GetPageHtml("http://shop.sandbox.ebay.com/i.html?_nkw=" + Keyword + "&_mPrRngCbx=1&_udlo=" + PriceFrom + "&_udhi=" + PriceTo + "&_trksid=p3286.c0.m14&_sop=12&_sc=1");

                MatchCollection m1 = Regex.Matches(pageSource, @"g-b"">\$(\d+.\d+)", RegexOptions.Singleline);
                double sum = 0;
                double average = 0;
                foreach (Match item in m1)
                {
                    //int sum =  item.Groups[1].Value;

                    sum += Convert.ToDouble(item.Groups[1].Value);

                }
                average = sum / m1.Count;
                marketResearchModel.AveragePrice = average;
                db.Entry(marketResearchModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marketResearchModel);
        }

        // GET: MarketResearchModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketResearchModel marketResearchModel = db.MarketResearch.Find(id);
            if (marketResearchModel == null)
            {
                return HttpNotFound();
            }
            return View(marketResearchModel);
        }

        // POST: MarketResearchModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MarketResearchModel marketResearchModel = db.MarketResearch.Find(id);
            db.MarketResearch.Remove(marketResearchModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
