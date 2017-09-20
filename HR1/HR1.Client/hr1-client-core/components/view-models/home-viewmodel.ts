import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES, JsonPipe} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {Component, View} from 'angular2/core';
import {ILogService, IAuthService, IShellService, IDialogService, Account} from 'app-core/app-core';


export class HomeViewModel {

    protected authService:IAuthService;
    protected logService:ILogService;
    protected shellService:IShellService;
    protected router: Router;

    constructor(shellService:IShellService, authService:IAuthService, logService:ILogService, dialogService:IDialogService, router: Router) {
        this.shellService = shellService;
        this.logService = logService;
        this.authService = authService;
        this.router = router;
    }



}