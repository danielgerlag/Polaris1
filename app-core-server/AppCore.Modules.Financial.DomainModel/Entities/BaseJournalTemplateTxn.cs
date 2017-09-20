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

    public abstract class BaseJournalTemplateTxn : TenantEntity
    {

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public decimal? Amount { get; set; }
                
        [MaxLength(10)]
        public string PrimaryFactorSource { get; set; }
                
        [MaxLength(10)]
        public string SecondaryFactorSource { get; set; }

        public bool IncludeInTotal { get; set; }

        public int JournalTxnTypeID { get; set; }
        public virtual JournalTxnType JournalTxnType { get; set; }                

    }

    public abstract class BaseJournalTemplateTxn<TJournalTemplate, TJournalTemplateInput, TJournalTemplateTxnPosting> : BaseJournalTemplateTxn
        where TJournalTemplate : BaseJournalTemplate
        where TJournalTemplateInput : BaseJournalTemplateInput
        where TJournalTemplateTxnPosting : BaseJournalTemplateTxnPosting
    {

        public int JournalTemplateID { get; set; }
        public virtual TJournalTemplate JournalTemplate { get; set; }

        public int? AmountInputID { get; set; }
        public virtual TJournalTemplateInput AmountInput { get; set; }

        public virtual ICollection<TJournalTemplateTxnPosting> Postings { get; set; } = new HashSet<TJournalTemplateTxnPosting>();
    }
}
