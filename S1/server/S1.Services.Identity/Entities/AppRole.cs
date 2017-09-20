using AppCore.Services.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace S1.Services.Identity
{
    public class AppRole : IdentityRole
    {
        [MaxLength(10)]
        public string Code { get; set; }
        
        public bool IsPropertyTenant { get; set; }

        public bool IsPropertyOwner { get; set; }

        public bool IsManagingAgent { get; set; }

        public bool IsCouncilMember { get; set; }

    }
}
