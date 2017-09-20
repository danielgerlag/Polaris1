import {Component} from 'angular2/core';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {Router} from 'angular2/router';
import {FormController, IDataService, ILogService, IDataService, IAuthService, IShellService, IDialogService} from 'app-core/app-core';
import {FormInput, FormSummary} from 'app-core-web/app-core-web'
import {RegisterViewModel} from 'hm1-core/hm1-core';


@Component({
    selector: 'register',
    template: `

    <div [ngFormModel]="form">

        <form-summary [validationContext]="validationContext"></form-summary>

        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="FirstName" title="First Name" >
            <input type="text" class="form-control" [(ngModel)]="model.FirstName" />
        </form-input>
        
        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="Surname" title="Surname" >
            <input type="text" class="form-control" [(ngModel)]="model.Surname" />
        </form-input>

        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="PropertyName" title="Property Name" >
            <input type="text" class="form-control" [(ngModel)]="model.PropertyName" />
        </form-input>

        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="Email" title="Email Address" >
            <input type="text" class="form-control" [(ngModel)]="model.Email" />
        </form-input>

        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="Password" title="Password" >
            <input type="password" class="form-control" [(ngModel)]="model.Password"/>
        </form-input>

        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="ConfirmPassword" title="Confirm Password" >
            <input type="password" class="form-control" [(ngModel)]="model.ConfirmPassword"/>
        </form-input>

        <div class="modal-footer">
            <button type="button" class="btn btn-primary" (click)="submit()">Register</button>
        </div>

    </div>
    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, FormInput, FormSummary]
})
export class RegisterViewModelWeb extends RegisterViewModel {
    constructor(shellService:IShellService, authService:IAuthService, fb:FormBuilder, logService:ILogService, dialogService:IDialogService, router:Router, dataService: IDataService) {
        super(shellService, authService, fb, logService, dialogService, router, dataService);
    }
}

