using System;
using TradeManager.com.ebay;
using TradeManager.com.ebay.developer;    // use your project name here
namespace TradeManager.Functions           // use your project name here
{
    class GeteBayOfficialTime
    {
        [STAThread]
        static void Main(string[] args)
        {
            string endpoint = "https://api.sandbox.ebay.com/wsapi";
            string callName = "GeteBayOfficialTime";
            string siteId = "0";
            string appId = "NotyetaC-f7bb-4ca5-97bb-ff7749470550";     // use your app ID
            string devId = "37ecd1fc-5180-4492-840c-d5f1e4e43a89";     // use your dev ID
            string certId = "6f56af79-65cf-4d1c-873f-9d3f34641115";   // use your cert ID
            string version = "405";
            // Build the request URL
            string requestURL = endpoint
            + "?callname=" + callName
            + "&siteid=" + siteId
            + "&appid=" + appId
            + "&version=" + version
            + "&routing=default";
            // Create the service
            eBayAPIInterfaceService service = new eBayAPIInterfaceService();
            // Assign the request URL to the service locator.
            service.Url = requestURL;
            // Set credentials
            service.RequesterCredentials = new CustomSecurityHeaderType();
            service.RequesterCredentials.eBayAuthToken = "AgAAAA**AQAAAA**aAAAAA**w0J2VQ**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhDZeFogmdj6x9nY+seQ**KHoDAA**AAMAAA**ZbZQVOOT/g9zM+zyGbPxdgbbAE+vfJjLPpUP929lAOgwZm0mvgrcyo5cp+WyxtLAorQvpvYnlmSpSJg6Ya8gct/o49daexbLi9uIcafEvDlOr5B27q+D0FdIkdCKwK5YCRr7esAeog37ErnQCOiuTCEyZBq6yyNEbNZDwRr+xYSniYPXEc5p8v5xNXEEeb2fUjancZmOFgczzlm4gE/I0oojR7iCm3/4/VSaWXGVt6hDwCi4nZuDHJFbvypANc9DMxt1Rw9VGgiTMisgBnAlYvhBEQ+mU5xZ8JhPbkQsnHPQdIrBH9fdH8Nuq124T7eWkyF40PP4amJlrdNEFmDQIVGW0O1pnkaogsfTDsInHnIBTEIqzf+5JMUFx3nyexUOIGkEEmxo9MAdrzld+g0LNuVt7vmv+2QSJmIFihxLbtz6eQ0whu7qOotcj1AUokMQn1RKGTyZGiTgLMKThlcQ5odnhzGOIxEZn54xT8y60LTIIaPEFQ8uHNBwQd5GyIV6NKslEPnJyMzLnoSiMUvVKVbK7LSQLPK1GHVDr6w5iEjbnXiKRBNmue1q/cDpdWpjqRpP/9JYQVYROlLIsqg38TJ5SP/Q8eqqs50RHz7GtShkTfhhqEaJrc2TK68/hE3yvdVRQqvqxx+AurqwjlO8jEKPSYE6gEmYPu3ByKWQFt+i9BVgxd/9mDJr7ZT2GSzVwnlcwmsbebPOU6wrvaxSCViYrLKRBf3VzTohhjM/hjt6YwEMEY1SpzoKQKg+C3qd";    // use your token
            service.RequesterCredentials.Credentials = new UserIdPasswordType();
            service.RequesterCredentials.Credentials.AppId = appId;
            service.RequesterCredentials.Credentials.DevId = devId;
            service.RequesterCredentials.Credentials.AuthCert = certId;
            // Make the call to GeteBayOfficialTime
            GeteBayOfficialTimeRequestType request = new GeteBayOfficialTimeRequestType();
            request.Version = "405";
            GeteBayOfficialTimeResponseType response = service.GeteBayOfficialTime(request);
            Console.WriteLine("The time at eBay headquarters in San Jose, California, USA, is:");
            Console.WriteLine(response.Timestamp);
        }
    }
}