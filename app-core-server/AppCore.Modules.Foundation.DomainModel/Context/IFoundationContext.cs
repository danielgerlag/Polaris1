using AppCore.DomainModel.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Modules.Foundation.DomainModel.Context
{
    public interface IFoundationContext<TParty> : IDbContext
        where TParty : BaseParty
    {
        IDbSet<TParty> Parties { get; set; }

        IDbSet<UserInputType> UserInputTypes { get; set; }

        //IDbSet<UserInputValue> UserInputValues { get; set; }
    }
        
}
