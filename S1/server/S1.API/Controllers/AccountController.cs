using AppCore.API.Controllers;
using AppCore.API.Models.Account;
using AppCore.Services.Identity;
using Microsoft.AspNet.Identity;
using S1.API.Models.Account;
using S1.DomainModel;
using S1.DomainModel.Context;
using S1.Services.Identity;
using S1.Services.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace S1.API.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : BaseAccountController<AppUserManager, AppSignInManager, AppUser>
    {

        public AccountController(IS1Context appContext, IAppIdentityContext identityContext)
            : base(appContext, identityContext)
        {

        }


        protected override UserLoginResponse BuildLoginResponse(AppUser user)
        {
            var result = new UserLoginResponse();

            foreach (var apt in user.AppTenantUsers.ToList())
            {
                result.Tenants.Add(new TenantInfo()
                {
                    ID = apt.AppTenantID,
                    Name = apt.AppTenant.Name
                });
            }
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
                IS1Context db = AppCore.IoC.Container.Resolve<IS1Context>();

                try
                {
                    string roleId = _identityContext.GetRoleIdByCode("CM");

                    AppTenant tenant = new AppTenant();
                    tenant.Name = model.PropertyName;
                    var user = new AppUser() { UserName = model.Email, Email = model.Email };

                    user.AppTenantUsers.Add(new AppTenantUser()
                    {
                        AppRoleID = roleId,
                        AppTenant = tenant                        
                    });
                    

                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        //scope.Dispose();
                        return GetErrorResult(result);
                    }


                    var region = (_appContext as IS1Context).Regions.Single(x => x.Code == "BC"); //temp

                    var tenantParty = new Party() { AppTenantID = tenant.ID, Name = model.PropertyName, PartyType = "C" };

                    var tentantEntity = new AccountingEntity()
                    {
                        AppTenantID = tenant.ID,
                        Party = tenantParty,
                        Region = region
                    };

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