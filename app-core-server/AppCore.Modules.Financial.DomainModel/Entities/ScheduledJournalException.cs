using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{
    public class ScheduledJournalException : TenantEntity
    {
        public int ScheduledJournalLogID { get; set; }
        public virtual ScheduledJournalLog ScheduledJournalLog { get; set; }


        public string Message { get; set; }

        [Required]
        [MaxLength(1)]
        public string ExceptionType { get; set; }
    }
}
