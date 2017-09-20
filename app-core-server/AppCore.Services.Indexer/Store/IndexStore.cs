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
    public partial class IndexStore : DbContext, IIndexStore
    {
        public IndexStore()
            : base("name=DB")
        {
            Database.SetInitializer<IndexStore>(new IndexDBInit());

        }

        public virtual DbSet<EntityType> EntityTypes { get; set; }
        public virtual DbSet<EntityIndex> EntityIndexes { get; set; }
        public virtual DbSet<EntityKeyword> EntityKeywords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }


        public EntityIndex GetEntityIndex(string entityType, int entityID)
        {
            return EntityIndexes.SingleOrDefault(ei => ei.EntityType.Name == entityType && ei.EntityKey == entityID);
        }

        public void DeleteEntityIndex(EntityIndex index)
        {
            foreach (var kw in index.Keywords.ToList())
                EntityKeywords.Remove(kw);
            //EntityIndexes.Remove(index);
        }

        public int GetEntityTypeID(string typeName)
        {
            return EntityTypes.Single(x => x.Name == typeName).ID;
        }
                
        

        public List<EntityIndex> GetRelatedIndexes(string entityType, int entityId)
        {
            var refType = EntityTypes.FirstOrDefault(x => x.Name == entityType);
            if (refType == null)
                return new List<EntityIndex>();

            List<EntityIndex> result = EntityIndexes.Where(ei => ei.Keywords.Count(kw => kw.ReferenceEntityTypeID == refType.ID && kw.ReferenceEntityKey == entityId) > 0).Distinct().ToList();
            return result;
        }
                

        /*
        public static List<Entity> GetUnindexedEntities(this DataService dataService)
        {
            List<Entity> result = new List<Entity>();

            var dataAssembly = System.Reflection.Assembly.GetAssembly(typeof(Entity));
            List<Type> indexedTypes = dataAssembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIndexedEntity)) && !t.IsAbstract).ToList();

            foreach (Type entityType in indexedTypes)
            {
                var method1 = dataService.Context.GetType().GetMethod("CreateObjectSet", new Type[] { });
                var method2 = method1.MakeGenericMethod(entityType);

                if (method2 != null)
                {
                    dynamic set = method2.Invoke(dataService.Context, new object[] { });
                    string key = set.EntitySet.ElementType.KeyMembers[0].Name;

                    result.AddRange(set.Where("COUNT(SELECT VALUE i.EntityIndexID from EntityIndexes as i WHERE i.SourceType = '" + entityType.Name + "' AND i.SourceID == it." + key + " ) == 0"));
                    result.AddRange(set.Where("COUNT(SELECT VALUE i.EntityIndexID from EntityIndexes as i WHERE ((i.IndexTS < it.EditedTS) OR it.EditedTS IS NULL) AND i.SourceType = '" + entityType.Name + "' AND i.SourceID == it." + key + " ) > 0"));
                }
            }

            return result;
        }

        public static List<Entity> GetNonActiveIndices(this DataService dataService)
        {
            List<Entity> result = new List<Entity>();

            var dataAssembly = System.Reflection.Assembly.GetAssembly(typeof(Entity));
            List<Type> indexedTypes = dataAssembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IIndexedEntity)) && !t.IsAbstract).ToList();

            foreach (Type entityType in indexedTypes)
            {
                var method1 = dataService.Context.GetType().GetMethod("CreateObjectSet", new Type[] { });
                var method2 = method1.MakeGenericMethod(entityType);

                if (method2 != null)
                {
                    dynamic set = method2.Invoke(dataService.Context, new object[] { });
                    string key = set.EntitySet.ElementType.KeyMembers[0].Name;

                    string queryString = @"SELECT VALUE e FROM EntityIndexes AS e WHERE e.SourceType = '" + entityType.Name + "' AND NOT EXISTS (SELECT p." + key + " FROM " + set.EntitySet.ToString() + " AS p WHERE p." + key + " = e.SourceID AND e.SourceType = '" + entityType.Name + "')";

                    ObjectQuery<EntityIndex> indexqry =
                        new ObjectQuery<EntityIndex>(queryString, dataService.Context as ObjectContext);
                    result.AddRange(indexqry);
                }
            }
            return result;
        }
        */

    }

    public class IndexDBInit : CreateDatabaseIfNotExists<IndexStore>
    {

        protected override void Seed(IndexStore context)
        {

            base.Seed(context);
        }
    }
}
