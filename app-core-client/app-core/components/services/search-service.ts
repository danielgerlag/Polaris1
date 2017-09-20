import {Inject, Injectable} from 'angular2/core';
import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';

import {Subscription} from 'rxjs/Rx';
import {IShellService} from './shell-service';
import {IRemoteService} from './remote-service';
import {SearchRequest, SearchResponse, SearchResponseLine, SuggestionResponse, SearchType} from '../models';


export abstract class ISearchService {
    abstract search(query, searchType, sender: any, callback: (sender: any, data: SearchResponse) => any): void;
    abstract suggest(query, sender: any, callback: (sender: any, data: SuggestionResponse) => any): void;
    abstract selectResult(item: SearchResponseLine);
    abstract  getSearchTypes(): SearchType[];
}

@Injectable()
export abstract class SearchService implements ISearchService {

    protected remoteService: IRemoteService;
    protected shellService: IShellService;
    protected router: Router;

    constructor( @Inject(Router) router: Router, @Inject(IRemoteService) remoteService: IRemoteService, @Inject(IShellService) shellService: IShellService) {
        this.remoteService = remoteService;
        this.shellService = shellService;
        this.router = router;
    }

    public search(query, searchType, sender: any, callback: (sender: any, data: SearchResponse) => any): void {
        var request: SearchRequest = new SearchRequest();
        request.SearchQuery = query;
        request.SearchType = searchType;
        this.shellService.showLoader("Searching...");

        this.remoteService.post(this, 'Search/Search', request, (sender1: SearchService, data1: SearchResponse, status: number) => {
            sender1.shellService.hideLoader();
            callback(sender, data1);
        });
    }

    public suggest(query, sender: any, callback: (sender: any, data: SuggestionResponse) => any): void {
        var request: SearchRequest = new SearchRequest();
        request.SearchQuery = query;
        this.remoteService.post(this, 'Search/Suggest', request, (sender1: SearchService, data1: SuggestionResponse, status: number) => {
            callback(sender, data1);
        });
    }

    public abstract selectResult(item: SearchResponseLine) ;
    public abstract getSearchTypes(): SearchType[];

}
