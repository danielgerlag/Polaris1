using AppCore.Services.Identity.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace S1.Services.Identity.Services
{
    public class AppUserManager : UserManager<AppUser, string>
    {        

        public AppUserManager(UserStore<AppUser, AppRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim> store)
            :base(store)
        {
            
        }

        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
        }
        
    }
}
