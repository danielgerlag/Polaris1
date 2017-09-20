using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR1.Services.Identity
{
    public class TenantInit : CreateDatabaseIfNotExists<TenantContext>
    {
        protected override void Seed(TenantContext context)
        {
            base.Seed(context);
        }
    }
}

