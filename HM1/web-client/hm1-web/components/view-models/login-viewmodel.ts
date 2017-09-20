import {Component, View} from 'angular2/core';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {Router} from 'angular2/router';
import {FormController, ILogService, IDataService, IAuthService, IShellService, IDialogService} from 'app-core/app-core';
import {FormInput} from 'app-core-web/app-core-web'
import {LoginViewModel} from 'hm1-core/hm1-core';


@Component({
    selector: 'login',
    template: `

    <form-input [entity]="model" title="User Name (Email Address)" >
        <input type="text" class="form-control" [(ngModel)]="model.UserName" />
    </form-input>

    <form-input [entity]="model" title="Password" >
        <input type="password" class="form-control" [(ngModel)]="model.Password" />
    </form-input>

    <div class="modal-footer">
        <button type="button" class="btn btn-primary" (click)="submit()">Login</button>
    </div>
    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, FormInput]
})

export class LoginViewModelWeb extends LoginViewModel {
    constructor(shellService:IShellService, authService:IAuthService, fb:FormBuilder, logService:ILogService, dialogService:IDialogService, router:Router, dataService: IDataService) {
        super(shellService, authService, fb, logService, dialogService, router, dataService);
    }

}

