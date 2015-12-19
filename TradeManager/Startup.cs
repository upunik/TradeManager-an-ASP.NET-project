using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TradeManager.Startup))]
namespace TradeManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
