using AppCore.DomainModel.Abstractions;
using AppCore.Services.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1.Services.Identity
{
    public interface ITenantContext : IAppIdentityContext<AppTenant, AppUser, string, AppRole, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
    }
}
