using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Identity
{
    public abstract class AppIdentityContext<TAppTenant, TUser, TKey, TRole, TUserLogin, TUserRole, TUserClaim> 
        : IdentityDbContext<TUser, TRole, TKey, TUserLogin, TUserRole, TUserClaim>,
          IAppIdentityContext<TAppTenant, TUser, TKey, TRole, TUserLogin, TUserRole, TUserClaim>
        where TAppTenant : BaseAppTenant        
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>, IUser
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>

    {
        public AppIdentityContext()
            : base("DB")
        {
        }

        public IDbSet<TAppTenant> AppTenants { get; set; }

        public abstract bool UserCanAccessTenant(string userName, int appTenantID);
        public abstract void MarkPasswordNotExpired(string userId);
        public abstract string GetRoleIdByCode(string code);
    }
}
