using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using System.Configuration;

namespace TradeManager.Functions
{
    public class GetSessionID
    {
        public static String getSessionID()
        {
            ApiContext localContext = new ApiContext();
            ApiAccount apiAccount = new ApiAccount();
            apiAccount.Application = ConfigurationManager.AppSettings["UserAccount.AppId"];
            apiAccount.Certificate = ConfigurationManager.AppSettings["UserAccount.CertId"];
            apiAccount.Developer = ConfigurationManager.AppSettings["UserAccount.DevId"];
            localContext.ApiCredential.ApiAccount = apiAccount;
            localContext.SoapApiServerUrl = ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
            GetSessionIDCall apiCall = new GetSessionIDCall(localContext);
            apiCall.RuName = ConfigurationManager.AppSettings["UserAccount.RuName"];
            string SessionID = apiCall.GetSessionID();
            return SessionID;
        }
        public static String getAuthenticateUrl(string SessionID)
        {
            
            return "https://signin.sandbox.ebay.com/ws/eBayISAPI.dll?SignIn&" + "RuName=" + ConfigurationManager.AppSettings["UserAccount.RuName"]
            + "&SessID=" + SessionID;
        }
    }
}