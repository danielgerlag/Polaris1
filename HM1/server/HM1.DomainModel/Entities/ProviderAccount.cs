using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using AppCore.DomainModel.Abstractions.Intercepts;
using System.ComponentModel.DataAnnotations;
using AppCore.DomainModel.Abstractions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HM1.DomainModel
{
    public class ProviderAccount : TenantEntity
    {
        [ForeignKey("Contract")]
        public override int ID { get; set; }
                
        public virtual Contract Contract { get; set; }
                
        [MaxLength(500)]
        public string ServiceDescription { get; set; }
        

        public int BillingParticipantID { get; set; }
        public virtual Participant BillingParticipant { get; set; }

        public virtual ICollection<ProviderAccountParticipant> Participants { get; set; } = new HashSet<ProviderAccountParticipant>();

    }
}
