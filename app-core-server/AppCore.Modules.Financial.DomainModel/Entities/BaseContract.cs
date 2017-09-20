using AppCore.DomainModel.Abstractions.Entities;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{

    public abstract class BaseContract : TenantEntity
    {
    }

    public abstract class BaseContract<TAccountingEntity, TParty, TScheduledJournal> : BaseContract
        where TParty : BaseParty
        where TAccountingEntity : BaseAccountingEntity
        where TScheduledJournal : BaseScheduledJournal
    {

        public int AccountingEntityID { get; set; }
        public virtual TAccountingEntity AccountingEntity { get; set; }
        
        public int PartyID { get; set; }
        public virtual TParty Party { get; set; }

        [MaxLength(50)]
        public string Number { get; set; }

        public DateTime StartDate { get; set; }

        public virtual ICollection<TScheduledJournal> ScheduledJournals { get; set; } = new HashSet<TScheduledJournal>();
    }
}
