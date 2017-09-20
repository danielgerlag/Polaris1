using AppCore.API.Controllers;
using HR1.Services.Identity;
using HR1.Services.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppCore.API.Models.Account;
using System.Threading.Tasks;
using HR1.API.Models.Account;
using HR1.DomainModel.Context;
using Microsoft.AspNet.Identity;
using HR1.DomainModel;
using AppCore.Services.Identity;
using AppCore.DomainModel.Interface;

namespace HR1.API.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : BaseAccountController<AppUserManager, AppSignInManager, AppUser>
    {

        public AccountController(IHR1Context appContext, IAppIdentityContext identityContext)
            : base (appContext, identityContext)
        {

        }
        

        protected override UserLoginResponse BuildLoginResponse(AppUser user)
        {
            var result = new UserLoginResponse();
            result.Tenants.Add(new TenantInfo()
            {
                ID = user.AppTenant.ID,
                Name = user.AppTenant.Name
            });
            return result;
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
                        

            //using (TransactionScope scope = new TransactionScope())
            {
                IHR1Context db = AppCore.IoC.Container.Resolve<IHR1Context>();

                try
                {

                    AppTenant tenant = new AppTenant();
                    tenant.Name = model.CompanyName;
                    var user = new AppUser() { UserName = model.Email, Email = model.Email, AppTenant = tenant };
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        //scope.Dispose();
                        return GetErrorResult(result);
                    }

                    var tenantParty = new Party() { AppTenantID = tenant.ID, Name = model.CompanyName, PartyType = "C" };
                    var tentantEntity = new AccountingEntity() { AppTenantID = tenant.ID, Party = tenantParty };
                    db.AccountingEntities.Add(tentantEntity);
                    await db.SaveChangesAsync();                                        

                    //scope.Complete();
                }
                catch (Exception ex)
                {
                    //scope.Dispose();
                    throw ex;
                }
            }

            return Ok();

        }

        [Route("Login")]
        public override Task<IHttpActionResult> Login(UserLoginRequest model)
        {
            return base.Login(model);
        }

        [Route("Logout")]
        public override IHttpActionResult Logout()
        {
            return base.Logout();
        }

        [Route("Resume")]
        public async override Task<IHttpActionResult> Resume()
        {
            return await base.Resume();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

    }
}
