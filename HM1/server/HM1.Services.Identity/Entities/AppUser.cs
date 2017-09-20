using AppCore.Services.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HM1.Services.Identity
{
    public class AppUser : BaseAppUser
    {

        public virtual ICollection<AppTenant> AppTenants { get; set; } = new HashSet<AppTenant>();


    }
}
