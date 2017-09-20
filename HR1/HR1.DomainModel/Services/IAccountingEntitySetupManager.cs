using HR1.DomainModel.Context;

namespace HR1.DomainModel.Services
{
    public interface IAccountingEntitySetupManager
    {
        void Run(IHR1Context db, AccountingEntity accountingEntity);
    }
}