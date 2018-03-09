using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(V2.Startup))]
namespace V2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
