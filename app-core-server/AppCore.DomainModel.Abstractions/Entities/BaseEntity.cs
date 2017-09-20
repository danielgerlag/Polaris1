using AppCore.DomainModel.Abstractions.Intercepts;
using AppCore.DomainModel.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DomainModel.Abstractions.Entities
{
    public abstract class BaseEntity : IDomainEntity
    {
        [Key]
        public virtual int ID { get; set; }

        //[Timestamp]
        //[ConcurrencyCheck]
        //public byte[] RowVersion { get; set; }

        public DateTime DateCreatedUTC { get; set; }

        public DateTime DateModifiedUTC { get; set; }
                

        public event EntityEventHandler OnAddBeforeCommit;
        public event EntityEventHandler OnAddAfterCommit;
        public event EntityEventHandler OnChangeBeforeCommit;        
        public event EntityEventHandler OnChangeAfterCommit;

        public void RaiseOnAddBeforeCommit(IDbContext dataService)
        {
            if (OnAddBeforeCommit != null)
            {
                EntityEventArgs args = new EntityEventArgs() { DataService = dataService, Entity = this };
                OnAddBeforeCommit(this, args);
            }
        }
        public void RaiseOnChangeBeforeCommit(IDbContext dataService)
        {
            if (OnChangeBeforeCommit != null)
            {
                EntityEventArgs args = new EntityEventArgs() { DataService = dataService, Entity = this };
                OnChangeBeforeCommit(this, args);
            }
        }
        public void RaiseOnAddAfterCommit(IDbContext dataService)
        {
            if (OnAddAfterCommit != null)
            {
                EntityEventArgs args = new EntityEventArgs() { DataService = dataService, Entity = this };
                OnAddAfterCommit(this, args);
            }
        }
        public void RaiseOnChangeAfterCommit(IDbContext dataService)
        {
            if (OnChangeAfterCommit != null)
            {
                EntityEventArgs args = new EntityEventArgs() { DataService = dataService, Entity = this };
                OnChangeAfterCommit(this, args);
            }
        }

        public virtual string GetLookupText()
        {
            return string.Empty;
        }


        internal bool InterceptorsAttached { get; set; } = false;
        internal List<AttachedIntercept> Intercepts = new List<AttachedIntercept>();

    }

    public class EntityEventArgs : EventArgs
    {
        public IDbContext DataService { get; set; }
        public BaseEntity Entity { get; set; }
    }

    public delegate void EntityEventHandler(object sender, EntityEventArgs e);

    internal class AttachedIntercept
    {
        public InterceptAttribute Metadata { get; set; }
        public IEntityIntercept Intercept { get; set; }
    }
}
