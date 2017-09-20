import {IDataService, IDialogService, SubViewList} from 'app-core/app-core';

//import {NewTransactionTrigger} from './newTransactionTrigger'
//import {EditTransactionTrigger} from './editTransactionTrigger'


export abstract class ScheduledJournalSubView extends SubViewList {

    public originKey: string;

    constructor(dialogService: IDialogService) {
        super(dialogService);
    }


    protected createChildEntity(type): any {
        var item = this.dataService.createEntity("ScheduledJournal", {});
        item.AccountingEntityID = this.entity.AccountingEntityID;
        return item;
    }

    protected onAdd(item) {
        this.entity['ScheduledJournals'].push(item);
    }

    protected onEntityChanged() {
        super.onEntityChanged();
        this.loadNavigationGraph("ScheduledJournals", "");
    }


    protected beforeDialog(data: any) {
        super.beforeDialog(data);
        data.originKey = this.originKey;
    }

}

