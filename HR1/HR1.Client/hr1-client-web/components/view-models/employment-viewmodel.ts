import {Router, RouteParams, Location} from 'angular2/router';
import {Component} from 'angular2/core';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';

import {TAB_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';

import {ILogService, IAuthService, IShellService, IDialogService, IDataService} from 'app-core/app-core';
import {EntitySummary, EntityDropdown, FormInput, CRUDForm} from 'app-core-web/app-core-web';

import {ScheduledJournalSubViewWeb} from 'app-core-financial-web/view-models';

import {EmploymentViewModel} from 'hr1-client-core/view-models';

@Component({
    templateUrl: './templates/employment-edit.html',
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, FormInput, NgClass, EntitySummary, EntityDropdown, TAB_DIRECTIVES, CRUDForm, ScheduledJournalSubViewWeb]
})
export class EmploymentViewModelWeb extends EmploymentViewModel {

    constructor(params: RouteParams, router: Router, location: Location, dataService: IDataService, shellService: IShellService, authService: IAuthService, fb: FormBuilder, logService: ILogService, dialogService: IDialogService) {
        super(params, router, location, dataService, shellService, authService, fb, logService, dialogService);

    }


}