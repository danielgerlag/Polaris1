using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Intercepts;
using AppCore.DomainModel.Abstractions.Entities;
using System.ComponentModel.DataAnnotations;
using S1.DomainModel.Intercepts;

namespace S1.DomainModel
{
    [Intercept(typeof(NewWorkItem), Stage.OnAddBeforeCommit, 1)]
    public class WorkItem : TenantEntity
    {
        
        public int AccountingEntityID { get; set; }

        public virtual AccountingEntity AccountingEntity { get; set; }


        public int WorkItemStatusID { get; set; }

        public virtual WorkItemStatus WorkItemStatus { get; set; }


        [MaxLength(200)]
        public string Subject { get; set; }
                
        public string Description { get; set; }


        public virtual ICollection<WorkItemAttachment> Attachments { get; set; } = new HashSet<WorkItemAttachment>();


    }
}
