import {Component} from 'angular2/core';
import {Location, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {EntitySummary, EntityDropdown, FormInput, NumericInput} from 'app-core-web/app-core-web';
import {CRUDController, ILogService, IAuthService, IShellService, IDialogService, IDataService} from 'app-core/app-core';
import {JournalWizard} from './journal-wizard';

import {LoadCollection} from 'app-core/app-core';


@Component({
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, FormInput, NgClass, EntitySummary, EntityDropdown, NumericInput, JournalWizard],
    template: `
    <journalWizard [value]="entity" [dataService]="dataService" (onFinish)="save()">    
    </journalWizard>
    `
})
export class JournalNew extends CRUDController {


    constructor(params: RouteParams, router: Router, location: Location, dataService: IDataService, shellService: IShellService, authService: IAuthService, fb: FormBuilder, logService: ILogService, dialogService: IDialogService) {
        super(params, router, location, dataService, shellService, authService, fb, logService, dialogService);
    }

    protected intialValues(): any {
        return { };
    }

    protected title(): string {
        return "Journal";
    }

    protected typeName(): string {
        return "ScheduledJournal";
    }

    protected setName(): string {
        return "ScheduledJournals";
    }

    protected expandFields(): string[] {
        var result = super.expandFields();
        return result;
    }

    protected afterSave(sender: JournalNew, data: any) {
        super.afterSave(sender, data);
        sender.router.navigate(["Home"]);
    }


}