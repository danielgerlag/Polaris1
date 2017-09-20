using AppCore.DistributedServices;
using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Services.Interfaces;
using AppCore.Modules.Financial.DomainModel.Services.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.DistributedServices
{
    public abstract class BaseScheduledJournalWorker<TScheduledJournal, TAppDataContext> : DistributedWorker
        where TScheduledJournal : BaseScheduledJournal
        where TAppDataContext : IDbContext
    {
        protected IScheduledJournalRunner _runner;

        protected TAppDataContext _db;
        protected TAppDataContext _controlDb;

        protected TScheduledJournal scheduledJnl;
        protected ScheduledJournalLog log;

        public BaseScheduledJournalWorker(IScheduledJournalRunner runner)
            : base()
        {
            _runner = runner;
        }

        public override void Init(ReceivedMessage message, int threadNumber)
        {
            base.Init(message, threadNumber);
            _db = IoC.Container.Resolve<TAppDataContext>();
            _controlDb = IoC.Container.Resolve<TAppDataContext>();
            scheduledJnl = _db.Set<TScheduledJournal>().Find(message.Payload);

            if (scheduledJnl == null)
                return;

            AquireLocks();

        }

        protected virtual void AquireLocks()
        {
            _lockKeys.Add("SchedJnl:" + scheduledJnl.ID.ToString());
        }

        public override void DoWork()
        {
            if (scheduledJnl == null)
                return;

            _db.Entry(scheduledJnl).Reload();

            if (!CanExecuteNow())
                return;

            var txOptions = new System.Transactions.TransactionOptions();
            txOptions.IsolationLevel = System.Transactions.IsolationLevel.Snapshot;

            System.Transactions.TransactionScope txnScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew, txOptions);
            try
            {
                JournalRunResult runResult;
                using (txnScope)
                {
                    runResult = _runner.Run(_db, scheduledJnl);
                    if ((runResult.Errors.Count() == 0) && (runResult.Holders.Count() == 0) && (runResult.Deferrals.Count() == 0))
                    {
                        _db.SaveChanges();
                        txnScope.Complete();
                    }
                    else
                    {
                        txnScope.Dispose();
                    }
                }

                foreach (var err in runResult.Errors)
                {
                    log.Exceptions.Add(new ScheduledJournalException()
                    {
                        Message = err,
                        ExceptionType = "E",
                        AppTenantID = scheduledJnl.AppTenantID
                    });
                }

                foreach (var err in runResult.Holders)
                {
                    log.Exceptions.Add(new ScheduledJournalException()
                    {
                        Message = err,
                        ExceptionType = "H",
                        AppTenantID = scheduledJnl.AppTenantID
                    });
                }

                foreach (var err in runResult.Deferrals)
                {
                    log.Exceptions.Add(new ScheduledJournalException()
                    {
                        Message = err,
                        ExceptionType = "D",
                        AppTenantID = scheduledJnl.AppTenantID
                    });
                }

                if (runResult.Errors.Count() > 0)
                    MarkError();

                if (runResult.Holders.Count() > 0)
                    MarkHold();

                if ((runResult.Errors.Count() == 0) && (runResult.Holders.Count() == 0) && (runResult.Deferrals.Count() == 0))
                {
                    if (runResult.Journals.Count > 0)
                    {
                        //
                    }
                }
            }
            catch (Exception ex)
            {
                txnScope.Dispose();

                if ((!(ex is OptimisticConcurrencyException)) && (!ex.Message.Contains("deadlock victim")))
                    MarkError();

                if (ex is AggregateException)
                {
                    foreach (var ex2 in (ex as AggregateException).InnerExceptions)
                    {
                        log.Exceptions.Add(new ScheduledJournalException()
                        {
                            Message = ex2.Message,
                            ExceptionType = "E",
                            AppTenantID = scheduledJnl.AppTenantID

                        });
                    }
                }
                else
                {
                    log.Exceptions.Add(new ScheduledJournalException()
                    {
                        Message = ex.Message,
                        ExceptionType = "E",
                        AppTenantID = scheduledJnl.AppTenantID
                    });
                }
                throw ex;
            }

        }


        public override bool CanExecuteNow()
        {
            if (scheduledJnl == null)
                return true;

            if (!scheduledJnl.NextExecutionDate.HasValue)
                return false;

            if (scheduledJnl.NextExecutionDate.Value > DateTime.Now)
                return false;

            if (scheduledJnl.Archived)
                return false;
            
            return _runner.CanExecuteNow(_db, scheduledJnl);
        }

        public override void StartLog()
        {
            log = new ScheduledJournalLog();
            log.StartTime = DateTime.Now;
            log.Thread = ThreadNumber;
            log.AppTenantID = scheduledJnl.AppTenantID;
            log.ScheduledJournalID = scheduledJnl.ID;
            log.MachineName = Environment.MachineName;

            _controlDb.Set<ScheduledJournalLog>().Add(log);

            _controlDb.SaveChanges();
        }

        public override void EndLog(bool success, bool failure, bool requeue)
        {
            log.EndTime = DateTime.Now;
            log.IsSuccess = success;
            log.IsError = failure;
            log.IsRequeue = requeue;
            _controlDb.SaveChanges();
        }

        private void MarkError()
        {
            var sj = _controlDb.Set<TScheduledJournal>().Find(scheduledJnl.ID);
            sj.Error = true;
        }

        private void MarkHold()
        {
            //trigger.OnHold = true;
            //_db.SaveChanges();
        }
    }
}
