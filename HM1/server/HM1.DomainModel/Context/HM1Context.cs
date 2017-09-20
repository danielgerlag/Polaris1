using AppCore.DomainModel.Abstractions;
using AppCore.Modules.Financial.DomainModel;
using AppCore.Modules.Foundation.DomainModel;
using AppCore.Services.Indexer.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.DomainModel.Context
{
    public class HM1Context : BaseContext, IHM1Context
    {
        public IDbSet<AccountingEntity> AccountingEntities { get; set; }

        public IDbSet<Contract> Contracts { get; set; }

        public IDbSet<JournalTemplate> JournalTemplates { get; set; }

        public IDbSet<JournalTemplateTxnPosting> JournalTemplateTxnPostings { get; set; }

        public IDbSet<JournalTemplateTxn> JournalTemplateTxns { get; set; }

        public IDbSet<ScheduledJournal> ScheduledJournals { get; set; }

        public IDbSet<JournalType> JournalTypes { get; set; }


        public IDbSet<Journal> Journals { get; set; }

        public IDbSet<JournalTxn> JournalTxns { get; set; }

        public IDbSet<Ledger> Ledgers { get; set; }

        public IDbSet<LedgerAccount> LedgerAccounts { get; set; }

        public IDbSet<LedgerAccountType> LedgerAccountTypes { get; set; }

        public IDbSet<LedgerTxn> LedgerTxns { get; set; }

        public IDbSet<Party> Parties { get; set; }

        public IDbSet<Region> Regions { get; set; }

        public IDbSet<UserInputType> UserInputTypes { get; set; }

        public IDbSet<JournalTemplateInput> JournalTemplateInputs { get; set; }

        public IDbSet<ScheduledJournalInputValue> ScheduledJournalInputValues { get; set; }

        public IDbSet<Participant> Participants { get; set; }

        public IDbSet<ProviderAccountParticipant> ProviderAccountParticipants { get; set; }

        public IDbSet<ProviderAccount> ProviderAccounts { get; set; }

        protected override Type GetInterfaceType()
        {
            return typeof(IHM1Context);
        }

        public HM1Context(string connectionString, IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base(connectionString, indexQueue, indexRegistry)
        {
            Database.SetInitializer<HM1Context>(new HM1Init());
        }

        public HM1Context(IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base(indexQueue, indexRegistry)
        {
            Database.SetInitializer<HM1Context>(new HM1Init());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");

            base.OnModelCreating(modelBuilder);
        }
    }
}
