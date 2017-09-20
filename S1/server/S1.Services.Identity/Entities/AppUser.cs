using AppCore.Services.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace S1.Services.Identity
{
    public class AppUser : BaseAppUser
    {

        public virtual ICollection<AppTenantUser> AppTenantUsers { get; set; } = new HashSet<AppTenantUser>();

    }
}
