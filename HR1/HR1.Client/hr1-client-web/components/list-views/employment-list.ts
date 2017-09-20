import {NgClass, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
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
export class EmploymentListWeb extends WebListController {

    constructor(shellService: IShellService, dataService: IDataService, router: Router, authService: IAuthService) {
        super(shellService, dataService, router, authService);
        this.title = "Employment Contracts";
    }

    protected getQuery() {
        return "Employments?$expand=Employee,Employee/Party";
    }

    protected getSchemaModel() {
        return {
            fields: {
                'Employee.EmployeeNumber': { type: "string" },
                'Employee.Party.FirstName': { type: "string" },
                'Employee.Party.Surname': { type: "string" }
            }
        };
    }

    protected itemRoute(): string {
        return "Employment";
    }

    protected getColumns(): any {
        return [
            { title: 'First Name', field: 'Employee.Party.FirstName' },
            { title: 'Surname', field: 'Employee.Party.Surname' },
            { title: 'Employee Number', field: 'Employee.EmployeeNumber' }
        ];
    }

}