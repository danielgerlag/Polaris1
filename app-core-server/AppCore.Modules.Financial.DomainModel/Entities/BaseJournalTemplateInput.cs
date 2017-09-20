using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{
    public abstract class BaseJournalTemplateInput : UserInput
    {        
    }

    public abstract class BaseJournalTemplateInput<TJournalTemplate> : BaseJournalTemplateInput
        where TJournalTemplate : BaseJournalTemplate
    {

        public int JournalTemplateID { get; set; }

        [InverseProperty("UserInputs")]
        public virtual TJournalTemplate JournalTemplate { get; set; }

    }
}
