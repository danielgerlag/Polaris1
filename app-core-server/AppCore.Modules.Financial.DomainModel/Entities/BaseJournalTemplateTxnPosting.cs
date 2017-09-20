using AppCore.DomainModel.Abstractions.Entities;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{

    public abstract class BaseJournalTemplateTxnPosting : TenantEntity
    {
        [Required]
        [MaxLength(1)]
        public string PostType { get; set; }

        [Required]
        public bool AddBaseAmount { get; set; }

    }

    public abstract class BaseJournalTemplateTxnPosting<TJournalTemplateTxn, TLedgerAccount> : BaseJournalTemplateTxnPosting
        where TJournalTemplateTxn : BaseJournalTemplateTxn
        where TLedgerAccount : BaseLedgerAccount
    {

        public int JournalTemplateTxnID { get; set; }
        public virtual TJournalTemplateTxn JournalTemplateTxn { get; set; }

        public int LedgerAccountID { get; set; }
        public virtual TLedgerAccount LedgerAccount { get; set; }

        
    }
}
