using AppCore.DomainModel.Interface;

namespace AppCore.Modules.Financial.DomainModel.Services.Interfaces
{
    public interface IJournalBuilder
    {
        BaseJournal BuildJournal(IDbContext db, BaseScheduledJournal scheduledJournal, IPartyRole resolvedRole, decimal percentage);
    }
}