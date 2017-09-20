using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.Context
{    
    public interface IFinancialContext<TAccountingEntity, TContract, TJournalTemplate, TScheduledJournal, TJournal, TJournalTxn, TJournalTemplateInput, TScheduledJournalInputValue, TLedgerTxn, TJournalTemplateTxn, TJournalTemplateTxnPosting, TLedgerAccount>
        where TAccountingEntity : BaseAccountingEntity
        where TContract : BaseContract
        where TJournalTemplate : BaseJournalTemplate        
        where TScheduledJournal : BaseScheduledJournal
        where TJournal : BaseJournal
        where TJournalTxn : BaseJournalTxn
        where TJournalTemplateInput : BaseJournalTemplateInput
        where TScheduledJournalInputValue : BaseScheduledJournalInputValue
        where TLedgerTxn : BaseLedgerTxn
        where TJournalTemplateTxn : BaseJournalTemplateTxn
        where TJournalTemplateTxnPosting : BaseJournalTemplateTxnPosting
        where TLedgerAccount : BaseLedgerAccount
    {
        IDbSet<TAccountingEntity> AccountingEntities { get; set; }
        IDbSet<TContract> Contracts { get; set; }

        IDbSet<TJournalTemplate> JournalTemplates { get; set; }

        IDbSet<TScheduledJournal> ScheduledJournals { get; set; }

        IDbSet<JournalType> JournalTypes { get; set; }

        IDbSet<TJournal> Journals { get; set; }

        IDbSet<TJournalTxn> JournalTxns { get; set; }

        IDbSet<Ledger> Ledgers { get; set; }

        IDbSet<TLedgerAccount> LedgerAccounts { get; set; }

        IDbSet<LedgerAccountType> LedgerAccountTypes { get; set; }

        IDbSet<TLedgerTxn> LedgerTxns { get; set; }

        IDbSet<TJournalTemplateInput> JournalTemplateInputs { get; set; }

        IDbSet<TJournalTemplateTxnPosting> JournalTemplateTxnPostings { get; set; }

        IDbSet<TJournalTemplateTxn> JournalTemplateTxns { get; set; }

        IDbSet<TScheduledJournalInputValue> ScheduledJournalInputValues { get; set; }

        IDbSet<Region> Regions { get; set; }
    }
}

