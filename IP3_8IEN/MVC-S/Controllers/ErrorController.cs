using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static IP3_8IEN.UI.MVC_S.FilterConfig;

namespace IP3_8IEN.UI.MVC_S.Controllers
{
    [NoDirectAccess] // Kijk naar FilterConfig.cs
    public class ErrorController : Controller
    {
        public ViewResult Index() => View("Error");
        
        public ViewResult BadRequest()
        {
            Response.StatusCode = 400;
            return View();
        }

        public ViewResult NotAllowed()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
        
        public ViewResult Internal()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}