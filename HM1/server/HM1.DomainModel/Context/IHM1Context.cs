using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Context;
using AppCore.Modules.Foundation.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.DomainModel.Context
{
    public interface IHM1Context : IDbContext,
        IFoundationContext<Party>,
        IFinancialContext<AccountingEntity, Contract, JournalTemplate, ScheduledJournal, Journal, JournalTxn, JournalTemplateInput, ScheduledJournalInputValue, LedgerTxn, JournalTemplateTxn, JournalTemplateTxnPosting, LedgerAccount>
    {


        IDbSet<Participant> Participants { get; set; }

        IDbSet<ProviderAccountParticipant> ProviderAccountParticipants { get; set; }

        IDbSet<ProviderAccount> ProviderAccounts { get; set; }
    }
}
