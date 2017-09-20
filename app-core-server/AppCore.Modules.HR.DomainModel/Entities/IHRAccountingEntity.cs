using AppCore.Modules.Financial.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.HR.DomainModel
{
    public interface IHRAccountingEntity<TEmployment>
        where TEmployment : BaseEmployment
    {
        //ICollection<TEmployment> Employments { get; set; }
    }
}
