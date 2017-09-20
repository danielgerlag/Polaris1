using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Context;
using AppCore.Modules.Foundation.DomainModel.Context;
using AppCore.Modules.HR.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR1.DomainModel.Context
{
    public interface IHR1Context : IDbContext, 
        IFoundationContext<Party>, 
        IHRContext<Employee, Employment>, 
        IFinancialContext<AccountingEntity, Contract, JournalTemplate, ScheduledJournal, Journal, JournalTxn, JournalTemplateInput, ScheduledJournalInputValue>
    {
    }
}
