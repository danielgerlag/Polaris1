using AppCore.Modules.Financial.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Services.Interfaces;
using AppCore.Modules.Financial.DomainModel;

namespace HM1.DomainModel.Services
{
    public class ScheduledJournalRunner : BaseScheduledJournalRunner<ScheduledJournal>
    {
        public ScheduledJournalRunner(IJournalPoster journalPoster) 
            : base(journalPoster)
        {
        }

        protected override IJournalBuilder SelectJournalBuilder(IDbContext db, ScheduledJournal scheduledJournal)
        {
            return AppCore.IoC.Container.Resolve<JournalBuilder>();
        }

        protected override IPartyRole SelectContractParty(IDbContext db, ScheduledJournal scheduledJournal)
        {
            return scheduledJournal.Contract.Party;
        }
    }
}
