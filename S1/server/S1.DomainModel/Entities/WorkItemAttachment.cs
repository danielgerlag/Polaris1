using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Intercepts;
using AppCore.DomainModel.Abstractions.Entities;
using System.ComponentModel.DataAnnotations;

namespace S1.DomainModel
{    
    public class WorkItemAttachment : TenantEntity
    {
        
        public int WorkItemID { get; set; }
        public virtual WorkItem WorkItem { get; set; }

        public int DocumentContentID { get; set; }
        public virtual DocumentContent DocumentContent { get; set; }


        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]        
        public string Description { get; set; }

        
    }
}
