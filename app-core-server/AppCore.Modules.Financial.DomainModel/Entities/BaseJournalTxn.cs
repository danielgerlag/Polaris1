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

    public abstract class BaseJournalTxn : TenantEntity
    {

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }        

        public DateTime TxnDate { get; set; }

        public decimal Amount { get; set; }

        public bool IncludeInTotal { get; set; }

    }

    public abstract class BaseJournalTxn<TJournal, TLedgerTxn, TJournalTemplateTxn, TParty> : BaseJournalTxn
        where TJournal : BaseJournal
        where TLedgerTxn : BaseLedgerTxn
        where TJournalTemplateTxn : BaseJournalTemplateTxn
        where TParty : BaseParty
    {

        public int JournalID { get; set; }
        public virtual TJournal Journal { get; set; }

        public int JournalTemplateTxnID { get; set; }
        public virtual TJournalTemplateTxn JournalTemplateTxn { get; set; }

        public int? PartyID { get; set; }
        public virtual TParty Party { get; set; }

        public virtual ICollection<TLedgerTxn> LedgerTxns { get; set; } = new HashSet<TLedgerTxn>();

    }
}
