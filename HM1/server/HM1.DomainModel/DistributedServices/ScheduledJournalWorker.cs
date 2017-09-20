using AppCore.Modules.Financial.DomainModel.DistributedServices;
using HM1.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel.Services.Interfaces;

namespace HM1.DomainModel.DistributedServices
{
    public class ScheduledJournalWorker : BaseScheduledJournalWorker<ScheduledJournal, IHM1Context>
    {
        public ScheduledJournalWorker(IScheduledJournalRunner runner) 
            : base(runner)
        {
        }
    }
}
