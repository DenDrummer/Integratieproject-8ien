using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

namespace IP_8IEN.UI_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            string lang = CultureInfo.CurrentCulture.ToString();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DefaultLocalized",
                url: "{language}-{culture}/{controller}/{action}/{id}",
                defaults: new
                {
                    language = lang.Split('-')[0],
                    culture = lang.Split('-')[1],
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
