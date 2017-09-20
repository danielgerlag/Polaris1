using AppCore.DomainModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Interface
{
    public abstract class EntityIndexer<T> : IEntityIndexer
        where T : class
    {
        public IEnumerable<SearchKeyword> GetKeyWords(IDomainEntity entity)
        {
            return BuildKeyWords(entity as T);
        }

        public SearchResult GetSearchResult(IDomainEntity entity)
        {
            return BuildSearchResult(entity as T);
        }

        protected abstract IEnumerable<SearchKeyword> BuildKeyWords(T entity);
        protected abstract SearchResult BuildSearchResult(T entity);
        //int? GetDataAreaID();
    }

    public interface IEntityIndexer 
    {
        IEnumerable<SearchKeyword> GetKeyWords(IDomainEntity entity);
        SearchResult GetSearchResult(IDomainEntity entity);
        //int? GetDataAreaID();
    }
}
