using AppCore.DomainModel.Interface;
using AppCore.Services.Indexer.Interface;
using AppCore.Services.Indexer.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

namespace AppCore.Services.Indexer.Builder
{
    public class IndexWorker : IIndexWorker
    {

        bool shutdown = false;
        List<Thread> threads = new List<Thread>();

        private IIndexQueue _indexQueue;
        //private IServiceProvider _services;
        private IIndexRegister _registry;
        //private ILogger<IndexWorker> _logger;

        public IndexWorker(IIndexQueue queue, IIndexRegister registry)
        {
            _indexQueue = queue;
            //_services = services;
            //logger = loggerFactory.CreateLogger<IndexWorker>();            
            //_logger = logger;
            _registry = registry;
        }

        public void Start()
        {
            _indexQueue.LoadPersistedQueue();
            for (int i = 0; i < 1; i++)
            {
                //_logger.LogDebug("Starting index thread " + i);
                Thread thread = new Thread(new ThreadStart(DoWork));
                threads.Add(thread);
                thread.Start();
            }
        }

        private void DoWork()
        {
            while (!shutdown)
            {
                IndexQueueItem item = null;
                try
                {
                    if (_indexQueue.Queue.TryDequeue(out item)) 
                    {
                        try
                        {
                            //_logger.LogDebug("Dequeued item - " + item.EntityID);

                            //IIndexStore indexStore = _services.GetService<IIndexStore>();  
                            IIndexStore indexStore = IoC.Container.Resolve<IIndexStore>();
                            IDbContext primaryDC = (IoC.Container.Resolve(item.ContextType) as IDbContext);
                            IIndexBuilder indexer = new IndexBuilder(indexStore, primaryDC, _indexQueue, _registry); //TODO: fix
                            indexer.IndexEntity(item.EntityType, item.EntityID, item.Recursive, item.ContextType);
                            indexStore.SaveChanges();
                            indexStore.Dispose();
                            primaryDC.Dispose();
                        }
                        catch (OptimisticConcurrencyException ex)
                        {
                            _indexQueue.Queue.Enqueue(item);
                            //_logger.LogError("Error indexing (" + item.EntityType + ", " + item.EntityID.ToString() + "): " + ex.Message, ex);
                        }
                        catch (Exception ex)
                        {
                            //todo: add error reporting
                            //_logger.LogError("Error indexing (" + item.EntityType + ", " + item.EntityID.ToString() + "): " + ex.Message, ex);
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(500); //no work
                    }
                }
                catch (Exception ex)
                {
                    //todo: add error reporting
                    //_logger.LogError("Error indexing (" + item.EntityType + ", " + item.EntityID.ToString() + "): " + ex.Message, ex);
                }
            }
        }

        public void Stop()
        {
            shutdown = true;

            foreach (Thread th in threads)
                th.Join();

            _indexQueue.PersistQueue();
        }
    }
}
