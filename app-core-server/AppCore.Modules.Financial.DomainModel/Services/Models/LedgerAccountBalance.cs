using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.Services.Models
{
    public class LedgerAccountBalance
    {
        
        public int LedgerAccountID { get; set; }
        public string LedgerAccountName { get; set; }
        public int AccountingEntityID { get; set; }
        public string AccountingEntityName { get; set; }

        public string LedgerAccountType { get; set; }

        public int PartyID { get; set; }
        public string PartyName { get; set; }
        

        public decimal Balance { get; set; }

    }
}
