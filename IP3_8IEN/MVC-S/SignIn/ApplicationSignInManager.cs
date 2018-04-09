using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC_S.SignIn
{
    // Vroeger stond dit in IdentityConfig
    // Normaal gezien moet dit in BL -> OWIN mag niet in BL
    // -> IAuthentication is OWIN en zorgt voor dit probleem
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
    }
}