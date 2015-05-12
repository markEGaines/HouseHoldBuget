using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HouseHoldBuget.Startup))]
namespace HouseHoldBuget
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
