using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.Services.Models
{
    public class LedgerAccountBalanceRequest
    {
        public int AppTenantID { get; set; }
        public DateTime EffectiveDate { get; set; }        
        public int? AccountingEntityID { get; set; }
        public int LedgerID { get; set; }
        
    }
}
