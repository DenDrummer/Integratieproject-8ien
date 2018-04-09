using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MVC_S.Models;

using IP3_8IEN.BL;
using IP3_8IEN.BL.Domain;
using MVC_S.SignIn;

namespace MVC_S
{
    /*
    ApplicationUserManager maakt gebruik van DAL -> Hoort thuis in BL!!!
    We maken gebruik van een Factory -> Create methode uit oorspronkelijke ApplicationUserManager gebruikt OWIN
    -> OWIN is UI en mag niet met DAL gecombineeerd worden!
*/
    public class ApplicationUserManagerFactory
    {
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    //new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
                    new DataProtectorTokenProvider<IP3_8IEN.BL.Domain.ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }

    /*
        ApplicationSignInManager maakt gebruik van OWIN en kan niet bestaan zonder OWIN
        -> Bad Practice door Microsoft, heel moeilijk op te lossen...
        ApplicationSignInManager werd verplaatst naar map SignIn in UI_MVC_S
    */
    public class ApplicationSignManagerFactory
    {
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }


    ////public class EmailService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        // Plug in your email service here to send an email.
    //        return Task.FromResult(0);
    //    }
    //}

    //public class SmsService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        // Plug in your SMS service here to send a text message.
    //        return Task.FromResult(0);
    //    }
    //}

    //// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    //public class ApplicationUserManager : UserManager<ApplicationUser>
    //{
    //    public ApplicationUserManager(IUserStore<ApplicationUser> store)
    //        : base(store)
    //    {
    //    }

    //    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
    //    {
    //        var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
    //        // Configure validation logic for usernames
    //        manager.UserValidator = new UserValidator<ApplicationUser>(manager)
    //        {
    //            AllowOnlyAlphanumericUserNames = false,
    //            RequireUniqueEmail = true
    //        };

    //        // Configure validation logic for passwords
    //        manager.PasswordValidator = new PasswordValidator
    //        {
    //            RequiredLength = 6,
    //            RequireNonLetterOrDigit = true,
    //            RequireDigit = true,
    //            RequireLowercase = true,
    //            RequireUppercase = true,
    //        };

    //        // Configure user lockout defaults
    //        manager.UserLockoutEnabledByDefault = true;
    //        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //        manager.MaxFailedAccessAttemptsBeforeLockout = 5;

    //        // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
    //        // You can write your own provider and plug it in here.
    //        manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
    //        {
    //            MessageFormat = "Your security code is {0}"
    //        });
    //        manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
    //        {
    //            Subject = "Security Code",
    //            BodyFormat = "Your security code is {0}"
    //        });
    //        manager.EmailService = new EmailService();
    //        manager.SmsService = new SmsService();
    //        var dataProtectionProvider = options.DataProtectionProvider;
    //        if (dataProtectionProvider != null)
    //        {
    //            manager.UserTokenProvider = 
    //                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
    //        }
    //        return manager;
    //    }
    //}

    //// Configure the application sign-in manager which is used in this application.
    //public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    //{
    //    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
    //        : base(userManager, authenticationManager)
    //    {
    //    }

    //    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    //    {
    //        return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
    //    }

    //    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
    //    {
    //        return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    //    }
    //}
}
