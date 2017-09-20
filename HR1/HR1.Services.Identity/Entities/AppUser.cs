using AppCore.Services.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR1.Services.Identity
{
    public class AppUser : BaseAppUser
    {

        public int AppTenantID { get; set; }
        public virtual AppTenant AppTenant { get; set; }


    }
}
