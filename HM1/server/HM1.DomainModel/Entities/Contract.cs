using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.Modules.Financial.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace HM1.DomainModel
{
    public class Contract : BaseContract<AccountingEntity, Party, ScheduledJournal>
    {
        
    }
}
