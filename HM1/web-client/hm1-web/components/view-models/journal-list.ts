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
export class JournalListWeb extends WebListController {

    constructor(shellService: IShellService, dataService: IDataService, router: Router, authService: IAuthService) {
        super(shellService, dataService, router, authService);
        this.title = "Journals";
    }

    protected getQuery() {
        return "Journals?$expand=JournalType";
    }

    protected getSchemaModel() {
        return {
            fields: {
                'JournalType.Name': { type: "string" },
                'Description': { type: "string" },
                'Reference': { type: "string" },
                'TxnDate': { type: "date" },
                'Amount': { type: "number" }
            }
        };
    }

    protected itemRoute(): string {
        return "Journal";
    }

    protected getColumns(): any {
        return [
            { title: 'Date', field: 'TxnDate' },
            { title: 'Description', field: 'Description' },
            { title: 'Reference', field: 'Reference' },
            { title: 'Type', field: 'JournalType.Name' },
            { title: 'Amount', field: 'Amount' }
        ];
    }

}