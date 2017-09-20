using AppCore.API.Controllers;
using AppCore.API.Models.Account;
using AppCore.Services.Identity;
using HM1.API.Models.Account;
using HM1.DomainModel;
using HM1.DomainModel.Context;
using HM1.Services.Identity;
using HM1.Services.Identity.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HM1.API.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : BaseAccountController<AppUserManager, AppSignInManager, AppUser>
    {

        public AccountController(IHM1Context appContext, IAppIdentityContext identityContext)
            : base(appContext, identityContext)
        {

        }


        protected override UserLoginResponse BuildLoginResponse(AppUser user)
        {
            var result = new UserLoginResponse();

            foreach (var apt in user.AppTenants.ToList())
            {
                result.Tenants.Add(new TenantInfo()
                {
                    ID = apt.ID,
                    Name = apt.Name
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
                IHM1Context db = AppCore.IoC.Container.Resolve<IHM1Context>();

                try
                {

                    AppTenant tenant = new AppTenant();
                    tenant.Name = model.PropertyName;
                    var user = new AppUser() { UserName = model.Email, Email = model.Email };
                    user.AppTenants.Add(tenant);

                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        //scope.Dispose();
                        return GetErrorResult(result);
                    }

                    var tenantParty = new Party() { AppTenantID = tenant.ID, Name = model.PropertyName, PartyType = "C" };
                    var region = db.Regions.Single(x => x.Code == "BC"); //temp
                    var tentantEntity = new AccountingEntity() { AppTenantID = tenant.ID, Party = tenantParty, Region = region };
                    db.AccountingEntities.Add(tentantEntity);

                    var userParty = new Party() { AppTenantID = tenant.ID, FirstName = model.FirstName, Surname = model.Surname, PartyType = "I" };
                    var userParticipant = new Participant() { AppTenantID = tenant.ID, Party = userParty };

                    db.Participants.Add(userParticipant);

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
