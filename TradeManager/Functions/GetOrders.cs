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
    public class GetOrders
    {
        private static ApiContext apiContext = null;
        public static OrderTypeCollection GetMyOrders(string Token, DateTime CreateTimeFrom)
        {
            ApiContext apiContext = GetApiContext(Token);
            DateTime CreateTimeTo;
            GetOrdersCall getOrders = new GetOrdersCall(apiContext);
            getOrders.DetailLevelList = new DetailLevelCodeTypeCollection();
            getOrders.DetailLevelList.Add(DetailLevelCodeType.ReturnAll);

            CreateTimeTo = DateTime.Now.ToUniversalTime();
            
            //minus 2 minutes
            TimeSpan ts2 = new TimeSpan(1200000000);
            getOrders.CreateTimeFrom = CreateTimeFrom;
            getOrders.CreateTimeTo = CreateTimeTo;
            TradingRoleCodeType OrderRole = TradingRoleCodeType.Seller;
            OrderStatusCodeType OrderStatus = OrderStatusCodeType.All;
            OrderTypeCollection Orders = getOrders.GetOrders(CreateTimeFrom, CreateTimeTo, OrderRole, OrderStatus);
            return Orders;
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
                //apiCredential.eBayToken = ConfigurationManager.AppSettings["UserAccount.ApiToken"];
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