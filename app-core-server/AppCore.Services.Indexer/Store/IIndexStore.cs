using AppCore.DomainModel.Interface;
using AppCore.Services.Indexer.Interface;
using AppCore.Services.Indexer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Store
{
    public interface IIndexStore : IDisposable
    {
        DbSet<EntityIndex> EntityIndexes { get; set; }
        DbSet<EntityKeyword> EntityKeywords { get; set; }
        DbSet<EntityType> EntityTypes { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync();

        void DeleteEntityIndex(EntityIndex index);
        EntityIndex GetEntityIndex(string entityType, int entityID);
        int GetEntityTypeID(string typeName);
        List<EntityIndex> GetRelatedIndexes(string entityType, int entityId);
        
    }
}
