using AppCore.DomainModel.Abstractions.Intercepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel;
using AppCore.Modules.Foundation.DomainModel;
using AppCore.Modules.Financial.DomainModel.Services.Interfaces;

namespace HM1.DomainModel.Intercepts
{
    public class OnceOffJournal : EntityIntercept<ScheduledJournal>
    {
        public OnceOffJournal()
        {

        }

        public override void Run(ScheduledJournal entity, IDbContext dataContext)
        {
            if ((entity.Frequency == "O") && (entity.NextExecutionDate.HasValue))
            {
            //    if (entity.NextExecutionDate.Value <= DateTime.Now)
            //    {
            //        var runner = AppCore.IoC.Container.Resolve<IScheduledJournalRunner>();
            //        var runResult = runner.Run(dataContext, entity);

            //        if (runResult.Errors.Count() > 0)
            //        {
            //            throw new Exception(runResult.Errors.First());
            //        }
            //    }
            }
            
        }

    }
}
