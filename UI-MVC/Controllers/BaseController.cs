using IP_8IEN.UI_MVC.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace IP_8IEN.UI_MVC.Controllers
{
    [Internationalization]
    public class BaseController : Controller
    {
        public ActionResult CookieHandler()
        {
            // check if a cookie exists and if it does load it
            string lang = Server.HtmlEncode(Request.Cookies["lang"]?.Value);

            // if cookie exists
            if (lang != null)
            {
                //read cookie
                Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
            else
            {
                // niet zeker wat we hier hadden gedaan met onderzoekstopics maar deze code stond in comment:
                /*HttpCookie cookie = new HttpCookie("language");
                cookie.Value = lang;
                Response.Cookies.Add(cookie);*/
                //throw new Exception(Resources.CookieNotFound);
            }

            return Redirect("Index");
        }
    }
}