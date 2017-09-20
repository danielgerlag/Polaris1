using AppCore.DistributedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.DomainModel.DistributedServices
{
    public class ScheduledJournalPool : WorkerPool
    {
        public ScheduledJournalPool(int threadCount, TimeSpan timeOut) : base(threadCount, timeOut)
        {
        }

        protected override string QueueKey
        {
            get
            {
                return "ScheduledJournalQueue";
            }
        }

        protected override string WorkerKey
        {
            get
            {
                return "ScheduledJournalWorker";
            }
        }
    }
}
