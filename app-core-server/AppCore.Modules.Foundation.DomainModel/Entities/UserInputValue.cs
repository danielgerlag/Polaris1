using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Foundation.DomainModel
{
    public abstract class UserInputValue : TenantEntity
    {        
        [MaxLength(500)]
        public string Value { get; set; }                

    }
}
