import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES, JsonPipe} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {Account} from '../models'

import {FormController} from '../base-controller';
import {ILogService} from '../services/log-service';
import {IAuthService} from '../services/auth-service';
import {IDataService} from '../services/data-service';
import {IShellService} from '../services/shell-service';
import {IDialogService} from '../services/dialog-service';

export class ChangePasswordViewModel extends FormController<Account.ChangePasswordRequest> {

    protected authService:IAuthService;
    protected router: Router;

    constructor(shellService:IShellService, authService:IAuthService, fb:FormBuilder, logService:ILogService, dialogService:IDialogService, router: Router, dataService: IDataService) {
        super(shellService, fb, dataService);
        this.authService = authService;
        this.router = router;
    }

    protected createModel(): Account.ChangePasswordRequest {
        return new Account.ChangePasswordRequest();
    }

    protected doSubmit(data:any) {
        var self = this;
        this.shellService.showLoader("Changing Password...");
        this.authService.changePassword(this.model, this, this.onResponse);
    }

    protected onResponse(sender: ChangePasswordViewModel, data: any, status: number): any {
        super.onResponse(sender, data, status);
        sender.shellService.hideLoader();
        if (status == 200) {
            sender.shellService.toastSuccess("Password Changed", "");
            sender.router.navigate(["Home"]);
        }
    }


}