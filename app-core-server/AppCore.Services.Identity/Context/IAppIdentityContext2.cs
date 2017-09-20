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
    public interface IAppIdentityContext : IDisposable
    {
        bool UserCanAccessTenant(string userName, int appTenantID);
        void MarkPasswordNotExpired(string userId);
        string GetRoleIdByCode(string code);
    }

    public interface IAppIdentityContext<TAppTenant, TUser, TKey, TRole, TUserLogin, TUserRole, TUserClaim> : IAppIdentityContext
        where TAppTenant : BaseAppTenant
        where TUser : IdentityUser<TKey, TUserLogin, TUserRole, TUserClaim>, IUser
        where TRole : IdentityRole<TKey, TUserRole>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserClaim : IdentityUserClaim<TKey>

    {
        IDbSet<TAppTenant> AppTenants { get; set; }
        
        IDbSet<TRole> Roles { get; set; }
        IDbSet<TUser> Users { get; set; }
    }
}
