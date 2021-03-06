﻿using AppCore.DomainModel.Abstractions.Intercepts;
using AppCore.Modules.Financial.DomainModel;
using HM1.DomainModel.Intercepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.DomainModel
{
    [Intercept(typeof(OnceOffJournal), Stage.OnAddBeforeCommit, 99)]
    public class ScheduledJournal : BaseScheduledJournal<AccountingEntity, Contract, JournalTemplate, ScheduledJournalInputValue>
    {
    }
}
