using AppCore.Services.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace S1.Services.Identity
{
    public class AppTenantUser 
    {
        [Key]
        public int ID { get; set; }

        public string AppUserID { get; set; }
        public virtual AppUser AppUser { get; set; }

        public int AppTenantID { get; set; }
        public virtual AppTenant AppTenant { get; set; }

        public string AppRoleID { get; set; }
        public virtual AppRole AppRole { get; set; }        

    }
}
