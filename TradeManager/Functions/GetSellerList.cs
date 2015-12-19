using System;
using System.Configuration;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using eBay.Service.Util;
using TradeManager.Models;
using System.Web;
using myTokens;

namespace TradeManager.Functions
{
    /// <summary>
    /// A simple item adding sample,
    /// show basic flow to list an item to eBay Site using eBay SDK.
    /// </summary>
    public class SellerList
    {

        private static ApiContext apiContext = null;
       public static ItemTypeCollection getSellerList(string Token)
        {
            

            //Initialize eBay ApiContext object
            ApiContext apiContext = GetApiContext(Token);


            //Create Call object and execute the Call
            GetSellerListCall apiCall = new GetSellerListCall(apiContext);
            //Begin to call eBay API
            apiCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);

            // Setting the Pagination which is a required input parameter for GetSellerList call
            CallRetry oCallRetry = new CallRetry();
            // set the delay between each retry to 1 millisecond
            oCallRetry.DelayTime = 1;
            // set the maximum number of retries
            oCallRetry.MaximumRetries = 3;
            // set the error codes on which to retry
            StringCollection oErrorCodes = new StringCollection();
            oErrorCodes.Add("10007"); // Internal error to the application ... general error
            oErrorCodes.Add("2"); // unsupported verb error
            oErrorCodes.Add("251"); // eBay Structured Exception ... general error
            oCallRetry.TriggerErrorCodes = oErrorCodes;
            // set the exception types on which to retry
            TypeCollection oExceptions = new TypeCollection();
            oExceptions.Add(typeof(System.Net.ProtocolViolationException));
            // the "Client found response content type of 'text/plain'" exception is of type SdkException, so let's add that to the list
            oExceptions.Add(typeof(SdkException));
            oCallRetry.TriggerExceptions = oExceptions;

            // set CallRetry back to ApiContext
            apiContext.CallRetry = oCallRetry;

            // set the timeout to 2 minutes
            apiContext.Timeout = 120000;

            GetSellerListCall oGetSellerListCall = new GetSellerListCall(apiContext);

            // set the Version used in the call
            oGetSellerListCall.Version = apiContext.Version;

            // set the Site of the call
            oGetSellerListCall.Site = apiContext.Site;

            // enable the compression feature
            oGetSellerListCall.EnableCompression = true;

            // use GranularityLevel of Fine
            oGetSellerListCall.GranularityLevel = GranularityLevelCodeType.Fine;

            // get the first page, 200 items per page
            PaginationType oPagination = new PaginationType();
            oPagination.EntriesPerPage = 200;
            oPagination.EntriesPerPageSpecified = true;
            oPagination.PageNumber = 1;
            oPagination.PageNumberSpecified = true;
            oGetSellerListCall.Pagination = oPagination;

            // ask for all items that are ending in the future (active items)
            oGetSellerListCall.EndTimeFilter = new TimeFilter(DateTime.Now, DateTime.Now.AddMonths(3));

            // return items that end soonest first
            oGetSellerListCall.Sort = 2;

            ItemTypeCollection items = oGetSellerListCall.GetSellerList();

            
            return items;
        }


        static ApiContext GetApiContext(string Token)
        {
            //apiContext is a singleton,
            //to avoid duplicate configuration reading
            if (apiContext != null)
            {
                return apiContext;
            }
            else
            {
                apiContext = new ApiContext();

                //set Api Server Url
                apiContext.SoapApiServerUrl =
                    ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
                //set Api Token to access eBay Api Server
                ApiCredential apiCredential = new ApiCredential();
                apiCredential.eBayToken = Token;
                apiContext.ApiCredential = apiCredential;
                //set eBay Site target to US
                apiContext.Site = SiteCodeType.US;

                //set Api logging
                apiContext.ApiLogManager = new ApiLogManager();
                apiContext.ApiLogManager.ApiLoggerList.Add(
                    new FileLogger("listing_log.txt", true, true, true)
                    );
                apiContext.ApiLogManager.EnableLogging = true;


                return apiContext;
            }
        }




    }
}
