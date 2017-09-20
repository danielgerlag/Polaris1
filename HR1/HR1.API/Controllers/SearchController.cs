using AppCore.API.Controllers;
using HR1.Services.Identity;
using HR1.Services.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppCore.API.Models.Account;
using System.Threading.Tasks;
using HR1.API.Models.Account;
using HR1.DomainModel.Context;
using Microsoft.AspNet.Identity;
using HR1.DomainModel;
using AppCore.Services.Identity;
using AppCore.DomainModel.Interface;
using AppCore.Services.Indexer.Interface;
using AppCore.API.Models.Search;

namespace HR1.API.Controllers
{
    [RoutePrefix("Search")]
    public class SearchController : BaseSearchController
    {

        public SearchController(IHR1Context appContext, IAppIdentityContext identityContext, ISearchService searchService)
            : base (appContext, identityContext, searchService)
        {

        }

        protected override string EntityNamespace
        {
            get
            {
                return "HR1.DomainModel";
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