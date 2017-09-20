import {Component} from 'angular2/core';
import {Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES, JsonPipe} from 'angular2/common';
import {FormInput} from '../form-input';
import {FormSummary} from '../form-summary';
import {ChangePasswordViewModel, ILogService, IAuthService, IDataService, IShellService, IDialogService} from 'app-core/app-core';

@Component({
    template: `   

    <div [ngFormModel]="form">

        <form-summary [validationContext]="validationContext"></form-summary>

        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="OldPassword" title="Old Password" >
            <input type="password" class="form-control" [(ngModel)]="model.OldPassword"/>
        </form-input>

        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="NewPassword" title="New Password" >
            <input type="password" class="form-control" [(ngModel)]="model.NewPassword"/>
        </form-input>

        <form-input [dataService]="dataService" [validationContext]="validationContext" validationKey="ConfirmPassword" title="Confirm Password" >
            <input type="password" class="form-control" [(ngModel)]="model.ConfirmPassword"/>
        </form-input>

        <div class="modal-footer">
            <button type="button" class="btn btn-primary" (click)="submit()">Change</button>
        </div>

    </div>
    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, FormInput, FormSummary]
})
export class ChangePasswordViewModelWeb extends ChangePasswordViewModel {

    constructor(shellService:IShellService, authService:IAuthService, fb:FormBuilder, logService:ILogService, dialogService:IDialogService, router: Router, dataService: IDataService) {
        super(shellService, authService, fb, logService, dialogService, router, dataService);

    }

}