import {OnInit, EventEmitter} from 'angular2/core';
import {ICustomModal, ModalDialogInstance, ICustomModalComponent} from 'angular2-modal/angular2-modal';
import {IDataService} from 'app-core/app-core';

export abstract class ModalWindow implements OnInit, ICustomModalComponent {

    dataService: IDataService;
    data: any;
    entity: any;
    dialog: ModalDialogInstance;

    ngOnInit() {

    }

    constructor(dialog: ModalDialogInstance, data: ICustomModal, dataService: IDataService) {
        this.dialog = dialog;
        if (data) {
            this.data = data;
            if (data.entity) {
                this.entity = data.entity;
            }
            else {
                this.entity = data;
            }
        }
        this.dataService = dataService;
        dataService.loadCollections(this);
    }


    beforeDismiss(): boolean {
        return true;
    }

    beforeClose(): boolean {
        return false;
    }

    protected ok() {
        this.dialog.close(this.entity);
    }

    protected cancel() {
        this.dialog.close(null);
    }

}

