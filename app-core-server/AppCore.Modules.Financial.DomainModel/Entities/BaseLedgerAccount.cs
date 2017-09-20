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

    public abstract class BaseLedgerAccount : TenantEntity
    {

        public int LedgerAccountTypeID { get; set; }
        public virtual LedgerAccountType LedgerAccountType { get; set; }

        public int LedgerID { get; set; }
        public virtual Ledger Ledger { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

    }

    public abstract class BaseLedgerAccount<TAccountingEntity> : BaseLedgerAccount
        where TAccountingEntity : BaseAccountingEntity       
    {

        public int AccountingEntityID { get; set; }
        public virtual TAccountingEntity AccountingEntity { get; set; }               

    }
}
