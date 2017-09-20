using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.API.Models.Account
{
    public class UserLoginResponse
    {
        public bool PasswordExpired { get; set; }
        public List<TenantInfo> Tenants { get; set; } = new List<TenantInfo>();
    }
}
