using System;
using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Services;
using System.Web;
using AppCore.Services.Identity;
using AppCore.Services.Identity.Services;
using AppCore.DomainModel.Abstractions.Entities;
using AppCore.DomainModel.Interface;

namespace AppCore.API.Services
{
    public abstract class TenantDataService<TContext> : EntityFrameworkDataService<TContext>
        where TContext : IDbContext
    {

        private ITenantFilter _tenantFilter;
        private IAppIdentityContext _appIdentityContext;

        protected ITenantFilter TenantFilter
        {
            get
            {
                if (_tenantFilter == null)
                    _tenantFilter = IoC.Container.Resolve<ITenantFilter>();
                return _tenantFilter;
            }
        }

        protected IAppIdentityContext AppIdentityContext
        {
            get
            {
                if (_appIdentityContext == null)
                    _appIdentityContext = IoC.Container.Resolve<IAppIdentityContext>();
                return _appIdentityContext;
            }
        }        

        protected override TContext CreateDataSource()
        {
            return IoC.Container.Resolve<TContext>();
        }

        protected override void OnStartProcessingRequest(ProcessRequestArgs args)
        {
            var httpReq = HttpContext.Current.Request;            
            string sTenant = httpReq.Headers["TenantID"];
            if (!String.IsNullOrEmpty(sTenant))
            {
                int provisionalID = Convert.ToInt32(sTenant);
                string userName = HttpContext.Current.User.Identity.Name;
                if (AppIdentityContext.UserCanAccessTenant(userName, provisionalID))
                    TenantFilter.AppTenantID = provisionalID;
                else
                    TenantFilter.AppTenantID = 0;

            }
            else
            {
                TenantFilter.AppTenantID = 0;
            }

            base.OnStartProcessingRequest(args);
        }

        protected virtual void InterceptChange(TenantEntity entity, UpdateOperations operations)
        {
            if (TenantFilter.AppTenantID == 0)
                throw new HttpRequestValidationException();

            if (operations == UpdateOperations.Add)
            {
                entity.AppTenantID = TenantFilter.AppTenantID;
            }

            if (operations == UpdateOperations.Change)
            {
                int originalTenant = CurrentDataSource.Entry(entity).OriginalValues.GetValue<int>("AppTenantID");

                if (originalTenant != TenantFilter.AppTenantID)
                    throw new HttpRequestValidationException();

                if (originalTenant != entity.AppTenantID)
                    throw new Exception("Entity cannot move across tenants");
            }

            if (operations == UpdateOperations.Delete)
            {
                int originalTenant = CurrentDataSource.Entry(entity).OriginalValues.GetValue<int>("AppTenantID");

                if (originalTenant != TenantFilter.AppTenantID)
                    throw new HttpRequestValidationException();                
            }
        }

    }
}
