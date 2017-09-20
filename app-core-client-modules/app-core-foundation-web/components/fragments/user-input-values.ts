import {Component, OnInit, EventEmitter, Input} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ControlGroup} from 'angular2/common';


import {EntityDropdown, FormInput, NumericInput} from 'app-core-web/app-core-web';

import {IDataService, IShellService, LoadCollection, DataComponent} from 'app-core/app-core';


@Component({
    selector: 'userInputValues',
    inputs: ['value', 'dataService', 'configID', 'configSet', 'configFKProperty', 'inputType'],
    outputs: ['valueChange'],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, EntityDropdown, FormInput, NumericInput],
    template: `    
    <div class="form-horizontal" *ngIf="ready">
        <form-input *ngFor="#templateInput of configEntity.UserInputs"
                    [entity]="findInput(templateInput.ID)"
                    title="{{ templateInput.Name }}"
                    labelClass="col-sm-2"
                    [ngSwitch]="templateInput.UserInputType.Code">
            <div class="col-sm-10">
                <input *ngSwitchWhen="'TEXT'" [(ngModel)]="findInput(templateInput.ID).Value" type="text" class="form-control">
                <input *ngSwitchWhen="'INT'" [(ngModel)]="findInput(templateInput.ID).Value" type="number" class="form-control">
                <input *ngSwitchWhen="'CURRENCY'" [(ngModel)]="findInput(templateInput.ID).Value" numericInput >
                <input *ngSwitchWhen="'PCT'" [(ngModel)]="findInput(templateInput.ID).Value" numericInput >
                <input *ngSwitchWhen="'DATE'" [(ngModel)]="findInput(templateInput.ID).Value" type="date" class="form-control">
                <!--
                <entity-dropdown *ngSwitchWhen="'LIST'"
                                 [(value)]="findInput(templateInput.ID).Value"
                                 query="AttributeLookupItems?$filter=AttributeLookupListID eq {{templateInput.AttributeLookupListID}}&$orderby=Description"
                                 keyField="Code"
                                 displayField="Description"
                                 nullable="true">
                </entity-dropdown>    
                -->               
                <p *ngSwitchWhen="'YEAR'">year...</p>
                <input *ngSwitchWhen="'BOOL'" [(ngModel)]="findInput(templateInput.ID).Value" type="checkbox" class="form-control">
                
            </div>
        </form-input>
    </div> 
    `
})
export class UserInputValues extends DataComponent {

    private dataService: IDataService;
    private entity: breeze.Entity;

    public configID: number;
    public configSet: string;

    public configFKProperty: string;
    public inputType: string;

    private configEntity: any;

    public ready: boolean;


    public valueChange: EventEmitter<any> = new EventEmitter();

    constructor(shellService:IShellService, dataService:IDataService) {
        super(shellService, dataService);
        this.ready = false;
    }

    ngOnInit() {
        super.ngOnInit();
        this.dataService.getEntity(this, this.configSet, this.configID, "UserInputs, UserInputs.UserInputType", false, this.onConfigRecieved, this.onConfigFail);
    }

    onConfigRecieved(sender: UserInputValues, result: breeze.QueryResult) {
        sender.configEntity = result.results[0];
        sender.ready = true;
    }

    onConfigFail(sender: UserInputValues, reason: any) {

    }

    public get value() {
        return this.entity;
    }
    public set value(value) {
        this.entity = value;
        this.onEntityChanged();
    }

    onEntityChanged() {
        this.valueChange.emit(this.entity);
    }

    findInput(userInputID) {
        var subject: any = this.entity;
        for (let inp of subject.UserInputValues) {
            if (inp[this.configFKProperty] == userInputID) {
                return inp;
            }
        }

        var newInp = this.dataService.createEntity(this.inputType, {});
        newInp[this.configFKProperty] = userInputID;
        subject.UserInputValues.push(newInp);
        return newInp;
    }

}

