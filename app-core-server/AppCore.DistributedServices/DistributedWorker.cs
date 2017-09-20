using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DistributedServices
{
    public abstract class DistributedWorker : IDisposable, IWorker
    {
        protected List<string> _lockKeys = new List<string>();
        protected ILockService _lockService;        

        public int ThreadNumber { get; set; }
        public ReceivedMessage Message { get; set; }

        public DistributedWorker()
        {
            
        }

        public virtual void Init(ReceivedMessage message, int threadNumber)
        {
            Message = message;
            ThreadNumber = threadNumber;
        }

        public abstract void StartLog();
        public abstract void DoWork();
        public abstract void EndLog(bool success, bool failure, bool requeue);


        public bool AquireLock()
        {
            bool result = true;
            _lockService = IoC.Container.Resolve<ILockService>(); //new LockService(_connectionString);
            foreach (string lockID in _lockKeys)
            {
                bool firstPass = _lockService.AquireLock(lockID);

                if (!firstPass)
                {
                    System.Threading.Thread.Sleep(500 * (ThreadNumber + 1));
                    result = result && _lockService.AquireLock(lockID);
                    if (!result)
                    {
                        _lockService.ReleaseAllLocks();
                        break;
                    }
                }
                else
                    result = result && firstPass;
            }
            return result;
        }

        public void ReleaseLock()
        {
            if (_lockService != null)
            {
                foreach (string lockID in _lockKeys)
                {
                    _lockService.ReleaseLock(lockID);
                }
                _lockService.ReleaseAllLocks();
            }
        }

        public abstract bool CanExecuteNow();

        public virtual void Dispose()
        {
            if (_lockService != null)
            {
                try
                {
                    _lockService.Dispose();
                }
                catch { }
            }
        }
    }
}
