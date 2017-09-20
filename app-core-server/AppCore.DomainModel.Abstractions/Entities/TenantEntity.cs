using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DomainModel.Abstractions.Entities
{
    public abstract class TenantEntity : BaseEntity
    {
        [Index]
        public int AppTenantID { get; set; }
    }
}
