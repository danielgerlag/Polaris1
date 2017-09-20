using AppCore.Services.Indexer.Interface;
using AppCore.Services.Indexer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppCore.DomainModel.Interface;
using AppCore.DomainModel.Abstractions.Entities;

namespace AppCore.Services.Indexer.Store
{
    public class IndexRegister : IIndexRegister
    {
        private readonly IIndexStore _indexStore;
        //private readonly IServiceProvider _serviceProvider;

        private List<string> _processedAssemblies = new List<string>();
        private Dictionary<string, Type> _entityTypes = new Dictionary<string, Type>();
        private Dictionary<Type, Type> _indexers = new Dictionary<Type, Type>();

        public IndexRegister(IIndexStore store)
        {
            _indexStore = store;
            //_serviceProvider = serviceProvider;
        }

        public void RegisterEntityTypes(Assembly assembly)
        {
            if (_processedAssemblies.Contains(assembly.FullName))
                return;                       

            _processedAssemblies.Add(assembly.FullName);

            Type[] types = assembly.GetTypes();

            //foreach (var t in types.Where(x => x.IsClass && !x.IsAbstract).Where(x => x.GetInterfaces().Any(y => y.GetGenericTypeDefinition() == typeof(EntityIndexer<>))))
            foreach (var t in types.Where(x => x.IsClass && !x.IsAbstract).Where(x => x.GetInterfaces().Any(y => y == typeof(IEntityIndexer))))
            {
                var entityType = GetEntityTypeFromIndexer(t);
                if (entityType != null)
                    _indexers.Add(entityType, t);
            }

            var indexableTypes = types
                .Where(t => t.IsSubclassOf(typeof(BaseEntity)) && (!t.IsAbstract))
                .ToList();

            foreach (var type in indexableTypes)
            {
                _entityTypes.Add(type.FullName, type);
                int count = _indexStore.EntityTypes.Count(x => x.Name == type.FullName);
                if (count == 0)
                {
                    _indexStore.EntityTypes.Add(new EntityType() { Name = type.FullName });
                }
            }
            _indexStore.SaveChanges();
        }

        public Type LookupEntityType(string name)
        {
            return _entityTypes[name];
        }

        public IEntityIndexer GetIndexer(Type entityType)
        {
            object indexer = IoC.Container.Resolve(_indexers[entityType]); //_serviceProvider.GetService(_indexers[entityType]);
            return (indexer as IEntityIndexer);
        }

        public bool CanIndex(IDomainEntity entity)
        {
            var type = entity.GetType();

            if (type.FullName.StartsWith("System.Data.Entity.DynamicProxies"))
                type = type.BaseType;

            return _indexers.ContainsKey(type);
        }

        private Type GetEntityTypeFromIndexer(Type indexerType)
        {
            if (indexerType.BaseType != null)
            {
                var generic = indexerType.BaseType.GetGenericTypeDefinition();
                if (generic == typeof(EntityIndexer<>))
                {
                    return indexerType.BaseType.GenericTypeArguments.First();
                }
            }
            return null;            
        }
    }
}
