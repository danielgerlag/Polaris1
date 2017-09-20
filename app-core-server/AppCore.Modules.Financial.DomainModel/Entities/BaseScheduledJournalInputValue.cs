using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{
    public abstract class BaseScheduledJournalInputValue : UserInputValue
    {        
    }

    public abstract class BaseScheduledJournalInputValue<TScheduledJournal, TJournalTemplateInput> : BaseScheduledJournalInputValue
        where TScheduledJournal : BaseScheduledJournal
        where TJournalTemplateInput : BaseJournalTemplateInput
    {

        public int ScheduledJournalID { get; set; }
        public virtual TScheduledJournal ScheduledJournal { get; set; }

        public int JournalTemplateInputID { get; set; }
        public virtual TJournalTemplateInput JournalTemplateInput { get; set; }

    }
}
