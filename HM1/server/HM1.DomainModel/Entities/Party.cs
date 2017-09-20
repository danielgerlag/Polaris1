using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Foundation.DomainModel;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Entities;

namespace HM1.DomainModel
{
    public class Party : BaseParty, IPartyRole
    {
        public int GetPartyID()
        {
            return ID;
        }

        public TenantEntity GetRoleEntity()
        {
            return this;             
        }
    }
}
