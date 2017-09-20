using AppCore.DistributedServices;
using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Services.Interfaces;
using HM1.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HM1.DomainModel.DistributedServices
{    
    public class JournalPollManager : ITransactionPollManager
    {

        private IHM1Context _db;
        private IQueueManager _queue;
        private Timer _timer;

        public JournalPollManager(IHM1Context db)
        {
            _db = db;
            _queue = AppCore.IoC.Container.ResolveKeyed<IQueueManager>("ScheduledJournalQueue");
        }

        public void Start(TimeSpan interval)
        {
            _timer = new Timer(new TimerCallback(TriggerPoll), null, TimeSpan.FromSeconds(10), interval);
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        private void TriggerPoll(object state)
        {
            try
            {
                var query = _db.ScheduledJournals.Where(x => x.Archived == false && x.NextExecutionDate <= DateTime.Now).Select(x => x.ID);
                var list = query.ToList();
                foreach (var item in list)
                    _queue.Enqueue(item);
            }
            catch (Exception ex)
            {
                //log
            }
        }

    }
}
