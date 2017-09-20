using AppCore.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1.Services.Identity
{
    public class AppTenant : BaseAppTenant
    {
        public virtual ICollection<AppTenantUser> AppTenantUsers { get; set; } = new HashSet<AppTenantUser>();
    }
}
