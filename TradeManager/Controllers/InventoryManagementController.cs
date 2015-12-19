using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TradeManager.Models;
using eBay.Service.Core.Soap;
using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TradeManager.Functions;

namespace TradeManager.Controllers
{
    [Authorize]
    public class InventoryManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: InventoryManagement
        public ActionResult Index(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            return View(db.Inventories.Where(m=>m.UserID == currentUserId).ToList());
        }

        // GET: InventoryManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentUserId = User.Identity.GetUserId();
            InventoryManagementModel inventoryManagementModel = db.Inventories.Find(id);
            if (inventoryManagementModel == null || inventoryManagementModel.UserID != currentUserId)
            {
                return HttpNotFound();
            }
            return View(inventoryManagementModel);
        }

        // GET: InventoryManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventoryManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SKU,Title,LocationStock,Quantity,StockTime,Description,CategoryID,StartPrice,BuyItNowPrice,ListingDuration,LocationCity")] InventoryManagementModel inventoryManagementModel)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                inventoryManagementModel.UserID = currentUserId;
                db.Inventories.Add(inventoryManagementModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventoryManagementModel);
        }

        // GET: InventoryManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryManagementModel inventoryManagementModel = db.Inventories.Find(id);
            var currentUserId = User.Identity.GetUserId();
            if (inventoryManagementModel == null || inventoryManagementModel.UserID != currentUserId)
            {
                return HttpNotFound();
            }

            return View(inventoryManagementModel);
        }

        // POST: InventoryManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SKU,Title,LocationStock,Quantity,StockTime,Description,CategoryID,StartPrice,BuyItNowPrice,ListingDuration,LocationCity")] InventoryManagementModel inventoryManagementModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventoryManagementModel).State = EntityState.Modified;
                db.SaveChanges();
                if (string.Compare("on", Request["Check"]) == 0)
                {
                    var currentUserId = User.Identity.GetUserId();
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var currentUser = manager.FindById(currentUserId);
                    string Token = currentUser.Token;
                    ItemTypeCollection items = SellerList.getSellerList(Token);
                    foreach (ItemType model in items)
                    {
                        if (string.Compare(model.SKU, Request["SKU"]) == 0)
                        {

                            //model.Title = "different name";

                            model.Title = "different name";
                            model.Title = Request["Title"];
                            //model.ItemID = item.ItemID;
                            model.SKU = Request["SKU"];
                            model.Description = Request["Description"];
                            model.Currency = CurrencyCodeType.USD;
                            model.StartPrice = new AmountType();
                            model.StartPrice.Value = Convert.ToDouble(Request["StartPrice"]);
                            model.StartPrice.currencyID = CurrencyCodeType.USD;
                            model.BuyItNowPrice = new AmountType();
                            model.BuyItNowPrice.Value = Convert.ToDouble(Request["BuyItNowPrice"]);
                            model.BuyItNowPrice.currencyID = CurrencyCodeType.USD;
                            model.ListingDuration = Request["ListingDuration"];
                            model.Location = Request["LocationCity"];
                            model.Country = CountryCodeType.US;
                            CategoryType category = new CategoryType();
                            category.CategoryID = Request["CategoryID"];
                            model.PrimaryCategory = category;
                            model.Quantity = Convert.ToInt32(Request["Quantity"]);
                            //model.PictureDetails.PictureURL = Request["Photo"];



                            model.ConditionID = 1000;
                            //model.ItemSpecifics = AddItem.buildItemSpecifics();
                            // payment methods
                            model.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection();
                            model.PaymentMethods.AddRange(
                                new BuyerPaymentMethodCodeType[] { BuyerPaymentMethodCodeType.PayPal }
                                );
                            // email is required if paypal is used as payment method
                            model.PayPalEmailAddress = "me@ebay.com";

                            // handling time is required
                            model.DispatchTimeMax = 1;
                            // shipping details
                            model.ShippingDetails = AddItem.BuildShippingDetails();

                            // return policy
                            model.ReturnPolicy = new ReturnPolicyType();
                            model.ReturnPolicy.ReturnsAcceptedOption = "ReturnsAccepted";

                            //add pictures
                            if (string.Compare("on", Request["AddPhoto"]) == 0)
                            {
                                StringCollection URLs = new StringCollection();
                                //save pictures to Photos
                                for (int i = 0; i < Request.Files.Count; i++)
                                {
                                    var file = Request.Files[i];
                                    if (file.ContentLength == 0)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        //save　　　
                                        string filetype = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, file.FileName.Length - file.FileName.LastIndexOf(".") - 1);
                                        file.SaveAs(Server.MapPath(@"~/Photos/" + DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + filetype));
                                        URLs.Add(Server.MapPath(@"~/Photos/" + DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + filetype));
                                    }
                                }

                                string[] PicURLs = UploadSiteHostedPictures.UploadMySiteHostedPictures(Token, URLs);
                                StringCollection SC = new StringCollection();
                                foreach (string Url in PicURLs)
                                {
                                    SC.Add(Url);
                                }
                                PictureDetailsType PicType = new PictureDetailsType();
                                PicType.PictureURL = SC;
                                model.PictureDetails = PicType;
                            }

                            ReviseFixedPriceItem.ReviseMyFixedPriceItem(Token, model);
                            return RedirectToAction("Index");
                        }
                    }
                }
            }          
            return RedirectToAction("Index");
        }

        // GET: InventoryManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryManagementModel inventoryManagementModel = db.Inventories.Find(id);
            var currentUserId = User.Identity.GetUserId();
            if (inventoryManagementModel == null || inventoryManagementModel.UserID != currentUserId)
            {
                return HttpNotFound();
            }
            return View(inventoryManagementModel);
        }

        // POST: InventoryManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InventoryManagementModel inventoryManagementModel = db.Inventories.Find(id);
            db.Inventories.Remove(inventoryManagementModel);
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

        public ActionResult AddToList(int? id)
        {   
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryManagementModel inventoryManagementModel = db.Inventories.Find(id);
            if (inventoryManagementModel == null)
            {
                return HttpNotFound();
            }

            return View(inventoryManagementModel);
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToList(int? id, string useless)
        {


            //Create an Item
            ItemType model = new ItemType();
            model.Title = Request["Title"];
            model.SKU = Request["SKU"];
            model.Description = Request["Description"];
            model.ListingType = ListingTypeCodeType.FixedPriceItem;
            model.Currency = CurrencyCodeType.USD;
            model.ListingDuration = Request["ListingDuration"];
            model.Location = Request["LocationCity"];
            model.Country = CountryCodeType.US;
            model.ConditionID = 1000;
            CategoryType category = new CategoryType();
            category.CategoryID = Request["CategoryID"];
            model.PrimaryCategory = category;
            model.StartPrice = new AmountType();
            model.StartPrice.Value = Convert.ToDouble(Request["StartPrice"]);
            model.StartPrice.currencyID = CurrencyCodeType.USD;
            //model.Variations = new VariationsType();
            model.Quantity = Convert.ToInt32(Request["Quantity"]);
            //model.BuyItNowPrice = new AmountType();
            //model.BuyItNowPrice.Value = Convert.ToDouble(Request["BuyItNowPrice"]);
            //model.BuyItNowPrice.currencyID = CurrencyCodeType.USD;

            //model.PictureDetails.PictureURL = Request["Photo"];


            //model.ItemSpecifics = AddItem.buildItemSpecifics();
            // payment methods
            model.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection();
            model.PaymentMethods.AddRange(
                new BuyerPaymentMethodCodeType[] { BuyerPaymentMethodCodeType.PayPal }
                );
            // email is required if paypal is used as payment method
            model.PayPalEmailAddress = "me@ebay.com";

            // handling time is required
            model.DispatchTimeMax = 1;
            // shipping details
            model.ShippingDetails = AddFixedPriceItem.BuildShippingDetails();

            // return policy
            model.ReturnPolicy = new ReturnPolicyType();
            model.ReturnPolicy.ReturnsAcceptedOption = "ReturnsAccepted";
            // handling time is required
            model.DispatchTimeMax = 1;
            
            //get Token
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserId);
            string Token = currentUser.Token;

            //add pictures
            if (string.Compare("on", Request["AddPhoto"]) == 0)
            {
                StringCollection URLs = new StringCollection();
                //save pictures to Photos
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file.ContentLength == 0)
                    {
                        continue;
                    }
                    else
                    {
                        //save　　　
                        string filetype = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, file.FileName.Length - file.FileName.LastIndexOf(".") - 1);
                        file.SaveAs(Server.MapPath(@"~/Photos/" + DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + filetype));
                        URLs.Add(Server.MapPath(@"~/Photos/" + DateTime.Now.ToString("yyyyMMddHHmmss") + i + "." + filetype));
                    }
                }

                string[] PicURLs = UploadSiteHostedPictures.UploadMySiteHostedPictures(Token, URLs);
                StringCollection SC = new StringCollection();
                foreach (string Url in PicURLs)
                {
                    SC.Add(Url);
                }
                PictureDetailsType PicType = new PictureDetailsType();
                PicType.PictureURL = SC;
                model.PictureDetails = PicType;
            }

            AddFixedPriceItem.AddMyFixedPriceItem(currentUser.Token, model);

            return RedirectToAction("Index");
        }

        public ActionResult Refresh(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserId);
            string Token = currentUser.Token;
            ItemTypeCollection items = SellerList.getSellerList(Token);
            foreach(ItemType item in items){
                if (string.Compare(item.SKU, db.Inventories.Find(id).SKU) == 0)
                    db.Inventories.Find(id).Quantity = item.Quantity;
                    db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult SellingList()
        {
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserId);
            string Token = currentUser.Token;
            ItemTypeCollection items = SellerList.getSellerList(Token);
            ViewBag.Items = items;
            return View();
        }

        public ActionResult EndItem(string id)
        {
            var currentUserId = User.Identity.GetUserId();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(currentUserId);
            string Token = currentUser.Token;
            EndmyItem.EndMyItem(Token, id);
             
            return RedirectToAction("SellingList");
        }
    
    }
}
