using AppCore.API.Controllers;
using AppCore.API.Models.Search;
using AppCore.Services.Identity;
using AppCore.Services.Indexer.Interface;
using HM1.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HM1.API.Controllers
{
    [RoutePrefix("Search")]
    public class SearchController : BaseSearchController
    {

        public SearchController(IHM1Context appContext, IAppIdentityContext identityContext, ISearchService searchService)
            : base(appContext, identityContext, searchService)
        {

        }

        protected override string EntityNamespace
        {
            get
            {
                return "HM1.DomainModel";
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
