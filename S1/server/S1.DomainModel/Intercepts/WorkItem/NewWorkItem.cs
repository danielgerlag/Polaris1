using AppCore.DomainModel.Abstractions.Intercepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCore.DomainModel.Interface;

namespace S1.DomainModel.Intercepts
{
    public class NewWorkItem : EntityIntercept<WorkItem>
    {

        public NewWorkItem()
        {

        }

        public override void Run(WorkItem entity, IDbContext dataContext)
        {
            var status = dataContext.Set<WorkItemStatus>().Single(x => x.Code == "OPEN");
            entity.WorkItemStatus = status;
        }
    }
}
