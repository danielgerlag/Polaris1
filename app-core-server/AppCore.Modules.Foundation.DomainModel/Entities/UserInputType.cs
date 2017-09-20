using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Foundation.DomainModel
{
    public class UserInputType : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(10)]
        public string Code { get; set; }
    }
}
