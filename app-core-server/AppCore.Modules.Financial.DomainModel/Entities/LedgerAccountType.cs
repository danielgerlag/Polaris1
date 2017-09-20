using AppCore.DomainModel.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel
{
    public class LedgerAccountType : BaseEntity
    {

        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsCurrent { get; set; }

        [Required]
        public bool IsAsset { get; set; }

        [Required]
        public bool IsLiability { get; set; }

        [Required]
        public bool IsIncome { get; set; }

        [Required]
        public bool IsExpense { get; set; }

        [Required]
        public bool IsDebtor { get; set; }

        [Required]
        public bool IsCredior { get; set; }

        [Required]
        public bool IsControl { get; set; }

        [Required]
        public bool IsEquity { get; set; }

        [Required]
        public bool CreditPositive { get; set; }
    }
}
