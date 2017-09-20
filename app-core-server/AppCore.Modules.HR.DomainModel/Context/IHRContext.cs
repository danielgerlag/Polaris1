using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.HR.DomainModel.Context
{
    public interface IHRContext<TEmployee, TEmployment> : IDbContext
        where TEmployee : BaseEmployee
        where TEmployment : BaseEmployment
    {
        IDbSet<TEmployee> Employees { get; set; }
        IDbSet<TEmployment> Employments { get; set; }
    }
}
