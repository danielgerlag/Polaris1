using AppCore.DomainModel.Abstractions.Entities;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{

    public abstract class BaseAccountingEntity : TenantEntity
    {

        public int RegionID { get; set; }
        public virtual Region Region { get; set; }

        public virtual ICollection<SequenceNumber> SequenceNumbers { get; set; } = new HashSet<SequenceNumber>();

    }

    public abstract class BaseAccountingEntity<TParty, TContract, TJournalTemplate, TScheduledJournal, TLedgerAccount> : BaseAccountingEntity
        where TParty : BaseParty
        where TContract : BaseContract
        where TJournalTemplate : BaseJournalTemplate
        where TScheduledJournal: BaseScheduledJournal
        where TLedgerAccount: BaseLedgerAccount
    {
        public int PartyID { get; set; }
        public virtual TParty Party { get; set; }


        public virtual ICollection<TContract> Contracts { get; set; } = new HashSet<TContract>();

        public virtual ICollection<TJournalTemplate> JournalTemplates { get; set; } = new HashSet<TJournalTemplate>();

        public virtual ICollection<TScheduledJournal> ScheduledJournals { get; set; } = new HashSet<TScheduledJournal>();

        public virtual ICollection<TLedgerAccount> LedgerAccounts { get; set; } = new HashSet<TLedgerAccount>();


    }
}
