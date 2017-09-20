using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Identity
{
    public abstract class BaseAppTenant
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        //[Index(IsUnique = true)]
        //public Guid Key { get; set; }
    }
}
