using System;
using System.Configuration;
using System.Collections.Generic;
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
    class GetItem
    {

        private static ApiContext apiContext = null;
        static void Main(string[] args)
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("+ Welcome to eBay SDK for .Net Sample +");
            Console.WriteLine("+ - ConsoleGetUser                    +");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine();

            //Initialize eBay ApiContext object
            ApiContext apiContext = GetApiContext();


            //Create Call object and execute the Call
            GetItemCall apiCall = new GetItemCall(apiContext);
            //Begin to call eBay API
            apiCall.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);
            ItemType item = apiCall.GetItem(ItemID: "110163122726");

            Console.WriteLine(item.Description);
            Console.WriteLine();
            Console.WriteLine("Press any key to close the program.");
            Console.ReadKey();




        }


        static ApiContext GetApiContext()
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
                apiCredential.eBayToken = ConfigurationManager.AppSettings["UserAccount.ApiToken"];
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
