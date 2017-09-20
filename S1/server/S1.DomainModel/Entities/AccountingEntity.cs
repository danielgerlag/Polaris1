using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Intercepts;

namespace S1.DomainModel
{    
    public class AccountingEntity : BaseAccountingEntity<Party, Contract, JournalTemplate, ScheduledJournal, LedgerAccount>
    {

        public virtual ICollection<WorkItem> WorkItems { get; set; } = new HashSet<WorkItem>();
    }
}
