using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1.Services.Identity
{
    public class TenantInit : CreateDatabaseIfNotExists<TenantContext>
    {
        protected override void Seed(TenantContext context)
        {

            context.Roles.Add(new AppRole() { Code = "CM", Name = "Council Member", IsCouncilMember = true });
                        
            base.Seed(context);
        }
    }
}

