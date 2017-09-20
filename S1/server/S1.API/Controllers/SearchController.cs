using AppCore.API.Controllers;
using AppCore.API.Models.Search;
using AppCore.Services.Identity;
using AppCore.Services.Indexer.Interface;
using S1.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace S1.API.Controllers
{
    [RoutePrefix("Search")]
    public class SearchController : BaseSearchController
    {

        public SearchController(IS1Context appContext, IAppIdentityContext identityContext, ISearchService searchService)
            : base(appContext, identityContext, searchService)
        {

        }

        protected override string EntityNamespace
        {
            get
            {
                return "S1.DomainModel";
            }
        }

        [Route("Search")]
        public override Task<SearchResponse> Search(SearchRequest request)
        {
            return base.Search(request);
        }

        [Route("Suggest")]
        public override Task<SuggestionResponse> Suggest(SearchRequest request)
        {
            return base.Suggest(request);
        }
    }
}