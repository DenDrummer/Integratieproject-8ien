using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_S.Startup))]
namespace MVC_S
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
