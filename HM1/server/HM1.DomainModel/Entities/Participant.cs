using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Intercepts;
using System.ComponentModel.DataAnnotations;
using AppCore.DomainModel.Abstractions.Entities;

namespace HM1.DomainModel
{
    public class Participant : TenantEntity, IPartyRole
    {
                
        public int PartyID { get; set; }
        public virtual Party Party { get; set; }

        public virtual ICollection<ProviderAccountParticipant> ProviderAccounts { get; set; } = new HashSet<ProviderAccountParticipant>();

        public int GetPartyID()
        {
            return PartyID;
        }

        public TenantEntity GetRoleEntity()
        {
            return this;
        }
    }
}
