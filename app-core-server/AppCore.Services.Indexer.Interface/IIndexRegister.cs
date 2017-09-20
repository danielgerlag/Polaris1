using AppCore.DomainModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Interface
{
    public interface IIndexRegister
    {        
        void RegisterEntityTypes(Assembly assembly);
        Type LookupEntityType(string name);
        IEntityIndexer GetIndexer(Type entityType);
        bool CanIndex(IDomainEntity entity);
    }
}
