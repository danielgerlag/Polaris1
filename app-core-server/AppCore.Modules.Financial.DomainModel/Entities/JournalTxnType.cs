using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{
    public class JournalTxnType : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string PrimaryFactorCode { get; set; }
                
        [MaxLength(10)]
        public string SecondaryFactorCode { get; set; }


        public int JournalTypeID { get; set; }
        public virtual JournalType JournalType { get; set; }
    }
}
