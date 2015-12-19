using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using System.Configuration;

namespace TradeManager.Functions
{
    public class FetchToken
    {
        public static String FetchUserToken(string SessionID){
            ApiContext localContext = new ApiContext();

            ApiAccount apiAccount = new ApiAccount();
            apiAccount.Application = ConfigurationManager.AppSettings["UserAccount.AppId"];
            apiAccount.Certificate = ConfigurationManager.AppSettings["UserAccount.CertId"];
            apiAccount.Developer = ConfigurationManager.AppSettings["UserAccount.DevId"];
            localContext.ApiCredential.ApiAccount = apiAccount;
            localContext.SoapApiServerUrl = ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
            FetchTokenCall apiCall = new FetchTokenCall();
            apiCall.ApiContext = localContext;
            return apiCall.FetchToken(SessionID);
         }
    }
}