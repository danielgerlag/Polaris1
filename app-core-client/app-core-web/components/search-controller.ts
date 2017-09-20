import {Component, View, OnInit, EventEmitter} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ControlGroup} from 'angular2/common';
import {Typeahead} from 'ng2-bootstrap/ng2-bootstrap';
import {ISearchService, IShellService, ILogService, SearchController} from 'app-core/app-core';


@Component({
    selector: 'search',
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, Typeahead],
    template: `
<div class="container-fluid">

    <div class="row">
        <div class="col-sm-6 col-sm-offset-3" role="search">
            <div class="input-group input-group-lg">
                <input type="text" placeholder="Search"
                       [(ngModel)]="searchStr"
                       [typeahead]="suggest()"
                       [typeaheadWaitMs]="300"
                       class="form-control"
                       style="width:100%">

                <select class="form-control" [(ngModel)]="searchType">
                    <option *ngFor="#type of searchTypes" value="{{type.Key}}" >{{type.Name}}</option>
                </select>

                <button type="submit" class="btn btn-primary btn-lg" (click)="search()">
                    <i class="fa fa-search"></i>
                </button>
            </div>

        </div>
    </div>


    <div>
        <div class="list-group">
            <button class="list-group-item" *ngFor="#result of searchResponse.Results" (click)="select(result)">
                <h5 class="list-group-item-heading"><b>{{ result.EntityType }}</b></h5>
                <h4 class="list-group-item-heading">{{ result.Reference }}</h4>
                <p class="list-group-item-text">{{ result.Name }}</p>
                <p class="list-group-item-text">{{ result.Summary }}</p>
            </button>
        </div>
    </div>

</div>
    `
})
export class SearchControllerWeb extends SearchController {
    constructor(searchService:ISearchService, shellService:IShellService) {
        super(searchService, shellService);
    }

}

