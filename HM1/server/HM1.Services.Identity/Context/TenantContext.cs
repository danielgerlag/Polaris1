using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.DomainModel.Abstractions;
using AppCore.Services.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HM1.Services.Identity
{
    public class TenantContext : AppIdentityContext<AppTenant, AppUser, string, IdentityRole, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>, ITenantContext
    {
        
        public TenantContext()
            : base()
        {
            Database.SetInitializer<TenantContext>(new TenantInit());
        }

        public override bool UserCanAccessTenant(string userName, int appTenantID)
        {
            var user = Users.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
                return false;

            return (user.AppTenants.Any(x => x.ID == appTenantID));
        }

        public override void MarkPasswordNotExpired(string userId)
        {
            var user = Users.FirstOrDefault(x => x.Id == userId);

            if (user != null)
            {
                user.PasswordExpired = false;
                SaveChanges();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("mt");
            base.OnModelCreating(modelBuilder);
        }

        public override string GetRoleIdByCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}
