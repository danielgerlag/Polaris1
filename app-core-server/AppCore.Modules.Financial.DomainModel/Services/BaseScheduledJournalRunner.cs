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
    public abstract class BaseScheduledJournalRunner<TScheduledJournal> : IScheduledJournalRunner
        where TScheduledJournal: BaseScheduledJournal        
    {

        IJournalPoster _journalPoster;
        //ISequenceNumberGeneator _sequenceNumberGeneator;
        //IContextParameterResolver _contextParameterResolver;
        
            
        public BaseScheduledJournalRunner(IJournalPoster journalPoster)
        {
            _journalPoster = journalPoster;
        }


        public JournalRunResult Run(IDbContext db, BaseScheduledJournal scheduledJournal)
        {
            return Run(db, (scheduledJournal as TScheduledJournal));
        }

        public bool CanExecuteNow(IDbContext db, BaseScheduledJournal scheduledJournal)
        {
            //int priority = 1; // scheduledJnl.JournalTemplate.Priority;
            //int blockingGenerators = 0;
            //IQueryable<TScheduledJournal> subSet = null;

            //blockingGenerators = subSet.Count(g => (!g.OnHold) && (!g.Archived) && g.ID != scheduledJnl.ID && g.TxnDate < scheduledJnl.TxnDate);

            //if (blockingGenerators == 0)
            //    blockingGenerators = subSet.Count(g => (!g.OnHold) && (!g.Archived) && g.ID != scheduledJnl.ID && g.TxnDate == scheduledJnl.TxnDate && g.JournalTemplate.Priority < priority);

            //return (blockingGenerators == 0);
            return true;
        }

        protected virtual JournalRunResult Run(IDbContext db, TScheduledJournal scheduledJournal)
        {
            JournalRunResult result = new JournalRunResult();

            var split = ResolveTopSplit(db, scheduledJournal);

            var builder = SelectJournalBuilder(db, scheduledJournal);

            foreach (var item in split)
            {
                var journal = builder.BuildJournal(db, scheduledJournal, item.Role, item.Percentage);
                _journalPoster.Post(db, journal);
                result.Journals.Add(journal);
            }
            

            IncrementTrigger(db, scheduledJournal);

            return result;
        }

        protected abstract IJournalBuilder SelectJournalBuilder(IDbContext db, TScheduledJournal scheduledJournal);
        protected abstract IPartyRole SelectContractParty(IDbContext db, TScheduledJournal scheduledJournal);


        protected virtual IEnumerable<ResolvedTopSplit> ResolveTopSplit(IDbContext db, TScheduledJournal scheduledJournal)
        {
            List<ResolvedTopSplit> result = new List<ResolvedTopSplit>();            
            result.Add(new ResolvedTopSplit() { Role = SelectContractParty(db, scheduledJournal), Percentage = 1 }); //default
            return result;
        }


        protected virtual void IncrementTrigger(IDbContext db, TScheduledJournal scheduledJournal)
        {
            
            switch (scheduledJournal.Frequency)
            {
                case "O":
                    scheduledJournal.Archived = true;
                    break;
                case "D":
                    scheduledJournal.NextExecutionDate = scheduledJournal.NextExecutionDate.Value.AddDays(1);
                    scheduledJournal.TxnDate = scheduledJournal.TxnDate.Value.AddDays(1);
                    break;
                case "BW":
                    scheduledJournal.NextExecutionDate = scheduledJournal.NextExecutionDate.Value.AddDays(14);
                    scheduledJournal.TxnDate = scheduledJournal.TxnDate.Value.AddDays(14);
                    break;
                case "M":
                    scheduledJournal.NextExecutionDate = scheduledJournal.NextExecutionDate.Value.AddMonths(1);
                    scheduledJournal.TxnDate = scheduledJournal.TxnDate.Value.AddMonths(1);
                    break;
                case "A":
                    scheduledJournal.NextExecutionDate = scheduledJournal.NextExecutionDate.Value.AddYears(1);
                    scheduledJournal.TxnDate = scheduledJournal.TxnDate.Value.AddYears(1);
                    break;
            }

            if (scheduledJournal.EffectiveTo.HasValue)
            {
                if (scheduledJournal.EffectiveTo.Value < DateTime.Now)
                {
                    scheduledJournal.Archived = true;
                }
            }

        }
    }

    public class ResolvedTopSplit
    {
        public IPartyRole Role { get; set; }
        public decimal Percentage { get; set; }
    }
}
