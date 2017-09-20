using AppCore.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.Services.Identity
{
    public class AppTenant : BaseAppTenant
    {
        public virtual ICollection<AppUser> AppUsers { get; set; } = new HashSet<AppUser>();
    }
}
