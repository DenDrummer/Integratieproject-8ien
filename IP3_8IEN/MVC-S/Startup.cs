using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(MVC_S.Startup))]
namespace MVC_S
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureAuth(app);
            app.UseWebApi(config);
        }
    }
}
