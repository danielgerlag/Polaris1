using AppCore.DomainModel.Abstractions.Entities;
using AppCore.DomainModel.Interface;
using AppCore.Services.Indexer.Interface;
using AppCore.Services.Indexer.Model;
using AppCore.Services.Indexer.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Builder
{
    public class IndexBuilder : IIndexBuilder
    {
        private IIndexStore _indexContext { get; set; }
        private IDbContext _primaryContext { get; set; }
        private IIndexQueue _indexQueue { get; set; }
        private IIndexRegister _registry { get; set; }


        public IndexBuilder(IIndexStore indexContext, IDbContext primaryContext, IIndexQueue queue, IIndexRegister registry)
        {
            _indexContext = indexContext;
            _primaryContext = primaryContext;
            _indexQueue = queue;
            _registry = registry;
        }

        public void IndexEntity(Type entityType, int id, bool recursive, Type contextType)
        {
            DateTime startTime = DateTime.Now;
            IDomainEntity entity = GetEntity(entityType, id);
            IEntityIndexer indexer = _registry.GetIndexer(entityType);

            if ((entity != null) && (indexer != null))
            {
                EntityIndex index = _indexContext.GetEntityIndex(entityType.FullName, id);
                if (index != null)
                {
                    _indexContext.DeleteEntityIndex(index);
                }
                else
                {
                    index = new EntityIndex();
                    _indexContext.EntityIndexes.Add(index);
                }

                HashSet<SearchKeyword> keywords = ProcessKeywords(indexer.GetKeyWords(entity));

                index.EntityTypeID = _indexContext.GetEntityTypeID(entityType.FullName);
                index.EntityKey = id;

                if (entity is TenantEntity)
                    index.AppTenantID = (entity as TenantEntity).AppTenantID;
                else
                    index.AppTenantID = null;

                index.IndexTimeUTC = DateTime.Now.ToUniversalTime();

                foreach (var kw in keywords)
                {
                    EntityKeyword indexKW = new EntityKeyword();

                    if (kw.Keyword.Length < 50)
                        indexKW.Keyword = kw.Keyword;
                    else
                        indexKW.Keyword = kw.Keyword.Substring(0, 49);

                    if (kw.RefType != null)
                    {
                        indexKW.ReferenceEntityKey = kw.RefID;
                        indexKW.ReferenceEntityTypeID = _indexContext.GetEntityTypeID(kw.RefType.FullName);
                    }
                    index.Keywords.Add(indexKW);
                }


                //index.IndexDuration = (DateTime.Now.TimeOfDay - startTime.TimeOfDay).Ticks;                
            }

            if (recursive)
            {
                List<EntityIndex> relatedIndexes = _indexContext.GetRelatedIndexes(entityType.FullName, id);
                //Type.GetType(entityType.FullName, x => )
                foreach (var idx in relatedIndexes)
                {
                    Type relatedEntityType = _registry.LookupEntityType(idx.EntityType.Name);
                    _indexQueue.QueueIndexWork(relatedEntityType, idx.EntityKey, false, contextType);
                }
            }
        }

        private IDomainEntity GetEntity(Type entityType, int id)
        {
            object result = _primaryContext.Set(entityType).Find(id);
            if (result is IDomainEntity)
                return (result as IDomainEntity);
            return null;
        }

        private HashSet<SearchKeyword> ProcessKeywords(IEnumerable<SearchKeyword> keywords)
        {
            HashSet<SearchKeyword> result = new HashSet<SearchKeyword>();

            foreach (SearchKeyword source in keywords)
            {
                if (!String.IsNullOrWhiteSpace(source.Keyword))
                {
                    source.Keyword = source.Keyword.Replace("\r\n", " ");
                    string[] potentials = source.Keyword.Split(' ', ',', ';');

                    if (potentials.Length > 1)
                        result.Add(new SearchKeyword(source.Keyword.ToUpper(), source.RefType, source.RefID));

                    foreach (string potential in potentials)
                    {
                        if (!String.IsNullOrWhiteSpace(potential))
                        {
                            string actual = potential.Trim().ToUpper();
                            if (!result.Any(r => r.Keyword == actual))
                                result.Add(new SearchKeyword(actual, source.RefType, source.RefID));
                        }
                    }
                }
            }

            return result;
        }

    }
}
