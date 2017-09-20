import {Component} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass} from 'angular2/common';
import {IDataService, IDialogService, SubViewList} from 'app-core/app-core';
import {CHILD_ENTITY_DIRECTIVES} from 'app-core-web/app-core-web';
//import {NewTransactionTrigger} from './newTransactionTrigger'
//import {EditTransactionTrigger} from './editTransactionTrigger'


@Component({
    selector: 'ledgerAccountChildView',
    template: `
     <div *ngIf="isInit">                
        <childEntityList [data]="entity.LedgerAccounts"
                         [canAdd]="true"
                         [canEdit]="true"
                         [canRemove]="false"
                         (onAdd)="add($event)" 
                         (onEdit)="edit($event)">
            <columns>
                <column title="Name" field="Name"></column>
            </columns>
        </childEntityList>
    </div>
    `,
    inputs: ['value', 'dataService'],
    outputs: ['valueChange'],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, CHILD_ENTITY_DIRECTIVES]
})
export class LedgerAccountChildView extends SubViewList {


    constructor(dialogService: IDialogService) {
        super(dialogService);
    }


    protected createChildEntity(type): any {
        var item = this.dataService.createEntity("LedgerAccount", {});
        //item.AccountingEntityID = this.entity.AccountingEntityID;
        return item;
    }

    protected getNewSubView():any {
        return null;
    }

    protected getEditSubView():any {
        return null;
    }

    protected onAdd(item) {
        this.entity['LedgerAccounts'].push(item);
    }

    protected onEntityChanged() {
        super.onEntityChanged();
        this.loadNavigationGraph("LedgerAccounts", "");
    }



}

