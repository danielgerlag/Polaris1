using AppCore.Modules.Financial.DomainModel.Context;
using AppCore.Modules.Financial.DomainModel.Services.Models;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.Services
{
    public abstract class BaseLedgerQueryService<TContext, TLedgerAccountBalanceRequest, TLedgerAccountBalance, TAccountingEntity, TContract, TJournalTemplate, TScheduledJournal, TJournal, TJournalTxn, TJournalTemplateInput, TScheduledJournalInputValue, TLedgerTxn, TJournalTemplateTxn, TJournalTemplateTxnPosting, TLedgerAccount, TParty>
        where TContext : IFinancialContext<TAccountingEntity, TContract, TJournalTemplate, TScheduledJournal, TJournal, TJournalTxn, TJournalTemplateInput, TScheduledJournalInputValue, TLedgerTxn, TJournalTemplateTxn, TJournalTemplateTxnPosting, TLedgerAccount>
        where TAccountingEntity : BaseAccountingEntity<TParty, TContract, TJournalTemplate, TScheduledJournal, TLedgerAccount>
        where TContract : BaseContract
        where TJournalTemplate : BaseJournalTemplate
        where TScheduledJournal : BaseScheduledJournal
        where TJournal : BaseJournal
        where TJournalTxn : BaseJournalTxn
        where TJournalTemplateInput : BaseJournalTemplateInput
        where TScheduledJournalInputValue : BaseScheduledJournalInputValue
        where TLedgerTxn : BaseLedgerTxn<TJournalTxn, TLedgerAccount, TParty, TAccountingEntity>
        where TJournalTemplateTxn : BaseJournalTemplateTxn
        where TJournalTemplateTxnPosting : BaseJournalTemplateTxnPosting
        where TLedgerAccountBalance : LedgerAccountBalance, new()
        where TLedgerAccountBalanceRequest : LedgerAccountBalanceRequest
        where TLedgerAccount : BaseLedgerAccount
        where TParty : BaseParty
    {


        public virtual IQueryable<TLedgerAccountBalance> GetLedgerAccountBalances(TContext context, TLedgerAccountBalanceRequest request)
        {
            var query = context.LedgerTxns.Where(x => x.AppTenantID == request.AppTenantID && x.TxnDate <= request.EffectiveDate && x.LedgerAccount.LedgerID == request.LedgerID);

            if (request.AccountingEntityID.HasValue)
                query = query.Where(x => x.AccountingEntityID == request.AccountingEntityID);

            return FlattenResults(query);
        }

        protected IQueryable<TLedgerAccountBalance> FlattenResults(IQueryable<TLedgerTxn> query)
        {
            return query
                .GroupBy(x => new { x.AccountingEntity, x.LedgerAccount })
                .Select(x => new TLedgerAccountBalance()
                {
                    Balance = x.Sum(t => t.Amount),
                    LedgerAccountID = x.Key.LedgerAccount.ID,
                    LedgerAccountName = x.Key.LedgerAccount.Name,
                    LedgerAccountType = x.Key.LedgerAccount.LedgerAccountType.Name,
                    AccountingEntityID = x.Key.AccountingEntity.ID,
                    AccountingEntityName = x.Key.AccountingEntity.Party.Name
                });
        }

    }
}
