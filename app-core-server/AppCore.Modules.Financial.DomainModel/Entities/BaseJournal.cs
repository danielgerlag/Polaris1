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

    public abstract class BaseJournal : TenantEntity
    {

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }                

        public int JournalTypeID { get; set; }
        public virtual JournalType JournalType { get; set; }

        [MaxLength(100)]
        public string Reference { get; set; }

        public DateTime TxnDate { get; set; }

        public decimal? Amount { get; set; }

    }

    public abstract class BaseJournal<TAccountingEntity, TJournalTxn> : BaseJournal
        where TAccountingEntity : BaseAccountingEntity
        where TJournalTxn : BaseJournalTxn
    {

        public int AccountingEntityID { get; set; }
        public virtual TAccountingEntity AccountingEntity { get; set; }

        public virtual ICollection<TJournalTxn> JournalTxns { get; set; } = new HashSet<TJournalTxn>();

    }
}
