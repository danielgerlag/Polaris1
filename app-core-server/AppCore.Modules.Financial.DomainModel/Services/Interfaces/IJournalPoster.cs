using AppCore.DomainModel.Interface;
using AppCore.Modules.Financial.DomainModel.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Financial.DomainModel.Services.Interfaces
{
    public interface IJournalPoster
    {
        JournalPostResult Post(IDbContext db, BaseJournal journal);
    }
}
