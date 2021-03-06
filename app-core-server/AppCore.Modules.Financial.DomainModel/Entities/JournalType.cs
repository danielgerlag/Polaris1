﻿using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{
    public class JournalType : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        public virtual ICollection<JournalTxnType> JournalTxnTypes { get; set; } = new HashSet<JournalTxnType>();
    }
}
