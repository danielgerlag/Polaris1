using AppCore.Modules.Financial.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel;

namespace HM1.DomainModel.Services
{
    public class JournalBuilder : BaseJournalBuilder<Journal, AccountingEntity, JournalTxn, ScheduledJournal, Contract, JournalTemplate, ScheduledJournalInputValue, JournalTemplateTxn, JournalTemplateInput, JournalTemplateTxnPosting, LedgerTxn, Party>
    {

        protected override void CreateJournalTxn(IDbContext db, ScheduledJournal scheduledJournal, JournalTemplateTxn templateTxn, decimal percentage, ResolvedSubSplit item, JournalTxn txn)
        {
            base.CreateJournalTxn(db, scheduledJournal, templateTxn, percentage, item, txn);

            if (item.Role is Participant)
                txn.Participant = (item.Role as Participant);
        }

        protected override decimal? ResolveIsFactor(IDbContext db, ScheduledJournal scheduledJournal, IPartyRole resolvedRole, JournalTemplateTxn templateTxn)
        {
            switch (templateTxn.JournalTxnType.PrimaryFactorCode)
            {
                case "PART":
                    return 1;
                default:
                    return base.ResolveIsFactor(db, scheduledJournal, resolvedRole, templateTxn);
            }
        }

        protected override IEnumerable<ResolvedSubSplit> ResolveSubSplit(IDbContext db, ScheduledJournal scheduledJournal, IPartyRole resolvedRole, JournalTemplateTxn templateTxn)
        {
            switch (templateTxn.JournalTxnType.PrimaryFactorCode)
            {
                case "PART":
                    List<ResolvedSubSplit> result = new List<ResolvedSubSplit>();

                    var providerAcct = db.Set<ProviderAccount>().Find(scheduledJournal.Contract.ID);
                    foreach (var participant in providerAcct.Participants)
                    {
                        result.Add(new ResolvedSubSplit() { Role = participant.Participant, Percentage = participant.Percentage });
                    }                                        
                    
                    return result;
                default:
                    return base.ResolveSubSplit(db, scheduledJournal, resolvedRole, templateTxn);
            }            
        }

    }
}
