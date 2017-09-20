import {Component} from 'angular2/core';
import {Location, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {EntitySummary, EntityDropdown, FormInput, CRUDForm} from 'app-core-web/app-core-web';
import {CRUDController, ILogService, IAuthService, IShellService, IDialogService, IDataService} from 'app-core/app-core';

import {LedgerAccountChildView} from './ledger-accounts-childview';
import {AccountingEntityLedgerBalances} from '../view-models/accounting-entity-ledger-balances';

import {LoadCollection} from 'app-core/app-core';


@Component({
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, FormInput, NgClass, EntitySummary, EntityDropdown, CRUDForm, LedgerAccountChildView, AccountingEntityLedgerBalances],
    template: `
<crudForm [title]="title()" (onSave)="save()">    
    <div class="row">
        <div class="tabbable">
            <ul class="nav nav-pills nav-stacked col-md-3">
                <li class="active"><a href="#General" data-toggle="tab">General</a></li>
                <li><a href="#LedgerAccounts" data-toggle="tab">LedgerAccounts</a></li>

            </ul>
            <div class="tab-content col-md-9">
                <div class="tab-pane active" id="General">


                    <form-input [entity]="entity.Party" title="Name">
                        <input type="text" class="form-control" [(ngModel)]="entity.Party.Name">
                    </form-input>


                </div>

                <div class="tab-pane" id="LedgerAccounts">
                    <ledgerAccountChildView [value]="entity" [dataService]="dataService"></ledgerAccountChildView>
                </div>
                
                
                <div class="tab-pane" id="LedgerBalances">
                    <accountingEntityLedgerBalances [accountingEntityID]="entity.ID" [ledgerID]="1" [effectiveDate]="Date()" ></accountingEntityLedgerBalances>
                </div>
                
                

            </div>
        </div>
    </div>

</crudForm>
    `
})
export class AccountingEntityCRUD extends CRUDController {


    constructor(params: RouteParams, router: Router, location: Location, dataService: IDataService, shellService: IShellService, authService: IAuthService, fb: FormBuilder, logService: ILogService, dialogService: IDialogService) {
        super(params, router, location, dataService, shellService, authService, fb, logService, dialogService);
    }

    protected intialValues(): any {
        var party = this.dataService.createEntity("Party", { PartyType: "C" });
        return { Party: party };
    }

    protected title(): string {
        return "Accounting Entity" + this.entity.Party.Name;
    }

    protected typeName(): string {
        return "AccountingEntity";
    }

    protected setName(): string {
        return "AccountingEntities";
    }

    protected expandFields(): string[] {
        var result = super.expandFields();
        result.push("Party");
        return result;
    }

    protected afterSave(sender: AccountingEntityCRUD, data: any) {
        super.afterSave(sender, data);
        sender.router.navigate(["Home"]);
    }
}