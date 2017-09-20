using AppCore.DomainModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Interface
{
    public interface ISearchService
    {
        List<SearchResult> Search(string searchString, string searchType, string entityNamespace, IDbContext dataContext, int? appTenantID = null);
        List<T> SearchEntity<T>(string searchString, IDbContext dataContext, IEnumerable<string> include, int? appTenantID = null) where T : class, IDomainEntity;
        List<string> GetSearchSuggestions(string searchString, int? appTenantID = null);
    }
}
