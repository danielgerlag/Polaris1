using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using AppCore.Modules.Foundation.DomainModel;
using AppCore.Modules.HR.DomainModel;
using AppCore.DomainModel.Abstractions;
using AppCore.Services.Indexer.Interface;

namespace HR1.DomainModel.Context
{
    public class HR1Context : BaseContext, IHR1Context
    {
        public IDbSet<AccountingEntity> AccountingEntities { get; set; }        
                
        public IDbSet<Employee> Employees { get; set; }

        public IDbSet<Employment> Employments { get; set; }

        public IDbSet<Contract> Contracts { get; set; }

        public IDbSet<JournalTemplate> JournalTemplates { get; set; }

        public IDbSet<ScheduledJournal> ScheduledJournals { get; set; }

        public IDbSet<JournalType> JournalTypes { get; set; }


        public IDbSet<Journal> Journals { get; set; }

        public IDbSet<JournalTxn> JournalTxns { get; set; }

        public IDbSet<LedgerAccountType> LedgerAccountTypes { get; set; }


        public IDbSet<Party> Parties { get; set; }                

        public IDbSet<UserInputType> UserInputTypes { get; set; }

        public IDbSet<JournalTemplateInput> JournalTemplateInputs { get; set; }

        public IDbSet<ScheduledJournalInputValue> ScheduledJournalInputValues { get; set; }

        protected override Type GetInterfaceType()
        {
            return typeof(IHR1Context);
        }

        public HR1Context(string connectionString, IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base(connectionString, indexQueue, indexRegistry)
        {
            Database.SetInitializer<HR1Context>(new HR1Init());
        }

        public HR1Context(IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base(indexQueue, indexRegistry)
        {
            Database.SetInitializer<HR1Context>(new HR1Init());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");


            //modelBuilder.Entity<Contract>()
            //   .HasOptional(s => s.Employment)
            //   .WithRequired(ad => ad.Contract);

            //modelBuilder.Entity<Employment>()                
            //    .HasRequired<Contract>(x => x.Contract)
            //    .WithOptional(x => x.Employment);


            base.OnModelCreating(modelBuilder);
        }
    }
}
