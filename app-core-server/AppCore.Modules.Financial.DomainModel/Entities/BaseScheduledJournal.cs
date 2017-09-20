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
    public abstract class BaseScheduledJournal : TenantEntity
    {
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        public DateTime? TxnDate { get; set; }

        public DateTime? NextExecutionDate { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool OnHold { get; set; }

        public bool Archived { get; set; }

        public bool Error { get; set; }

        [MaxLength(2)]
        public string Frequency { get; set; }

        public virtual ICollection<ScheduledJournalLog> Logs { get; set; } = new HashSet<ScheduledJournalLog>();

    }

    public abstract class BaseScheduledJournal<TAccountingEntity, TContract, TJournalTemplate, TScheduledJournalInputValue> : BaseScheduledJournal
        where TAccountingEntity : BaseAccountingEntity
        where TContract : BaseContract
        where TJournalTemplate : BaseJournalTemplate
        where TScheduledJournalInputValue: BaseScheduledJournalInputValue
    {

        public int AccountingEntityID { get; set; }
        public virtual TAccountingEntity AccountingEntity { get; set; }

        public int? ContractID { get; set; }
        public virtual TContract Contract { get; set; }

        public int JournalTemplateID { get; set; }
        public virtual TJournalTemplate JournalTemplate { get; set; }

        public virtual ICollection<TScheduledJournalInputValue> UserInputValues { get; set; } = new HashSet<TScheduledJournalInputValue>();

    }
}
