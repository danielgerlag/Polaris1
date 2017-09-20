import {Component} from 'angular2/core';
import {Location, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {EntitySummary, EntityDropdown, FormInput, NumericInput, CRUDForm, DateInput} from 'app-core-web/app-core-web';
import {CRUDController, ILogService, IAuthService, IShellService, IDialogService, IDataService} from 'app-core/app-core';

import {ScheduledJournalSubViewWeb} from 'app-core-financial-web/view-models';

import {LoadCollection} from 'app-core/app-core';


@Component({
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, FormInput, NgClass, EntitySummary, EntityDropdown, CRUDForm, NumericInput, DateInput, ScheduledJournalSubViewWeb],
    template: `
<crudForm [title]="title()" (onSave)="save()" *ngIf="isLoaded">    
    <div class="row">
        <div class="tabbable">
            <ul class="nav nav-pills nav-stacked col-md-3">
                <li class="active"><a href="#General" data-toggle="tab">General</a></li>
                <li><a href="#Participants" data-toggle="tab">Participants</a></li>
                <li><a href="#JournalSchedule" data-toggle="tab">Journal Schedule</a></li>

            </ul>
            <div class="tab-content col-md-9">
                <div class="tab-pane active" id="General">
                    
                    <form-input [entity]="entity.Contract.Party" title="Name">
                        <input type="text" class="form-control" [(ngModel)]="entity.Contract.Party.Name">
                    </form-input>
                    
                    <form-input [entity]="entity.Contract" title="Account Number">
                        <input type="text" class="form-control" [(ngModel)]="entity.Contract.Number">
                    </form-input>
                    
                    <form-input [entity]="entity.Contract" title="Start Date">
                        <input dateInput class="form-control" [(value)]="entity.Contract.StartDate">
                    </form-input>
                    
                    <form-input [entity]="entity" title="Billing Participant">
                        <select class="form-control" [(ngModel)]="entity.BillingParticipantID">
                            <option *ngFor="#p of participants" value="{{p.ID}}">{{p.Party.FirstName}} {{p.Party.Surname}}</option>
                        </select>
                    </form-input>


                </div>

                <div class="tab-pane" id="Participants">
                    <table class="table">
                        <thead>
                            <tr>
                                <th>Participant</th>
                                <th>Percentage</th>
                                <th>
                                    <button type="button" class="btn btn-default" (click)="addParticipant()">Add</button>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr *ngFor="#item of entity.Participants">
                                <td>
                                    <select class="form-control" [(ngModel)]="item.ParticipantID">
                                        <option *ngFor="#p of participants" value="{{p.ID}}">{{p.Party.FirstName}} {{p.Party.Surname}}</option>
                                    </select>
                                </td>
                                <td>
                                    <input numericInput [(value)]="item.Percentage" format="p0" min="0" max="1" step="0.01" >                                    
                                </td>
                                <td>
                                    <button type="button" class="btn btn-default" (click)="removeParticipant(item)">Remove</button>
                                </td>
                            </tr>
                        </tbody>                    
                    </table>
                </div>

                <div class="tab-pane" id="JournalSchedule">                
                    <scheduledJournalSubView [value]="entity.Contract" [dataService]="dataService" originKey="ProviderAccount" ></scheduledJournalSubView>                
                </div>

            </div>
        </div>
    </div>

</crudForm>
    `
})
export class ProviderAccountCRUD extends CRUDController {

    @LoadCollection({ entitySet: "Participants", orderBy: "Party.FirstName", filter: null, expand: "Party" })
    public participants: any = [];

    constructor(params: RouteParams, router: Router, location: Location, dataService: IDataService, shellService: IShellService, authService: IAuthService, fb: FormBuilder, logService: ILogService, dialogService: IDialogService) {
        super(params, router, location, dataService, shellService, authService, fb, logService, dialogService);
    }

    protected intialValues(): any {
        var party = this.dataService.createEntity("Party", { PartyType: "C" });
        var contract = this.dataService.createEntity("Contract", { Party: party });
        return { Contract: contract };
    }

    protected title(): string {
        return "Provider Account - " + this.entity.Contract.Party.Name;
    }

    protected typeName(): string {
        return "ProviderAccount";
    }

    protected setName(): string {
        return "ProviderAccounts";
    }

    protected expandFields(): string[] {
        var result = super.expandFields();
        result.push("Participants");
        result.push("Contract");
        result.push("Contract.Party");
        return result;
    }

    protected afterSave(sender: ProviderAccountCRUD, data: any) {
        super.afterSave(sender, data);
        sender.router.navigate(["Home"]);
    }

    public addParticipant() {
        var item = this.dataService.createEntity("ProviderAccountParticipant", {});
        this.entity.Participants.push(item);
    }

    public removeParticipant(item) {
        this.entity.Participants.remove(item);
    }
}