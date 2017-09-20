using System;
using System.Linq.Expressions;
using AppCore.DomainModel.Abstractions.Entities;

namespace AppCore.Services.Identity.Services
{
    public interface ITenantFilter
    {
        int AppTenantID { get; set; }

        Expression<Func<TEntity, bool>> BuildFilter<TEntity>() where TEntity : TenantEntity;
    }
}