using AppCore.DomainModel.Abstractions.Entities;
using AppCore.Modules.Financial.DomainModel;
using AppCore.Modules.Foundation.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.HR.DomainModel
{
    public abstract class BaseEmployment : TenantEntity
    {
        
    }

    public abstract class BaseEmployment<TContract, TEmployee> : BaseEmployment
        where TContract : BaseContract
        where TEmployee : BaseEmployee
    {

        [ForeignKey("Contract")]
        public override int ID { get; set; }

        //public int ContractID { get; set; }
        public virtual TContract Contract { get; set; }

        public int EmployeeID { get; set; }
        public virtual TEmployee Employee { get; set; }
        
    }
}
