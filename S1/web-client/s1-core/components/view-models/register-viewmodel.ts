import {Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES, JsonPipe} from 'angular2/common';
import {RegisterRequest} from '../binding-models'
import {FormController, ILogService, IDataService, IAuthService, IShellService, IDialogService, Account} from 'app-core/app-core';

export class RegisterViewModel extends FormController<RegisterRequest> {

    protected authService:IAuthService;
    protected router: Router;

    constructor(shellService:IShellService, authService:IAuthService, fb:FormBuilder, logService:ILogService, dialogService:IDialogService, router: Router, dataService: IDataService) {
        super(shellService, fb, dataService);
        this.authService = authService;
        this.router = router;
    }

    protected createModel():RegisterRequest {
        return new RegisterRequest();
    }

    protected doSubmit(data:any) {
        var self = this;
        this.shellService.showLoader("Registering...");
        this.authService.register(this.model, this, this.onResponse);
    }

    protected onResponse(sender: RegisterViewModel, data: any, status: number): any {
        super.onResponse(sender, data, status);
        sender.shellService.hideLoader();
        if (status == 200) {
            sender.shellService.toastSuccess("Register", "Registration successful");
            sender.login(sender.model.Email, sender.model.Password);
        }
    }


    protected login(username, password) {
        var loginReq: Account.LoginRequest = new Account.LoginRequest();
        loginReq.Username = username;
        loginReq.Password = password;
        this.shellService.showLoader("Authenticating...");
        this.authService.login(loginReq, this, (sender: RegisterViewModel, data, status) => {
            sender.shellService.hideLoader();
            if (status == 200) {
                sender.router.navigate(["Home"]);

            }
        });
    }

}