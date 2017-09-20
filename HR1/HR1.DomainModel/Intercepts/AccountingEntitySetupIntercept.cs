using AppCore.DomainModel.Abstractions.Intercepts;
using AppCore.DomainModel.Interface;
using HR1.DomainModel.Context;
using HR1.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR1.DomainModel.Intercepts
{
    public class AccountingEntitySetupIntercept : EntityIntercept<AccountingEntity>
    {

        IAccountingEntitySetupManager _setupManager;

        public AccountingEntitySetupIntercept(IAccountingEntitySetupManager setupManager)
        {
            _setupManager = setupManager;
        }

        public override void Run(AccountingEntity entity, IDbContext dataContext)
        {
            _setupManager.Run(dataContext as IHR1Context, entity);
        }

    }
}
