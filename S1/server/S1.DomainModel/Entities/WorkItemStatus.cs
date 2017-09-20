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
    public class WorkItemStatus : BaseEntity
    {

        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsOpen { get; set; }

        public bool IsApproved { get; set; }
        
    }
}
