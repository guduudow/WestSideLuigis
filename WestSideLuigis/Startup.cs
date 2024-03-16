using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WestSideLuigis.Startup))]
namespace WestSideLuigis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
