using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IP_8IEN.UI.MVC_S.Startup))]
namespace IP_8IEN.UI.MVC_S
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) => ConfigureAuth(app);
    }
}
