using AppCore.API.Models.Search;
using AppCore.DomainModel.Interface;
using AppCore.Services.Identity;
using AppCore.Services.Indexer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.API.Controllers
{
    public abstract class BaseSearchController : AppApiController
    {
        private ISearchService _searchService;

        public BaseSearchController(IDbContext appContext, IAppIdentityContext identityContext, ISearchService searchService)
            :base(appContext, identityContext)
        {
            _searchService = searchService;
        }

        protected abstract string EntityNamespace { get; }
        
        public virtual async Task<SearchResponse> Search(SearchRequest request)
        {
            int tenantID = GetAppTenantID();
            List<SearchResult> data = await Task.Factory.StartNew<List<SearchResult>>(new Func<List<SearchResult>>(() =>
            {
                List<SearchResult> rawResult = new List<SearchResult>();
                rawResult.AddRange(_searchService.Search(request.SearchQuery, request.SearchType, EntityNamespace, _appContext, tenantID));
                return rawResult;
            }));

            SearchResponse result = new SearchResponse();

            result.Results = data.Select(x => new SearchResponseLine()
            {
                EntityType = x.EntityType,
                ID = x.ID,
                Name = x.Name,
                Number = x.Number,
                Reference = x.Reference,
                Summary = x.Summary
            }).ToList();

            return result;
        }

        
        public virtual async Task<SuggestionResponse> Suggest(SearchRequest request)
        {
            SuggestionResponse result = new SuggestionResponse();
            int tenantID = GetAppTenantID();
            
            result.Suggestions = await Task.Factory.StartNew<List<string>>(new Func<List<string>>(() =>
            {
                List<string> rawResult = new List<string>();
                rawResult.AddRange(_searchService.GetSearchSuggestions(request.SearchQuery, tenantID));
                return rawResult;
            }));

            return result;
        }
    }
}
