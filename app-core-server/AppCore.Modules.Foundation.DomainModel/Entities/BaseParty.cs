using AppCore.DomainModel.Abstractions.Entities;
using AppCore.Modules.Foundation.DomainModel.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Foundation.DomainModel
{
    [PartyNameValidator]
    public abstract class BaseParty : TenantEntity
    {
        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string Surname { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1)]
        [Required]
        public string PartyType { get; set; }

    }
}
