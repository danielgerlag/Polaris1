import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES, JsonPipe} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {Component, View} from 'angular2/core';
import {FormController, ILogService, IDataService, IAuthService, IShellService, IDialogService, Account} from 'app-core/app-core';


export class LoginViewModel extends FormController<Account.LoginRequest> {

    protected authService:IAuthService;
    protected router: Router;

    constructor(shellService:IShellService, authService:IAuthService, fb:FormBuilder, logService:ILogService, dialogService:IDialogService, router: Router, dataService: IDataService) {
        super(shellService, fb, dataService);
        this.authService = authService;
        this.router = router;
    }

    protected createModel():Account.LoginRequest {
        return new Account.LoginRequest();
    }

    protected doSubmit(data:any) {
        var self = this;
        this.shellService.showLoader("Authenticating...");
        this.authService.login(this.model, this, (sender: LoginViewModel, data, status) => {
            sender.shellService.hideLoader();
            if (status == 200) {
                sender.router.navigate(["Home"]);
            }
        });
    }


}