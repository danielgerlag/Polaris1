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
    public class ProviderAccountParticipant : TenantEntity
    {

        public decimal Percentage { get; set; }

        public int ProviderAccountID { get; set; }
        public virtual ProviderAccount ProviderAccount { get; set; }

        public int ParticipantID { get; set; }
        public virtual Participant Participant { get; set; }

    }
}
