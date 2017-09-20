import {Component, OnInit, EventEmitter, provide, ElementRef, Injector, Renderer} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ControlGroup} from 'angular2/common';

import {IDataService, IDialogService} from 'app-core/app-core';
import {CHILD_ENTITY_DIRECTIVES} from 'app-core-web/app-core-web';
import {ScheduledJournalSubView} from 'app-core-financial/view-models';

import {NewScheduledJournal} from '../view-containers/scheduled-journal-new';
//import {EditTransactionTrigger} from './editTransactionTrigger'


@Component({
    selector: 'scheduledJournalSubView',
    template: `
     <div *ngIf="isInit">                
        <childEntityList [data]="entity.ScheduledJournals"
                         [canAdd]="true"
                         [canEdit]="true"
                         [canRemove]="false"
                         (onAdd)="add($event)" 
                         (onEdit)="edit($event)">
            <columns>
                <column title="Description" field="Description"></column>                
                <column title="Next Run" field="NextExecutionDate"></column>
                <column title="Frequency" field="Frequency"></column>
            </columns>
        </childEntityList>
    </div>
    `,
    inputs: ['value', 'dataService', 'originKey'],
    outputs: ['valueChange'],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, CHILD_ENTITY_DIRECTIVES]
})
export class ScheduledJournalSubViewWeb extends ScheduledJournalSubView {

    constructor(dialogService: IDialogService) {
        super(dialogService);
    }

    protected getNewSubView():any {
        return NewScheduledJournal;
    }

    protected getEditSubView():any {
        return null;
    }


}

