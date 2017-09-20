import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES, JsonPipe} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {CRUDController, ILogService, IAuthService, IShellService, IDialogService, IDataService} from 'app-core/app-core';

import {LoadCollection} from 'app-core/app-core';


export abstract class EmploymentViewModel extends CRUDController {

    @LoadCollection({ entitySet: "AccountingEntities", expand: "Party", orderBy: "Party.Name" })
    public accountingEntities: any = [];

    constructor(params: RouteParams, router: Router, location: Location, dataService: IDataService, shellService: IShellService, authService: IAuthService, fb: FormBuilder, logService: ILogService, dialogService: IDialogService) {
        super(params, router, location, dataService, shellService, authService, fb, logService, dialogService);
    }

    protected typeName(): string {
        return "Employment";
    }

    protected setName(): string {
        return "Employments";
    }

    protected title(): steing {
        return "Employment Contract";
    }

    protected expandFields(): string[] {
        var result = super.expandFields();
        result.push("Employee");
        result.push("Contract");
        result.push("Employee.Party");
        return result;
    }

    protected intialValues(): any {
        var party = this.dataService.createEntity("Party", { PartyType: "I" });

        return {
            Employee: this.dataService.createEntity("Employee", {
                Party: party
            }),
            Contract: this.dataService.createEntity("Contract", {
                Party: party
        })};
    }

    protected afterSave(sender: EmploymentViewModel, data: any) {
        super.afterSave(sender, data);
        sender.router.navigate(["Home"]);
    }
}