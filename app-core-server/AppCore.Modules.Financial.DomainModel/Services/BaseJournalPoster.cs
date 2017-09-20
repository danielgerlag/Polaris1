using AppCore.Modules.Financial.DomainModel.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Services.Models;
using AppCore.Modules.Foundation.DomainModel;

namespace AppCore.Modules.Financial.DomainModel.Services
{
    public abstract class BaseJournalPoster<TAccountingEntity, TJournal, TJournalTxn, TLedgerTxn, TJournalTemplate, TJournalTemplateInput, TJournalTemplateTxn, TJournalTemplateTxnPosting, TLedgerAccount, TParty> : IJournalPoster
        where TAccountingEntity : BaseAccountingEntity
        where TJournal : BaseJournal<TAccountingEntity, TJournalTxn>
        where TJournalTxn : BaseJournalTxn<TJournal, TLedgerTxn, TJournalTemplateTxn, TParty>
        where TLedgerTxn : BaseLedgerTxn<TJournalTxn, TLedgerAccount, TParty, TAccountingEntity>, new()
        where TLedgerAccount : BaseLedgerAccount
        where TJournalTemplate : BaseJournalTemplate
        where TJournalTemplateInput : BaseJournalTemplateInput
        where TJournalTemplateTxnPosting : BaseJournalTemplateTxnPosting<TJournalTemplateTxn, TLedgerAccount>
        where TJournalTemplateTxn : BaseJournalTemplateTxn<TJournalTemplate, TJournalTemplateInput, TJournalTemplateTxnPosting>
        where TParty : BaseParty
    {
        public JournalPostResult Post(IDbContext db, BaseJournal journal)
        {
            return Post(db, (journal as TJournal));
        }


        protected virtual JournalPostResult Post(IDbContext db, TJournal journal)
        {
            JournalPostResult result = new JournalPostResult();

            foreach (var journalTxn in journal.JournalTxns)
            {
                foreach (var posting in journalTxn.JournalTemplateTxn.Postings)
                {
                    TLedgerTxn ledgerTxn = new TLedgerTxn();
                    ResolveAccount(posting, ledgerTxn);

                    if (ledgerTxn.LedgerAccount == null)
                        throw new Exception("Ledger account not specified");

                    ResolveFields(journal, journalTxn, posting, ledgerTxn);

                    journalTxn.LedgerTxns.Add(ledgerTxn);
                }
            }

            return result;
        }

        protected virtual void ResolveFields(TJournal journal, TJournalTxn journalTxn, TJournalTemplateTxnPosting posting, TLedgerTxn ledgerTxn)
        {
            ledgerTxn.TxnDate = journalTxn.TxnDate;
            //ledgerTxn.TransactionOrigin = journal.TransactionOrigin;
            ledgerTxn.AccountingEntity = journal.AccountingEntity;
            ledgerTxn.PartyID = journalTxn.PartyID;


            int multiplier = 0;
            if (ledgerTxn.LedgerAccount.LedgerAccountType.CreditPositive)
            {
                if (posting.PostType == "C")
                    multiplier = 1;
                else
                    multiplier = -1;
            }
            else
            {
                if (posting.PostType == "D")
                    multiplier = 1;
                else
                    multiplier = -1;
            }

            decimal amount = 0;

            if (posting.AddBaseAmount)
                amount += journalTxn.Amount;

            ledgerTxn.Amount = (amount * multiplier);
        }

        protected virtual void ResolveAccount(TJournalTemplateTxnPosting posting, TLedgerTxn ledgerTxn)
        {
            ledgerTxn.LedgerAccount = posting.LedgerAccount;
        }
    }
}
