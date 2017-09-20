using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DistributedServices
{
    public class MemoryQueue : IQueueManager
    {

        protected ConcurrentQueue<int> _queue = new ConcurrentQueue<int>();

        public MemoryQueue()
        {
        }

        public void Ack(ReceivedMessage item)
        {
            
        }

        public void Enqueue(int obj)
        {
            _queue.Enqueue(obj);
        }

        public void NAck(ReceivedMessage item)
        {
            _queue.Enqueue(item.Payload);
        }

        public void Requeue(ReceivedMessage item)
        {
            _queue.Enqueue(item.Payload);
        }

        public ReceivedMessage TryDequeue(int timeout)
        {
            int item;
            if (_queue.TryDequeue(out item))
            {
                return new ReceivedMessage(item);
            }
            return null;
        }
    }
}
