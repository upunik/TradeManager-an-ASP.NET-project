using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
using eBay.Service.Core.Soap;
using System.Configuration;
using TradeManager.Models;
using System.Web;
using myTokens;

namespace TradeManager.Functions
{
    public class ReviseFixedPriceItem
    {
        private static ApiContext apiContext = null;
        public static void ReviseMyFixedPriceItem(string Token, ItemType item)    
      {

          //create the context
          ApiContext context = GetApiContext(Token);

          //set the User token
          ReviseFixedPriceItemCall apiCall = new ReviseFixedPriceItemCall(apiContext);

          //Basic (Title revision)
          apiCall.Item = item;
          StringCollection Empty = new StringCollection();
          apiCall.ReviseFixedPriceItem(item, Empty);

          ////create the context
          //ApiContext context = new ApiContext();

          ////set the User token
          //context.ApiCredential.eBayToken = "AgAAAA**AQAAAA**aAAAAA**w0J2VQ**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhDZeFogmdj6x9nY+seQ**KHoDAA**AAMAAA**ZbZQVOOT/g9zM+zyGbPxdgbbAE+vfJjLPpUP929lAOgwZm0mvgrcyo5cp+WyxtLAorQvpvYnlmSpSJg6Ya8gct/o49daexbLi9uIcafEvDlOr5B27q+D0FdIkdCKwK5YCRr7esAeog37ErnQCOiuTCEyZBq6yyNEbNZDwRr+xYSniYPXEc5p8v5xNXEEeb2fUjancZmOFgczzlm4gE/I0oojR7iCm3/4/VSaWXGVt6hDwCi4nZuDHJFbvypANc9DMxt1Rw9VGgiTMisgBnAlYvhBEQ+mU5xZ8JhPbkQsnHPQdIrBH9fdH8Nuq124T7eWkyF40PP4amJlrdNEFmDQIVGW0O1pnkaogsfTDsInHnIBTEIqzf+5JMUFx3nyexUOIGkEEmxo9MAdrzld+g0LNuVt7vmv+2QSJmIFihxLbtz6eQ0whu7qOotcj1AUokMQn1RKGTyZGiTgLMKThlcQ5odnhzGOIxEZn54xT8y60LTIIaPEFQ8uHNBwQd5GyIV6NKslEPnJyMzLnoSiMUvVKVbK7LSQLPK1GHVDr6w5iEjbnXiKRBNmue1q/cDpdWpjqRpP/9JYQVYROlLIsqg38TJ5SP/Q8eqqs50RHz7GtShkTfhhqEaJrc2TK68/hE3yvdVRQqvqxx+AurqwjlO8jEKPSYE6gEmYPu3ByKWQFt+i9BVgxd/9mDJr7ZT2GSzVwnlcwmsbebPOU6wrvaxSCViYrLKRBf3VzTohhjM/hjt6YwEMEY1SpzoKQKg+C3qd";

          ////set the server url
          //context.SoapApiServerUrl = "https://api.sandbox.ebay.com/wsapi";

          ////enable logging
          //context.ApiLogManager = new ApiLogManager();
          //context.ApiLogManager.ApiLoggerList.Add(new FileLogger("log.txt", true, true, true));
          //context.ApiLogManager.EnableLogging = true;

          ////set the version
          ////context.Version = "817";
          //context.Site = SiteCodeType.US;

          //ReviseFixedPriceItemCall reviseFP = new ReviseFixedPriceItemCall(context);

          ////ItemType item = new ItemType();
          //item.SKU = "5591";

          ////Specify the entire item specifics container
          //item.ItemSpecifics = new NameValueListTypeCollection();

          //NameValueListTypeCollection ItemSpecs = new NameValueListTypeCollection();

          //NameValueListType nv1 = new NameValueListType();
          //StringCollection valueCol1 = new StringCollection();

          //nv1.Name = "Brand";
          //valueCol1.Add("Ralph Lauren");
          //nv1.Value = valueCol1;

          //ItemSpecs.Add(nv1);

          //NameValueListType nv2 = new NameValueListType();
          //StringCollection valueCol2 = new StringCollection();
          //nv2.Name = "Size";
          //valueCol2.Add("M");
          //nv2.Value = valueCol2;
          //ItemSpecs.Add(nv2);

          //NameValueListType nv3 = new NameValueListType();
          //StringCollection valueCol3 = new StringCollection();
          //nv3.Name = "Colour";
          //valueCol3.Add("Blue");
          //nv3.Value = valueCol3;
          //ItemSpecs.Add(nv3);

          //item.ItemSpecifics = ItemSpecs;

          //reviseFP.Item = item;
          //reviseFP.Execute();
 
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