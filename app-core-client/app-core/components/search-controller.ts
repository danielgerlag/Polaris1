import {OnInit} from 'angular2/core';
import {IShellService, ShellInfo} from './services/shell-service';
import {ISearchService} from './services/search-service';
import {SearchRequest, SearchType, SearchResponse, SearchResponseLine, SuggestionResponse} from './models'

export abstract class SearchController {

    private shellService: IShellService;
    private searchService: ISearchService;
    public searchStr: string;
    public searchType: string;
    public searchResponse: SearchResponse;
    public searchTypes: SearchType[];


    constructor(searchService: ISearchService, shellService: IShellService) {

        this.searchService = searchService;
        this.shellService = shellService;
        this.searchResponse = new SearchResponse();
        this.searchTypes = searchService.getSearchTypes();
        this.searchType = "";
    }

    public search() {
        this.searchService.search(this.searchStr, this.searchType, this, (sender: SearchController, data: SearchResponse) => {
            sender.searchResponse = data;
        });
    }

    public suggest(): Function {
        var self = this;
        let f: Function = function (): Promise<string[]> {
            let p: Promise<string[]> = new Promise<string[]>((resolve: Function) => {
                self.searchService.suggest(self.searchStr, self, (sender: SearchController, data: SuggestionResponse) => {
                    resolve(data.Suggestions);
                });

            });
            return p;
        };
        return f;
    }

    select(item: SearchResponseLine) {
        this.searchService.selectResult(item);
    }
}
