using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IP_8IEN.UI_MVC.Startup))]
namespace IP_8IEN.UI_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
