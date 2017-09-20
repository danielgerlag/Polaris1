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

    public abstract class BaseLedgerTxn : TenantEntity
    {                

        public DateTime TxnDate { get; set; }

        public decimal Amount { get; set; }

    }

    public abstract class BaseLedgerTxn<TJournalTxn, TLedgerAccount, TParty, TAccountingEntity> : BaseLedgerTxn
        where TJournalTxn : BaseJournalTxn
        where TLedgerAccount : BaseLedgerAccount
        where TParty : BaseParty
        where TAccountingEntity : BaseAccountingEntity
    {

        public int AccountingEntityID { get; set; }
        public virtual TAccountingEntity AccountingEntity { get; set; }

        public int JournalTxnID { get; set; }
        public virtual TJournalTxn JournalTxn { get; set; }

        public int LedgerAccountID { get; set; }
        public virtual TLedgerAccount LedgerAccount { get; set; }

        public int? PartyID { get; set; }
        public virtual TParty Party { get; set; }

        // public virtual ICollection<TJournalTemplateTxn> JournalTemplateTxns { get; set; } = new HashSet<TJournalTemplateTxn>();


        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public string OpDescription
        //{
        //    get
        //    {
        //        if (LedgerAccount != null)
        //        {
        //            if (LedgerAccount.LedgerAccountType != null)
        //            {
        //                if (LedgerAccount.LedgerAccountType.CreditPositive)
        //                {
        //                    if (Amount >= 0)
        //                        return "Credit";
        //                    else
        //                        return "Debit";
        //                }
        //                else
        //                {
        //                    if (Amount >= 0)
        //                        return "Debit";
        //                    else
        //                        return "Credit";
        //                }
        //            }
        //        }
        //        return string.Empty;
        //    }
        //    private set { }
        //}

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public decimal AbsAmount
        //{
        //    get
        //    {
        //        return Math.Abs(Amount);
        //    }
        //    private set { }
        //}
    }
}
