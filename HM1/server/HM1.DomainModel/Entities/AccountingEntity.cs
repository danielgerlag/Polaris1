using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Intercepts;
using System.ComponentModel.DataAnnotations;
using HM1.DomainModel.Intercepts;

namespace HM1.DomainModel
{
    [Intercept(typeof(NewProperty), Stage.OnAddBeforeCommit, 1)]
    public class AccountingEntity : BaseAccountingEntity<Party, Contract, JournalTemplate, ScheduledJournal, LedgerAccount>
    {

        [MaxLength(100)]
        public string PropertyName { get; set; }
           
    }
}
