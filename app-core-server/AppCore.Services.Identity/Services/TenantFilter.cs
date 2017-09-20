using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Identity.Services
{
    public class TenantFilter : ITenantFilter
    {

        public int AppTenantID { get; set; }

        public Expression<Func<TEntity, bool>> BuildFilter<TEntity>()
            where TEntity : TenantEntity
        {
            return (x => x.AppTenantID == AppTenantID);            
        }

    }
}
