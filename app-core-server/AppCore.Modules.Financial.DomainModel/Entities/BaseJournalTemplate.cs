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

    public abstract class BaseJournalTemplate : TenantEntity
    {

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public int? SequenceNumberID { get; set; }
        public virtual SequenceNumber SequenceNumber { get; set; }

        public int JournalTypeID { get; set; }
        public virtual JournalType JournalType { get; set; }                

        [MaxLength(50)]
        public string OriginKey { get; set; }

        //public abstract BaseJournalTemplateInput ResolveReferenceInput();
        //public abstract IEnumerable<BaseJournalTemplateTxn> ResolveTxns();

    }

    public abstract class BaseJournalTemplate<TAccountingEntity, TJournalTemplateTxn, TJournalTemplateInput> : BaseJournalTemplate
        where TAccountingEntity : BaseAccountingEntity
        where TJournalTemplateTxn : BaseJournalTemplateTxn
        where TJournalTemplateInput : BaseJournalTemplateInput
    {

        public int AccountingEntityID { get; set; }
        public virtual TAccountingEntity AccountingEntity { get; set; }

        public int? ReferenceInputID { get; set; }
        public virtual TJournalTemplateInput ReferenceInput { get; set; }

        public virtual ICollection<TJournalTemplateTxn> JournalTemplateTxns { get; set; } = new HashSet<TJournalTemplateTxn>();

        public virtual ICollection<TJournalTemplateInput> UserInputs { get; set; } = new HashSet<TJournalTemplateInput>();

        //public override BaseJournalTemplateInput ResolveReferenceInput()
        //{
        //    return ReferenceInput;
        //}

        //public override IEnumerable<BaseJournalTemplateTxn> ResolveTxns()
        //{
        //    return JournalTemplateTxns.Cast<BaseJournalTemplateTxn>();
        //}

    }
}
