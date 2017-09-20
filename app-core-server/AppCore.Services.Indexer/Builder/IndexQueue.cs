using AppCore.Services.Indexer.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Builder
{
    public class IndexQueue : IIndexQueue
    {
        private ConcurrentQueue<IndexQueueItem> _queue = new ConcurrentQueue<IndexQueueItem>();

        public void QueueIndexWork(Type entityType, int entityId, bool recursive, Type contextType)
        {
            if (!_queue.Any(i => i.EntityType == entityType && i.EntityID == entityId))
            {
                _queue.Enqueue(new IndexQueueItem() { EntityType = entityType, EntityID = entityId, Recursive = recursive, ContextType = contextType });
            }
        }

        public ConcurrentQueue<IndexQueueItem> Queue
        {
            get
            {
                return _queue;
            }
        }


        public void PersistQueue()
        {
            //StringBuilder content = new StringBuilder();
            //IndexQueueItem item = null;
            //while (Queue.TryDequeue(out item))
            //    content.AppendLine(item.ToString());

            //string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            //path = System.IO.Path.GetDirectoryName(path);
            //string fileName = System.IO.Path.Combine(path, "index.queue");

            //System.IO.File.WriteAllText(fileName, content.ToString());
        }

        public void LoadPersistedQueue()
        {
            //string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            //path = System.IO.Path.GetDirectoryName(path);
            //string fileName = System.IO.Path.Combine(path, "index.queue");

            //if (System.IO.File.Exists(fileName))
            //{
            //    var task = new System.Threading.Tasks.Task(new Action(() =>
            //    {
            //        try
            //        {
            //            string[] content = System.IO.File.ReadAllLines(fileName);

            //            foreach (var line in content)
            //            {
            //                var item = IndexQueueItem.FromString(line);
            //                if (item != null)
            //                    IndexQueue.QueueIndexWork(item.EntityType, item.EntityID, item.Recursive);
            //            }
            //            System.IO.File.Delete(fileName);
            //        }
            //        catch
            //        {
            //            //
            //        }
            //    }));
            //    task.Start();
            //}

        }
    }
}
