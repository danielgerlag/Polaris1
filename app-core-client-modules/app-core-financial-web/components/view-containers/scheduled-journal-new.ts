import {NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {ICustomModal, ModalDialogInstance} from 'angular2-modal/angular2-modal';

import {Component} from 'angular2/core';

import {IDialogService, ILogService, IDataService, IAuthService, IShellService} from 'app-core/app-core';
import {EntitySummary, EntityDropdown, ModalWindow} from 'app-core-web/app-core-web';

//import {NewParty} from 'app-core-client-financial/core';

import {ScheduledJournalWizardWeb} from '../wizards/scheduled-journal-wizard';

@Component({
    template: `
    <div>        
        <scheduledJournalWizard [value]="entity" [dataService]="dataService" (onCancel)="cancel()" (onFinish)="ok()" [originKey]="data.originKey"></scheduledJournalWizard>
    </div>
    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ScheduledJournalWizardWeb]
})
export class NewScheduledJournal extends ModalWindow {

    constructor(dialog: ModalDialogInstance, data: ICustomModal, dataService: IDataService) {
        super(dialog, data, dataService);

    }

}