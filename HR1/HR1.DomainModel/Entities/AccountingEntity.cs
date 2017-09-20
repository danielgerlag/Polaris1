using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.HR.DomainModel;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Intercepts;
using HR1.DomainModel.Intercepts;

namespace HR1.DomainModel
{
    [Intercept(typeof(AccountingEntitySetupIntercept), Stage.OnAddBeforeCommit, 1)]
    public class AccountingEntity : BaseAccountingEntity<Party, Contract, JournalTemplate, ScheduledJournal, LedgerAccount>,
        IHRAccountingEntity<Employment>  
    {
        //public virtual ICollection<Employment> Employments { get; set; } = new HashSet<Employment>();
    }
}
