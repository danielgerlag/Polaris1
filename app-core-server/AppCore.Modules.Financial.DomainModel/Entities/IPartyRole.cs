using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{
    public interface IPartyRole
    {
        int GetPartyID();
        TenantEntity GetRoleEntity();
    }
}
