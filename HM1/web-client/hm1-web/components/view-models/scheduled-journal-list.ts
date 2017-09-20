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
export class ScheduledJournalListWeb extends WebListController {

    constructor(shellService: IShellService, dataService: IDataService, router: Router, authService: IAuthService) {
        super(shellService, dataService, router, authService);
        this.title = "Scheduled Journals";
    }

    protected getQuery() {
        return "ScheduledJournals?$expand=JournalTemplate";
    }

    protected getSchemaModel() {
        return {
            fields: {
                'JournalTemplate.Name': { type: "string" },
                'Description': { type: "string" },
                'Frequency': { type: "string" },
                'TxnDate': { type: "date" }
            }
        };
    }

    protected itemRoute(): string {
        return "ScheduledJournal";
    }

    protected getColumns(): any {
        return [
            { title: 'Date', field: 'TxnDate' },
            { title: 'Type', field: 'JournalTemplate.Name' },
            { title: 'Frequency', field: 'Frequency' },
            { title: 'Description', field: 'Description' }
        ];
    }

}