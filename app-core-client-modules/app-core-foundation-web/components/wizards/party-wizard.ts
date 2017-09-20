import {Component, View, OnInit, EventEmitter} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ControlGroup} from 'angular2/common';

import {TAB_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {FormInput, EntityDropdown, EntitySummary} from 'app-core-web/app-core-web';

import {IDataService, ILogService, IShellService} from 'app-core/app-core';
import {PartyWizard} from 'app-core-foundation/wizards';

import {PartyGeneralFragmentWeb} from '../fragments/party-general'

@Component({
    selector: 'partyWizard',
    inputs: ['value', 'dataService'],
    outputs: ['valueChange', 'onFinish', 'onCancel'],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, TAB_DIRECTIVES, EntityDropdown, FormInput, EntitySummary, PartyGeneralFragmentWeb],
    template: `
<div class="panel panel-default" *ngIf="step == 1">
    <div class="panel-heading">Step {{step}} - General</div>
    <div class="panel-body" *ngIf="entity">
        <partyGeneral [(value)]="entity" [dataService]="dataService"></partyGeneral>
    </div>
</div>

<div class="panel panel-default" *ngIf="step == 2">
    <div class="panel-heading">Step {{step}} - Contact Details</div>
    <div class="panel-body">
        <p>contact details...</p>
    </div>
</div>

<div class="panel panel-default" *ngIf="step == 3">
    <div class="panel-heading">Step {{step}} - Bank Accounts</div>
    <div class="panel-body">
        <p>bank details...</p>
    </div>
</div>

<div class="panel panel-default" *ngIf="step == 4">
    <div class="panel-heading">Step {{step}} - Summary</div>
    <div class="panel-body">
        Summary

        <error-summary [canShow]="showErrorSummary" [entity]="entity"></error-summary>
    </div>
</div>


<div class="modal-footer">

    <button type="button" class="btn" (click)="cancel()">Cancel</button>
    <button type="button" class="btn" (click)="prevStep()">Back</button>
    <button type="button" class="btn btn-primary" (click)="nextStep()">Next</button>

    <button type="button" class="btn btn-primary" (click)="finish()">Finish</button>
</div>
    `
})
export class PartyWizardWeb extends PartyWizard {

    constructor(shellService:IShellService, dataService:IDataService, logService: ILogService) {
        super(shellService, dataService, logService);
    }

}

