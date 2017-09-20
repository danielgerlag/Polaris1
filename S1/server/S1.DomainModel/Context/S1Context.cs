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

namespace S1.DomainModel.Context
{
    public class S1Context : BaseContext, IS1Context
    {
        public IDbSet<AccountingEntity> AccountingEntities { get; set; }
                
        public IDbSet<Contract> Contracts { get; set; }

        public IDbSet<JournalTemplate> JournalTemplates { get; set; }

        public IDbSet<ScheduledJournal> ScheduledJournals { get; set; }

        public IDbSet<JournalType> JournalTypes { get; set; }


        public IDbSet<Journal> Journals { get; set; }

        public IDbSet<JournalTxn> JournalTxns { get; set; }

        public IDbSet<LedgerAccountType> LedgerAccountTypes { get; set; }


        public IDbSet<Party> Parties { get; set; }

        public IDbSet<Region> Regions { get; set; }

        public IDbSet<UserInputType> UserInputTypes { get; set; }

        public IDbSet<JournalTemplateInput> JournalTemplateInputs { get; set; }

        public IDbSet<ScheduledJournalInputValue> ScheduledJournalInputValues { get; set; }

        public IDbSet<DocumentContent> DocumentContents { get; set; }

        public IDbSet<WorkItem> WorkItems { get; set; }

        public IDbSet<WorkItemStatus> WorkItemStatuses { get; set; }

        public IDbSet<WorkItemAttachment> WorkItemAttachments { get; set; }

        protected override Type GetInterfaceType()
        {
            return typeof(IS1Context);
        }

        public S1Context(string connectionString, IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base(connectionString, indexQueue, indexRegistry)
        {
            Database.SetInitializer<S1Context>(new S1Init());
        }

        public S1Context(IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base(indexQueue, indexRegistry)
        {
            Database.SetInitializer<S1Context>(new S1Init());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");            

            base.OnModelCreating(modelBuilder);
        }
    }
}
