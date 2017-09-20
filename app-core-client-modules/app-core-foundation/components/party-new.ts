import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {RouteParams} from 'angular2/router';

import {Component, View} from 'angular2/core';

import {CRUDController, IDialogService, ILogService, IDataService, IAuthService, IShellService} from 'app-core/app-core';

export class NewParty extends CRUDController {

    constructor(params: RouteParams, router: Router, location: Location, dataService: IDataService, shellService: IShellService, authService: IAuthService, fb: FormBuilder, logService: ILogService, dialogService: IDialogService) {
        super(params, router, location, dataService, shellService, authService, fb, logService, dialogService);
        this.title = "Party";
    }

    protected typeName(): string {
        return "Party";
    }

    protected setName(): string {
        return "Parties";
    }

    protected afterSave(sender: NewParty, data: any) {
        super.afterSave(sender, data);        
        sender.router.navigate(["Home"]);
    }


}
