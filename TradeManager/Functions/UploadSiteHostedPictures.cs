using System;
using System.Collections.Generic;
using System.Text;
using eBay.Service.EPS;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace TradeManager.Functions
{
    public class UploadSiteHostedPictures
    {
        public static string[] UploadMySiteHostedPictures(string Token, StringCollection PictureList)
        {
            ApiContext context = new ApiContext();
            context.ApiCredential.eBayToken = ConfigurationManager.AppSettings["UserAccount.ApiToken"];
            context.Site = SiteCodeType.US;
            context.SignInUrl = @"https://api.sandbox.ebay.com/wsapi";
            eBayPictureService pictureService = new eBayPictureService(context);
            pictureService.ApiContext.EPSServerUrl = @"https://api.sandbox.ebay.com/ws/api.dll";
            UploadSiteHostedPicturesRequestType request = new UploadSiteHostedPicturesRequestType();
            request.PictureName = "Deva_testimage_URL";
            request.PictureSet = PictureSetCodeType.Large;
            request.PictureSetSpecified = true;
            string[] PicTemp = PictureList.ToArray();
            string[] picURLs = pictureService.UpLoadPictureFiles(PhotoDisplayCodeType.SuperSize, PicTemp);
            return picURLs;
        }

        //public static void SavePictures(HttpFileCollection Files)
        //{
        //    if (Files.Count == 0)
        //    {
        //        //file count > 0
        //        return;
        //    }

        //    var file = Files[0];
        //    if (file.ContentLength == 0)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        //save　　　
        //        string filetype = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1, file.FileName.Length - file.FileName.LastIndexOf(".") - 1);
        //        file.SaveAs(Controller.Server.MapPath(@"~/Photos/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + filetype));
        //    }
        //}
    }
}