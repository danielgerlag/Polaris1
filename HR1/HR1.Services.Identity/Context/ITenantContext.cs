using AppCore.DomainModel.Abstractions;
using AppCore.Services.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR1.Services.Identity
{
    public interface ITenantContext : IAppIdentityContext<AppTenant, AppUser, string, IdentityRole, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
    }
}
