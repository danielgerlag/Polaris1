using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.Services.Interfaces
{
    public interface ITransactionPollManager
    {
        void Start(TimeSpan interval);
        void Stop();
    }
}
