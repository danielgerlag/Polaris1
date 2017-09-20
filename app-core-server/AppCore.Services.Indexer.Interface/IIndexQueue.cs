using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Interface
{
    public interface IIndexQueue
    {
        void QueueIndexWork(Type entityType, int entityId, bool recursive, Type contextType);
        void PersistQueue();
        void LoadPersistedQueue();

        ConcurrentQueue<IndexQueueItem> Queue { get; }

    }
}
