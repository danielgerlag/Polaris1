import {NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {Router} from 'angular2/router';
import {Component} from 'angular2/core';
import {IDataService, IAuthService, IShellService} from 'app-core/app-core';
import {DataGrid, WebListController, ListHeader} from 'app-core-web/app-core-web';

@Component({
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, DataGrid, ListHeader],
    template: `
    <listHeader title="{{title}}" (add)="addItem()" (open)="openItem()" (delete)="deleteItem()"
                [canAdd]="canAdd()" [canOpen]="canOpen()" [canDelete]="canDelete()">
    <div dataGrid
         [dataSource]="dataSource"
         [(selectedItem)]="selectedItem"
         [columns]="columns"
         height="300">
    </div>
</listHeader>
    `
})
export class WorkItemListWeb extends WebListController {

    constructor(shellService: IShellService, dataService: IDataService, router: Router, authService: IAuthService) {
        super(shellService, dataService, router, authService);
        this.title = "Work Items";
    }

    protected getQuery() {
        return "WorkItems?$expand=WorkItemStatus";
    }

    protected getSchemaModel() {
        return {
            fields: {
                'Subject': { type: "string" },
                'WorkItemStatus.Name': { type: "string" }
            }
        };
    }

    protected itemRoute(): string {
        return "WorkItem";
    }

    protected getColumns(): any {
        return [
            { title: 'Subject', field: 'Subject' },
            { title: 'Status', field: 'WorkItemStatus.Name' }
        ];
    }

}