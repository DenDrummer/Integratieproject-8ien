using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace IP_8IEN.UI_MVC.Controllers
{
    public class LanguageController : BaseController
    {
        public ActionResult ChangeLang(string newLang)
        {
            #region Change language
            Thread.CurrentThread.CurrentCulture = new CultureInfo(newLang);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            #endregion

            #region save in cookie
            HttpCookie cookie = new HttpCookie("lang");
            cookie.Value = newLang;
            //save the cookie only one hour for testing purposes
            cookie.Expires = DateTime.Now.AddHours(1);
            HttpContext.Response.SetCookie(cookie);
            #endregion

            #region create new URL
            // get original url without https://
            //  (for example www.8ien.kdg.be/Home/Index )
            string uri = Request.UrlReferrer.PathAndQuery;

            //split into parameters
            string[] uriParams = uri.Split('/');

            //start creating new url
            StringBuilder newUri = new StringBuilder();

            //start with the Path with a / at the end(for example www.8ien.kdg.be/ )
            newUri.Append($"{uriParams[0]}");

            //add the language parameter (default nl-NL/ )
            newUri.Append($"{newLang}");

            //make Regular Expression to check for pre-existing language parameter
            Regex langRegex = new Regex("^[a-z]{2}-[A-Z]{2}");

            for (int i = 1; i < uriParams.Length; i++)
            {
                //if it's not a language parameter
                if (!langRegex.IsMatch(uriParams[i]))
                {
                    //append another slash
                    newUri.Append('/');
                    //append the parameter
                    newUri.Append(uriParams[i]);
                }
            }

            #endregion
            return Redirect(newUri.ToString());
        }
    }
}