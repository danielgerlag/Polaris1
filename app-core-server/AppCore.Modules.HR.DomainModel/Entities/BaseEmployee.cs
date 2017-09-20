using AppCore.DomainModel.Abstractions.Entities;
using AppCore.Modules.Financial.DomainModel;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.HR.DomainModel
{
    public abstract class BaseEmployee : TenantEntity
    {
        [MaxLength(100)]
        public string EmployeeNumber { get; set; }
    }

    public abstract class BaseEmployee<TParty> : BaseEmployee        
        where TParty : BaseParty
    {                

        public int PartyID { get; set; }
        public virtual TParty Party { get; set; }
        
    }
}
