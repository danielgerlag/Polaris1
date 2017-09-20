using AppCore.DomainModel.Abstractions.Annotations;
using AppCore.DomainModel.Abstractions.Entities;
using AppCore.DomainModel.Abstractions.Intercepts;
using AppCore.DomainModel.Interface;
using AppCore.Services.Indexer.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.DomainModel.Abstractions
{
    public abstract class BaseContext : DbContext, IDbContext
    {

        private readonly IIndexRegister _indexRegistry;
        private readonly IIndexQueue _indexQueue;

        public BaseContext(string connectionString, IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base(connectionString)
        {
            _indexQueue = indexQueue;
            _indexRegistry = indexRegistry;
        }

        public BaseContext(IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base("name=DB")
        {
            _indexQueue = indexQueue;
            _indexRegistry = indexRegistry;
        }

        public BaseContext(DbConnection connection, IIndexQueue indexQueue, IIndexRegister indexRegistry)
            : base(connection, true)
        {
            _indexQueue = indexQueue;
            _indexRegistry = indexRegistry;
        }

        private ConcurrentQueue<BaseEntity> _postCommitAddedQueue = new ConcurrentQueue<BaseEntity>();
        private ConcurrentQueue<BaseEntity> _postCommitChangedQueue = new ConcurrentQueue<BaseEntity>();
        private bool _addedPostCommitEventToTxn = false;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                                    
            foreach (Type classType in GetInterfaceType().Assembly.GetTypes().Where(x => x.IsClass))                                       
            {
                foreach (var propAttr in classType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetCustomAttribute<DecimalPrecisionAttribute>() != null).Select(
                       p => new { prop = p, attr = p.GetCustomAttribute<DecimalPrecisionAttribute>(true) }))
                {

                    var entityConfig = modelBuilder.GetType().GetMethod("Entity").MakeGenericMethod(classType).Invoke(modelBuilder, null);
                    ParameterExpression param = ParameterExpression.Parameter(classType, "c");
                    Expression property = Expression.Property(param, propAttr.prop.Name);
                    LambdaExpression lambdaExpression = Expression.Lambda(property, true,
                                                                             new ParameterExpression[]
                                                                                 {param});
                    DecimalPropertyConfiguration decimalConfig;
                    if (propAttr.prop.PropertyType.IsGenericType && propAttr.prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[7];
                        decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
                    }
                    else
                    {
                        MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[6];
                        decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
                    }

                    decimalConfig.HasPrecision(propAttr.attr.Precision, propAttr.attr.Scale);
                }
            }


        }

        public override int SaveChanges()
        {
            try
            {
                PreSave();
                int count = base.SaveChanges();
                PostSave();
                return count;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errs = ex.EntityValidationErrors;
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public override async Task<int> SaveChangesAsync()
        {
            PreSave();
            int count = await base.SaveChangesAsync();
            PostSave();
            return count;
        }

        public override DbSet Set(Type entityType)
        {
            return base.Set(entityType);
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public new DbChangeTracker ChangeTracker
        {
            get { return base.ChangeTracker; }
        }

        public new DbContextConfiguration Configuration
        {
            get { return base.Configuration; }
        }

        public new Database Database
        {
            get { return base.Database; }
        }

        private void PreSave()
        {
            List<DbEntityEntry> processedEntities = new List<DbEntityEntry>();
            var trackedEntities = ChangeTracker.Entries();

            while (processedEntities.Count() < trackedEntities.Count())
            {
                PreProcessTrackedEntities(trackedEntities, processedEntities);
                trackedEntities = ChangeTracker.Entries();
            }
        }

        private void PreProcessTrackedEntities(IEnumerable<DbEntityEntry> trackedEntities, List<DbEntityEntry> processedEntities)
        {
            foreach (var item in trackedEntities)
            {
                if (!processedEntities.Contains(item))
                {
                    if (item.Entity is BaseEntity)
                    {
                        AttachIntercepts(item.Entity as BaseEntity);

                        if (item.State == EntityState.Added)
                        {
                            (item.Entity as BaseEntity).DateCreatedUTC = DateTime.UtcNow;
                            (item.Entity as BaseEntity).DateModifiedUTC = DateTime.UtcNow;

                            (item.Entity as BaseEntity).RaiseOnAddBeforeCommit(this);

                            if ((!_postCommitAddedQueue.Contains(item.Entity)))
                                _postCommitAddedQueue.Enqueue(item.Entity as BaseEntity);
                        }

                        if (item.State == EntityState.Modified)
                        {
                            (item.Entity as BaseEntity).DateModifiedUTC = DateTime.UtcNow;

                            (item.Entity as BaseEntity).RaiseOnChangeBeforeCommit(this);

                            if ((!_postCommitChangedQueue.Contains(item.Entity)))
                                _postCommitChangedQueue.Enqueue(item.Entity as BaseEntity);
                        }

                        //todo: deleted events                    
                    }
                    processedEntities.Add(item);
                }
            }
        }

        private void PostSave()
        {
            var openTransaction = System.Transactions.Transaction.Current;
            if (openTransaction == null)
            {
                //Task.Factory.StartNew(new Action(() => { PerformPostCommit(); }));
                PerformPostCommit();
            }
            else
            {
                if (!_addedPostCommitEventToTxn)
                {
                    openTransaction.TransactionCompleted += openTransaction_TransactionCompleted;
                    _addedPostCommitEventToTxn = true;
                }
            }
        }

        private void PerformPostCommit()
        {
            BaseEntity entity;
            while (_postCommitAddedQueue.TryDequeue(out entity))
            {
                if (entity is BaseEntity)
                {
                    entity.RaiseOnAddAfterCommit(this);
                    QueueIndex(entity);
                }
            }

            while (_postCommitChangedQueue.TryDequeue(out entity))
            {
                if (entity is BaseEntity)
                {
                    entity.RaiseOnChangeAfterCommit(this);
                    QueueIndex(entity);
                }
            }
        }


        private void QueueIndex(BaseEntity entity)
        {
            if (_indexRegistry != null)
            {
                if (_indexRegistry.CanIndex(entity))
                {
                    Type type = entity.GetType();
                    if (type.FullName.StartsWith("System.Data.Entity.DynamicProxies"))
                        type = type.BaseType;
                    _indexQueue.QueueIndexWork(type, entity.ID, true, GetInterfaceType());
                }
            }
        }

        void openTransaction_TransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        {
            PerformPostCommit();
        }

        private void AttachIntercepts(BaseEntity entity)
        {
            if (!entity.InterceptorsAttached)
            {
                var attributes = entity.GetType().GetCustomAttributes(typeof(InterceptAttribute), true).Cast<InterceptAttribute>();

                foreach (var interceptAttr in attributes)
                {
                    IEntityIntercept intercept = (IoC.Container.Resolve(interceptAttr.Intercept) as IEntityIntercept);
                    entity.Intercepts.Add(new AttachedIntercept() { Metadata = interceptAttr, Intercept = intercept });
                }

                entity.OnAddAfterCommit += Entity_OnAddAfterCommit;
                entity.OnAddBeforeCommit += Entity_OnAddBeforeCommit;
                entity.OnChangeAfterCommit += Entity_OnChangeAfterCommit;
                entity.OnChangeBeforeCommit += Entity_OnChangeBeforeCommit;
                entity.InterceptorsAttached = true;
            }
        }

        private void Entity_OnChangeBeforeCommit(object sender, EntityEventArgs e)
        {
            var intercepts = e.Entity.Intercepts.Where(x => x.Metadata.Stage == Stage.OnChangeBeforeCommit).OrderBy(x => x.Metadata.Order);
            foreach (var intercept in intercepts)
            {
                intercept.Intercept.Execute(e.Entity, e.DataService);
            }
        }

        private void Entity_OnChangeAfterCommit(object sender, EntityEventArgs e)
        {
            var intercepts = e.Entity.Intercepts.Where(x => x.Metadata.Stage == Stage.OnChangeAfterCommit).OrderBy(x => x.Metadata.Order);
            foreach (var intercept in intercepts)
            {
                intercept.Intercept.Execute(e.Entity, e.DataService);
            }
        }

        private void Entity_OnAddBeforeCommit(object sender, EntityEventArgs e)
        {
            var intercepts = e.Entity.Intercepts.Where(x => x.Metadata.Stage == Stage.OnAddBeforeCommit).OrderBy(x => x.Metadata.Order);
            foreach (var intercept in intercepts)
            {
                intercept.Intercept.Execute(e.Entity, e.DataService);
            }
        }

        private void Entity_OnAddAfterCommit(object sender, EntityEventArgs e)
        {
            var intercepts = e.Entity.Intercepts.Where(x => x.Metadata.Stage == Stage.OnAddAfterCommit).OrderBy(x => x.Metadata.Order);
            foreach (var intercept in intercepts)
            {
                intercept.Intercept.Execute(e.Entity, e.DataService);
            }
        }

        protected abstract Type GetInterfaceType();


    }
}
