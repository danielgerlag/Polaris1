using AppCore.Modules.Financial.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.DomainModel.Services
{
    public class JournalPoster : BaseJournalPoster<AccountingEntity, Journal, JournalTxn, LedgerTxn, JournalTemplate, JournalTemplateInput, JournalTemplateTxn, JournalTemplateTxnPosting, LedgerAccount, Party>
    {
    }
}
