using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Foundation.DomainModel
{
    public class SequenceNumber : TenantEntity
    {
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        public int NextValue { get; set; }

        [Required]
        [MaxLength(20)]
        public string FormatMask { get; set; }
    }
}
