using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Services.Interfaces;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.Services
{
    public abstract class BaseJournalBuilder<TJournal, TAccountingEntity, TJournalTxn, TScheduledJournal, TContract, TJournalTemplate, TScheduledJournalInputValue, TJournalTemplateTxn, TJournalTemplateInput, TJournalTemplateTxnPosting, TLedgerTxn, TParty> : IJournalBuilder
        where TJournal : BaseJournal<TAccountingEntity, TJournalTxn>, new()
        where TAccountingEntity : BaseAccountingEntity
        where TJournalTxn : BaseJournalTxn<TJournal, TLedgerTxn, TJournalTemplateTxn, TParty>, new()
        where TScheduledJournal : BaseScheduledJournal<TAccountingEntity, TContract, TJournalTemplate, TScheduledJournalInputValue>
        where TContract : BaseContract
        where TJournalTemplate : BaseJournalTemplate<TAccountingEntity, TJournalTemplateTxn, TJournalTemplateInput>
        where TScheduledJournalInputValue : BaseScheduledJournalInputValue<TScheduledJournal, TJournalTemplateInput>
        where TJournalTemplateTxn : BaseJournalTemplateTxn<TJournalTemplate, TJournalTemplateInput, TJournalTemplateTxnPosting>
        where TJournalTemplateInput : BaseJournalTemplateInput
        where TJournalTemplateTxnPosting : BaseJournalTemplateTxnPosting
        where TLedgerTxn : BaseLedgerTxn
        where TParty : BaseParty
    {

        
        public BaseJournal BuildJournal(IDbContext db, BaseScheduledJournal scheduledJournal, IPartyRole resolvedRole, decimal percentage)
        {
            return BuildJournal(db, (scheduledJournal as TScheduledJournal), resolvedRole, percentage);
        }

        protected virtual TJournal BuildJournal(IDbContext db, TScheduledJournal scheduledJournal, IPartyRole resolvedRole, decimal percentage)
        {
            TJournal journal = new TJournal();
            var template = scheduledJournal.JournalTemplate;

            journal.AppTenantID = scheduledJournal.AppTenantID;
            journal.Description = scheduledJournal.Description;
            journal.JournalType = template.JournalType;
            journal.AccountingEntity = scheduledJournal.AccountingEntity;
            //journal.TransactionOrigin = scheduledJournal.TransactionOrigin;
            //journal.Public = resolvedParty;
            
            journal.TxnDate = scheduledJournal.TxnDate.Value.Date;
            journal.Reference = ResolveReference(db, scheduledJournal);

            db.Set<TJournal>().Add(journal);

            foreach (var templateTxn in template.JournalTemplateTxns)
            {
                //if (templateTxn.JournalTxnType.IsCode)
                
                BuildTxn(db, scheduledJournal, resolvedRole, journal, templateTxn, percentage);
                
            }

            journal.Amount = journal.JournalTxns.Where(x => x.IncludeInTotal).Sum(x => x.Amount);

            return journal;
        }

        protected virtual void BuildTxn(IDbContext db, TScheduledJournal scheduledJournal, IPartyRole resolvedRole, TJournal journal, TJournalTemplateTxn templateTxn, decimal percentage)
        {
            var split = ResolveSubSplit(db, scheduledJournal, resolvedRole, templateTxn);
            foreach (var item in split)
            {
                TJournalTxn txn = new TJournalTxn();
                CreateJournalTxn(db, scheduledJournal, templateTxn, percentage, item, txn);
                journal.JournalTxns.Add(txn);
            }
        }

        protected virtual void CreateJournalTxn(IDbContext db, TScheduledJournal scheduledJournal, TJournalTemplateTxn templateTxn, decimal percentage, ResolvedSubSplit item, TJournalTxn txn)
        {
            var isFactor = ResolveIsFactor(db, scheduledJournal, item.Role, templateTxn);
            var ofFactor = ResolveOfFactor(db, scheduledJournal, item.Role, templateTxn);

            if ((!isFactor.HasValue) || (!ofFactor.HasValue))
                throw new Exception("Unable to resolve amount");

            txn.Amount = (((isFactor.Value * ofFactor.Value) * item.Percentage) * percentage);
            //txn.Amount = (ResolveAmount(db, scheduledJournal, resolvedParty, templateTxn) * percentage);

            txn.Description = templateTxn.Description;

            txn.PartyID = item.Role.GetPartyID();
            //txn.TransactionOrigin = transactionTrigger.TransactionOrigin;
            txn.TxnDate = scheduledJournal.TxnDate.Value.Date;
            //txn. = templateTxn;            
            txn.AppTenantID = scheduledJournal.AppTenantID;
            txn.JournalTemplateTxn = templateTxn;
            txn.IncludeInTotal = templateTxn.IncludeInTotal;
        }


        protected virtual IEnumerable<ResolvedSubSplit> ResolveSubSplit(IDbContext db, TScheduledJournal scheduledJournal, IPartyRole resolvedRole, TJournalTemplateTxn templateTxn)
        {
            List<ResolvedSubSplit> result = new List<ResolvedSubSplit>();
            result.Add(new ResolvedSubSplit() { Role = resolvedRole, Percentage = 1 }); //default
            return result;
        }


        protected virtual decimal? ResolveIsFactor(IDbContext db, TScheduledJournal scheduledJournal, IPartyRole resolvedRole, TJournalTemplateTxn templateTxn)
        {
            decimal? result = null;

            switch (templateTxn.JournalTxnType.PrimaryFactorCode)
            {
                case "PERC":
                case "AMT":

                    switch (templateTxn.PrimaryFactorSource)
                    {
                        case "TEMPLATE":                            
                            result = templateTxn.Amount;                            
                            break;
                        case "INPUT":
                            if (templateTxn.AmountInputID.HasValue)
                            {
                                var amountInput = scheduledJournal.UserInputValues.First(x => x.JournalTemplateInputID == templateTxn.AmountInputID);
                                result = Convert.ToDecimal(amountInput.Value);
                            }
                            break;
                        case "CONTEXT":
                            //if (templateTxn.AmountContextParameterID.HasValue)
                            //{
                            //    result = ResolveContextParameter(db, transactionTrigger, templateTxn, resolvedPublic);
                            //}
                            break;
                    }                    
                    break;
            }

            //if (templateTxn.InvertPercentage)
            //{
            //    result = (1 - result.Value);
            //}

            return result;
        }

        protected virtual decimal? ResolveOfFactor(IDbContext db, TScheduledJournal scheduledJournal, IPartyRole resolvedRole, TJournalTemplateTxn templateTxn)
        {
            decimal? result = null;

            switch (templateTxn.JournalTxnType.SecondaryFactorCode)
            {
                case null:
                    result = 1;
                    break;
                case "PERC":
                case "AMT":
                    switch (templateTxn.SecondaryFactorSource)
                    {
                        case "TEMPLATE":
                            result = templateTxn.Amount;
                            break;
                        case "INPUT":
                            if (templateTxn.AmountInputID.HasValue)
                            {
                                var amountInput = scheduledJournal.UserInputValues.First(x => x.JournalTemplateInputID == templateTxn.AmountInputID);
                                result = Convert.ToDecimal(amountInput.Value);
                            }
                            break;
                        case "CONTEXT":
                            //if (templateTxn.AmountContextParameterID.HasValue)
                            //{
                            //    result = ResolveContextParameter(db, transactionTrigger, templateTxn, resolvedPublic);
                            //}
                            break;
                    }
                    break;                    
                case "LEDGER":
                    throw new NotImplementedException();                    
            }

            //if (templateTxn.InvertPercentage)
            //{
            //    result = (1 - result.Value);
            //}

            return result;
        }
                

        protected virtual string ResolveReference(IDbContext db, TScheduledJournal scheduledJournal)
        {
            var template = scheduledJournal.JournalTemplate;
            var refTemplateInput = template.ReferenceInput;

            if (refTemplateInput != null)
            {
                var refInput = scheduledJournal.UserInputValues.First(x => x.JournalTemplateInputID == refTemplateInput.ID);
                return refInput.Value;
            }

            if (template.SequenceNumberID.HasValue)
            {
                //return _sequenceNumberGeneator.GetNextNumber(db, transactionTrigger.JournalTemplate.SequenceNumberID.Value);
            }

            return null;
        }

        //private decimal ResolveLedgerBalance(IDbContext db, TScheduledJournal scheduledJournal, BaseJournalTemplateTxn templateTxn, BaseParty resolvedParty)
        //{
        //    if (templateTxn.AmountLedgerAccount == null)
        //        throw new Exception("Ledger account not specified");

        //    PolicyTransactionTrigger policyTransactionTrigger = null;
        //    if (transactionTrigger.PolicyTransactionTrigger != null)
        //        policyTransactionTrigger = (transactionTrigger.PolicyTransactionTrigger);

        //    var query = db.LedgerTxns.Where(x => x.ReportingEntityID == transactionTrigger.ReportingEntityID && x.LedgerAccountID == templateTxn.AmountLedgerAccountID && x.TxnDate <= transactionTrigger.TxnDate);

        //    switch (templateTxn.BalanceOrigin)
        //    {
        //        case "P":
        //            query = query.Where(x => x.PolicyID == policyTransactionTrigger.PolicyID);
        //            break;
        //        case "U":
        //            if (resolvedPublic == null)
        //                throw new Exception("No public to resolve ledger balance for");

        //            query = query.Where(x => x.PublicID == resolvedPublic.ID);
        //            break;
        //        case "X":
        //            if (resolvedPublic == null)
        //                throw new Exception("No public to resolve ledger balance for");

        //            query = query.Where(x => x.PublicID == resolvedPublic.ID && x.PolicyID == policyTransactionTrigger.PolicyID);
        //            break;
        //            //todo: agents, sps, etc...
        //    }

        //    return query.Sum(x => x.Amount);
        //}

        //private decimal ResolveContextParameter(IDbContext db, TScheduledJournal scheduledJournal, JournalTemplateTxn templateTxn, Public resolvedPublic)
        //{
        //    if (scheduledJournal.PolicyTransactionTrigger != null)
        //    {
        //        var policyTransactionTrigger = (scheduledJournal.PolicyTransactionTrigger);
        //        if (policyTransactionTrigger.Policy != null)
        //        {
        //            return _contextParameterResolver.Resolve(db, trigger.TxnDate.Value, templateTxn.AmountContextParameterID.Value, policyTransactionTrigger.Policy);
        //        }
        //    }

        //    throw new NotImplementedException();
        //}

    }

    public class ResolvedSubSplit
    {
        public IPartyRole Role { get; set; }
        public decimal Percentage { get; set; }
    }
}
