using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Context;
using AppCore.Modules.Foundation.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1.DomainModel.Context
{
    public interface IS1Context : IDbContext,
        IFoundationContext<Party>,        
        IFinancialContext<AccountingEntity, Contract, JournalTemplate, ScheduledJournal, Journal, JournalTxn, JournalTemplateInput, ScheduledJournalInputValue>
    {

        IDbSet<DocumentContent> DocumentContents { get; set; }

        IDbSet<WorkItem> WorkItems { get; set; }

        IDbSet<WorkItemStatus> WorkItemStatuses { get; set; }

        IDbSet<WorkItemAttachment> WorkItemAttachments { get; set; }

    }
}
