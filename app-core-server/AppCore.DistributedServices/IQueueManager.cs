namespace AppCore.DistributedServices
{
    public interface IQueueManager
    {
        void Enqueue(int obj);
        ReceivedMessage TryDequeue(int timeout);

        void Ack(ReceivedMessage item);
        void NAck(ReceivedMessage item);
        void Requeue(ReceivedMessage item);
    }
}