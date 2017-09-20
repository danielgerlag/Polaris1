import {Component, Input} from 'angular2/core';
import {NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {EntitySummary, FormInput, NumericInput, DateInput} from 'app-core-web/app-core-web';
import {ILogService, IDataService, IShellService, WizardController} from 'app-core/app-core';
import {LoadCollection} from 'app-core/app-core';

import {UserInputValues} from 'app-core-foundation-web/fragments';

@Component({
    selector: 'journalWizard',
    inputs: ['value', 'dataService', 'originKey'],
    outputs: ['valueChange', 'onFinish', 'onCancel'],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, FormInput, EntitySummary, DateInput, UserInputValues],
    template: `
<div class="panel panel-default" *ngIf="step == 1">
    <div class="panel-heading">Template</div>
    <div class="panel-body" *ngIf="entity">
        
        <form-input [entity]="entity" title="Template">
            <select class="form-control" [(ngModel)]="entity.JournalTemplateID">
                <option *ngFor="#item of templates" value="{{ item.ID }}">{{ item.Name }}</option>
            </select>
        </form-input>
        
        
    </div>
</div>

<div class="panel panel-default" *ngIf="step == 2">
    <div class="panel-heading">Inputs</div>
    <div class="panel-body">    
        <userInputValues [value]="entity" 
                         [dataService]="dataService" 
                         configFKProperty="JournalTemplateInputID"
                         inputType="ScheduledJournalInputValue"
                         configSet="JournalTemplates" 
                         configID="{{entity.JournalTemplateID}}" ></userInputValues>    
    </div>
</div>

<div class="panel panel-default" *ngIf="step == 3">
    <div class="panel-heading">Schedule</div>
    <div class="panel-body">
        
        <form-input [entity]="entity" title="Description">
            <input type="text" class="form-control" [(ngModel)]="entity.Description" />                
        </form-input>
        
        <form-input [entity]="entity" title="Journal Date">
            <input dateInput [(value)]="entity.TxnDate" />
        </form-input>
        
        <form-input [entity]="entity" title="Next Run Date">
            <input dateInput [(value)]="entity.NextExecutionDate" />
        </form-input>
        
        <form-input [entity]="entity" title="Frequency">
            <select class="form-control" [(ngModel)]="entity.Frequency">
                <option value="O">Once off</option>
                <option value="W">Weekly</option>
                <option value="B">Bi-weekly</option>
                <option value="M">Monthly</option>
                <option value="Q">Quarterly</option>
                <option value="A">Annually</option>
            </select>
        </form-input>
        
    </div>
</div>

<div class="panel panel-default" *ngIf="step == 4">
    <div class="panel-heading">Step {{step}} - Summary</div>
    <div class="panel-body">
        Summary
        
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
export class JournalWizard extends WizardController {

    protected originKey: string;

    @LoadCollection({ entitySet: "JournalTemplates", orderBy: "Name", filter: null, expand: null })
    public templates: any = [];


    constructor(shellService:IShellService, dataService:IDataService, logService: ILogService) {
        super(shellService, dataService, logService);
    }

    protected ngOnInit() {
        super.ngOnInit();
    }

}

