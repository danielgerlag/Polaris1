using AppCore.DomainModel.Interface;
using AppCore.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppCore.API.Controllers
{
    public abstract class AppApiController : ApiController
    {
        protected IDbContext _appContext;
        protected IAppIdentityContext _identityContext;

        public AppApiController(IDbContext appContext, IAppIdentityContext identityContext)
        {
            _appContext = appContext;
            _identityContext = identityContext;
        }

        protected int GetAppTenantID()
        {
            if (!Request.Headers.Contains("TenantID"))
                return 0;
            
            var header = Request.Headers.First(x => x.Key == "TenantID");            
            string sTenant = header.Value.FirstOrDefault();
            if (!String.IsNullOrEmpty(sTenant))
            {
                int provisionalID = Convert.ToInt32(sTenant);
                string userName = User.Identity.Name;
                if (_identityContext.UserCanAccessTenant(userName, provisionalID))
                    return provisionalID;

            }
            return 0;
        }
    }
}
