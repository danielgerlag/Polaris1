using AppCore.DomainModel.Abstractions.Entities;
using AppCore.DomainModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DomainModel.Abstractions.Intercepts
{
    public interface IEntityIntercept<T>
        where T : BaseEntity
    {
        void Run(T entity, IDbContext dataContext);
    }

    public interface IEntityIntercept
    {
        void Execute(BaseEntity entity, IDbContext dataContext);
    }

    public abstract class EntityIntercept<T> : IEntityIntercept, IEntityIntercept<T>
        where T : BaseEntity
    {
        public void Execute(BaseEntity entity, IDbContext dataContext)
        {
            Run(entity as T, dataContext);
        }
        public abstract void Run(T entity, IDbContext dataContext);
    }
}
