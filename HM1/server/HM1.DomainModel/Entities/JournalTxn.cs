using AppCore.Modules.Financial.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.DomainModel
{
    public class JournalTxn : BaseJournalTxn<Journal, LedgerTxn, JournalTemplateTxn, Party>
    {

        public int? ParticipantID { get; set; }
        public virtual Participant Participant { get; set; }

    }
}
