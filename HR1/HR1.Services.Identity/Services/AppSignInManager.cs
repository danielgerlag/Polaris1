using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace HR1.Services.Identity.Services
{
    public class AppSignInManager : SignInManager<AppUser, string>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        {
         
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AppUser user)
        {
            return user.GenerateUserIdentityAsync<AppUserManager, AppUser>((AppUserManager)UserManager, DefaultAuthenticationTypes.ApplicationCookie);
        }

        //public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        //{
        //    return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        //}
    }
}
