using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.DistributedServices
{
    public abstract class WorkerPool : IWorkerPool
    {
        protected bool _shutdown = true;
        protected int _maxThreads;
        protected List<Thread> _threads = new List<Thread>();
        protected IQueueManager _queue;
        protected int _timeOut;        
        private Object _counterLock = new Object();

        private int _counter = 0;

        public WorkerPool(int threadCount, TimeSpan timeOut)
        {           
            _maxThreads = threadCount;
            _timeOut = Convert.ToInt32(timeOut.TotalMilliseconds);
            _queue = CreateQueue();
        }

        protected abstract string WorkerKey { get; }
        protected abstract string QueueKey { get; }

        protected IWorker CreateWorker(ReceivedMessage message, int threadNumber)
        {
            IWorker result = IoC.Container.ResolveKeyed<IWorker>(WorkerKey);
            result.Init(message, threadNumber);
            return result;
        }

        protected IQueueManager CreateQueue()
        {
            IQueueManager result = IoC.Container.ResolveKeyed<IQueueManager>(QueueKey);
            //result.Init();
            return result;
        }

        public virtual void Start()
        {
            _shutdown = false;
            for (int i = 0; i < _maxThreads; i++)
            {
                Thread thread = new Thread(Execute);
                _threads.Add(thread);
                thread.Start();
            }
        }

        public virtual void Stop()
        {
            _shutdown = true;

            foreach (Thread th in _threads)
                th.Join();
        }

        private void Execute()
        {
            int threadNo;
            lock (_counterLock)
            {
                threadNo = _counter;
                _counter++;
            }

            while (!_shutdown)
            {

                try
                {
                    ReceivedMessage item = _queue.TryDequeue(_timeOut);
                    if (item != null)
                    {
                        try
                        {
                            IWorker worker = CreateWorker(item, threadNo);
                            worker.StartLog();
                            if (worker.CanExecuteNow())
                            {
                                if (worker.AquireLock())
                                {
                                    try
                                    {
                                        worker.DoWork();
                                        try
                                        {
                                            worker.EndLog(true, false, false);
                                        }
                                        finally
                                        {
                                            _queue.Ack(item);
                                            worker.ReleaseLock();
                                            worker.Dispose();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _queue.Ack(item);
                                        worker.ReleaseLock();
                                        worker.EndLog(false, true, true);
                                        worker.Dispose();
                                        ex.Data.Add("PoolType", GetType().Name);
                                        ex.Data.Add("Key", item.Payload);
                                        //todo: send alert
                                    }

                                }
                                else
                                {
                                    //Thread.Sleep(2000);
                                    _queue.Ack(item);
                                    worker.EndLog(false, false, true);
                                    worker.Dispose();
                                }
                            }
                            else
                            {
                                _queue.Ack(item);
                                worker.EndLog(false, false, true);
                                worker.Dispose();
                                //_queue.Requeue(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(_timeOut); //throttle retries
                            _queue.Ack(item);
                            ex.Data.Add("PoolType", GetType().Name);
                            ex.Data.Add("Key", item.Payload);
                            //todo: send alert
                        }
                    }
                    else
                    {
                        Thread.Sleep(_timeOut); //no work
                    }

                }
                catch (Exception ex)
                {
                    //todo: send alert
                }
            }
        }



    }
}
