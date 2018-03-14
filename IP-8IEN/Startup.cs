using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IP_8IEN.Startup))]
namespace IP_8IEN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
