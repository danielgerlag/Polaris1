using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Identity
{
    public abstract class BaseAppUser : IdentityUser
    {

        public bool PasswordExpired { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync<TManager, TUser>(TManager manager, string authenticationType)
            where TManager : UserManager<TUser, string>
            where TUser : BaseAppUser, IUser<string>
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this as TUser, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
