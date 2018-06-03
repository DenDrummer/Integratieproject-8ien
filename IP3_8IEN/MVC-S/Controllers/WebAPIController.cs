using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain.Dashboard;
using IP3_8IEN.BL.Domain.Gebruikers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC_S.Controllers
{
    public class WebAPIController : ApiController
    {
        
        private IDataManager dMgr = new DataManager();
        private IGebruikerManager gMgr = new GebruikerManager();
        private IDashManager dashMgr = new DashManager();
        private ApplicationUserManager aMgr = new ApplicationUserManager();
            [HttpGet]
            [Route("api/GetDashbord")]
            public Dashbord UserDashBoardById(string name){
            //Dashbord van ingelogde gebruiker ophalen
            try
            {
                //ApplicationUser appUser = aMgr.FindById(User.Identity.GetUserId());
                //string userName = appUser.UserName;
                Gebruiker user = gMgr.FindUser(name);

                Dashbord dashbord = dashMgr.GetDashboardWithFollows(user);
                dashbord = dashMgr.UpdateDashboard(dashbord); // <-- zien dat elk DashItem minstens 3h up-to-date is

                var list = JsonConvert.SerializeObject(dashbord,Formatting.None,new JsonSerializerSettings()
                {
                     ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return dashbord;
            }
            catch
            {
                return null;

            }
        }
    }
}
