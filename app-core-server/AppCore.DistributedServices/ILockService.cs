using System;

namespace AppCore.DistributedServices
{
    public interface ILockService: IDisposable
    {
        bool AquireLock(string uid);
        void ReleaseAllLocks();
        void ReleaseLock(string uid);
    }
}