using AppCore.DomainModel.Interface;
using AppCore.Services.Indexer.Interface;
using AppCore.Services.Indexer.Model;
using AppCore.Services.Indexer.Store;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer
{
    public class SearchService : ISearchService
    {
        private readonly IIndexStore _store;
        private readonly IIndexRegister _register;

        public SearchService(IIndexStore store, IIndexRegister register)
        {
            _store = store;
            _register = register;
        }


        public List<SearchResult> Search(string searchString, string searchType, string entityNamespace, IDbContext dataContext, int? appTenantID = null)
        {
            List<SearchResult> result = new List<SearchResult>();
            string[] keywords = new string[0];

            if (searchString == null)
                return result;

            if (searchString.Count(c => c == ' ') >= (searchString.Length / 2))
            {
                keywords = new string[1] { searchString.ToUpper().Trim() };
            }
            else
            {
                keywords = searchString.ToUpper().Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (keywords.Length > 0)
            {
                IQueryable<EntityIndex> query = _store.EntityIndexes.AsQueryable();

                if (appTenantID != null)
                    query = query.Where(x => x.AppTenantID == appTenantID);

                if (!String.IsNullOrEmpty(searchType))
                {
                    string searchTypeName = entityNamespace + "." + searchType;
                    query = query.Where(idx => idx.EntityType.Name == searchTypeName);
                }

                if (searchString != "*")
                {
                    foreach (string keyword in keywords)
                        query = query.Where(idx => idx.Keywords.Count(kw => kw.Keyword.StartsWith(keyword)) > 0);
                }

                var queryResult = query.ToList();

                foreach (var item in queryResult)
                {
                    Type entityType = _register.LookupEntityType(item.EntityType.Name);
                    IEntityIndexer indexer = _register.GetIndexer(entityType);
                    object entity = dataContext.Set(entityType).Find(item.EntityKey);

                    if ((entity != null) && (indexer != null))
                    {
                        var resultLine = indexer.GetSearchResult(entity as IDomainEntity);
                        resultLine.Entity = (entity as IDomainEntity);
                        resultLine.Rank = 100;
                        result.Add(resultLine);
                    }
                }
            }

            return result.OrderBy(r => r.Rank).ThenBy(r => r.Name).ToList();
        }

        public List<T> SearchEntity<T>(string searchString, IDbContext dataContext, IEnumerable<string> include, int? appTenantID = null)
            where T : class, IDomainEntity
        {
            List<T> result = new List<T>();
            string[] keywords = new string[0];

            if (searchString.Count(c => c == ' ') >= (searchString.Length / 2))
            {
                keywords = new string[1] { searchString.ToUpper().Trim() };
            }
            else
            {
                keywords = searchString.ToUpper().Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
            string typeName = typeof(T).FullName;
            IQueryable<EntityIndex> query = _store.EntityIndexes.Where(x => x.EntityType.Name == typeName).AsQueryable();
            if (appTenantID != null)
                query = query.Where(x => x.AppTenantID == appTenantID);

            if (keywords.Length > 0)
            {
                foreach (string keyword in keywords)
                    query = query.Where(idx => idx.Keywords.Count(kw => kw.Keyword.StartsWith(keyword)) > 0);

                var queryResult = query.ToList();

                foreach (var item in queryResult)
                {
                    var findQry = dataContext.Set<T>().Where(x => x.ID == item.EntityKey);
                    foreach (string inc in include)
                        findQry = findQry.Include(inc);
                    T entity = findQry.FirstOrDefault();
                    if (entity != null)
                        result.Add(entity);
                }
            }

            return result;
        }

        public IQueryable<EntityIndex> PrepareSearchQuery(string searchString, int? appTenantID = null)
        {
            IQueryable<EntityIndex> query = _store.EntityIndexes.AsQueryable();
            if (appTenantID != null)
                query = query.Where(x => x.AppTenantID == appTenantID);

            string[] keywords = new string[0];

            if (searchString.Count(c => c == ' ') >= (searchString.Length / 2))
            {
                keywords = new string[1] { searchString.ToUpper().Trim() };
            }
            else
            {
                keywords = searchString.ToUpper().Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (keywords.Length > 0)
            {
                foreach (string keyword in keywords)
                    query = query.Where(idx => idx.Keywords.Count(kw => kw.Keyword.StartsWith(keyword)) > 0);
            }

            return query;
        }

        public List<string> GetSearchSuggestions(string searchString, int? appTenantID = null)
        {
            List<string> result = new List<string>();

            string[] keywords = searchString.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] keywordsUncased = searchString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (searchString.Length > 1)
            {
                string predicate = string.Empty;
                for (int i = 0; i < keywordsUncased.Count() - 1; i++)
                    predicate = predicate + keywordsUncased[i] + " ";

                IQueryable<EntityKeyword> query = _store.EntityKeywords.AsQueryable();

                if (appTenantID != null)
                    query = query.Where(x => x.EntityIndex.AppTenantID == appTenantID);

                string lastKeyword = keywords.Last();

                foreach (string keyword in keywords)
                    query = query.Where(idx => idx.Keyword != keyword && idx.Keyword.StartsWith(lastKeyword) && idx.EntityIndex.Keywords.Count(kw => kw.Keyword.StartsWith(keyword)) > 0);

                List<string> newItems = query.Select(kw => predicate + kw.Keyword.ToLower()).Distinct().Take(10).ToList();

                result.AddRange(newItems);
            }

            return result;
        }
    }
}
