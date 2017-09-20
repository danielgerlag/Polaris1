import {Component, OnInit, EventEmitter} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ControlGroup} from 'angular2/common';
import {Typeahead} from 'ng2-bootstrap/ng2-bootstrap';
import {SuggestionResponse, ODataWrapper, ODataCollectionWrapper, ISearchService, IRemoteService} from 'app-core/app-core';


@Component({
    selector: 'entity-selector',
    inputs: ['value', 'path', 'columns'],
    outputs: ['valueChange'],
    template: `
    <div class="input-group">
    <input type="text" class="form-control" value="{{ lookupText }}" readonly />

    <div class="input-group-btn">
        <div class="btn-group" role="group">
            <div class="dropdown dropdown-lg">
                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-expanded="false"><span class="glyphicon glyphicon-search"></span></button>
                <div class="dropdown-menu dropdown-menu-right" role="menu">
                    <form class="form-horizontal" role="form">

                        <div class="input-group">
                            <input type="text" placeholder="Search"
                                   [(ngModel)]="searchQuery"
                                   [typeahead]="suggest()"
                                   [typeaheadWaitMs]="300"
                                   class="form-control">
                            <div class="input-group-btn">
                                <div class="btn-group" role="group">
                                    <button type="submit" class="btn btn-default" (click)="search()">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </form>

                    <table class="table table-hover">
                        <tr>
                            <th *ngFor="#col of columns">{{ col.Title }}</th>
                        </tr>
                        <tbody>
                            <tr *ngFor="#item of searchResult" (click)="select(item.ID)">
                                <td *ngFor="#col of columns">{{ drillObject(item, col.Name) }}</td>
                            </tr>
                        </tbody>
                    </table>


                </div>
            </div>
            <button type="button" class="btn btn-default"><span class="glyphicon glyphicon-new-window" aria-hidden="true"></span></button>
        </div>
    </div>


</div>`,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, Typeahead]
})
export class EntitySelector implements OnInit {

    private _entityId: number;
    searchQuery: string;
    searchResult: any[];
    lookupText: string;

    private valueChange: EventEmitter<any> = new EventEmitter();

    public columns: any;
    public path: string;

    remoteService: IRemoteService;
    searchService: ISearchService;

    get value() {
        return this._entityId;
    }
    set value(value) {
        this._entityId = value;
        this.onEntityChanged();
    }

    constructor(remoteService: IRemoteService, searchService: ISearchService) {
        this.remoteService = remoteService;
        this.searchService = searchService;
    }


    onEntityChanged() {
        if (this._entityId > 0) {
            this.remoteService.get(this, "Data.svc/GetLookupText?set='" + this.path + "'&id=" + this._entityId, (sender: EntitySelector, data: ODataWrapper<string>, status: number) => {
                sender.lookupText = data.value;
            });
        }
        else {
            this.lookupText = "";
        }
        this.valueChange.next(this._entityId);
    }

    search() {
        var self = this;
        self.remoteService.get(self, 'Data.svc/Search' + self.path + "?query='" + self.searchQuery +"'", self.onSearchResponse);
    }

    suggest() {
        var self = this;
        let f: Function = function (): Promise<string[]> {
            let p: Promise<string[]> = new Promise<string[]>((resolve: Function) => {
                self.searchService.suggest(self.searchQuery, self, (sender: any, data: SuggestionResponse) => {
                    resolve(data.Suggestions);
                });
            });
            return p;
        };
        return f;
    }

    protected onSearchResponse(sender: EntitySelector, data: ODataCollectionWrapper<any>, status: number): any {
        sender.searchResult = data.value;
    }

    select(id: number) {
        this.value = id;
    }

    onInit() {
        this.ngOnInit();
    }

    ngOnInit() {
    }


    private drillObject(data: any, path: string): any {
        var delimeter = path.indexOf(".");
        if (delimeter == -1) {
            return data[path];
        }
        else {
            var newPath = path.slice(delimeter + 1);
            var newData = data[path.slice(0, delimeter)];
            return this.drillObject(newData, newPath);
        }
    }
}
