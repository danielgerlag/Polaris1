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
export class ProviderAccountListWeb extends WebListController {

    constructor(shellService: IShellService, dataService: IDataService, router: Router, authService: IAuthService) {
        super(shellService, dataService, router, authService);
        this.title = "Provider Accounts";
    }

    protected getQuery() {
        return "ProviderAccounts?$expand=Contract,Contract/Party";
    }

    protected getSchemaModel() {
        return {
            fields: {
                'Contract.Party.Name': { type: "string" },
                'Contract.Number': { type: "string" }
            }
        };
    }

    protected itemRoute(): string {
        return "ProviderAccount";
    }

    protected getColumns(): any {
        return [
            { title: 'Company Name', field: 'Contract.Party.Name' },
            { title: 'Account Number', field: 'Contract.Number' }
        ];
    }

}