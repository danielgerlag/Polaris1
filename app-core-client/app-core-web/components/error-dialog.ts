import {Component, OnInit, EventEmitter} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ControlGroup} from 'angular2/common';

import {ModalDialogInstance} from 'angular2-modal/angular2-modal';
import {ICustomModal, ICustomModalComponent} from 'angular2-modal/angular2-modal';


@Component({
    selector: 'errorDialog',
    template: `
    <div class="modal-content">
        <div class="modal-header alert-danger">
            <button type="button" class="close" (click)="cancel()">&times;</button>
            <h4 class="modal-title">Error</h4>
        </div>
        <div class="modal-body form-horizontal">
            {{ context.message }}
        </div>
        <div class="modal-footer">
            <button type="button" class="btn" (click)="cancel()">Cancel</button>
            <button type="button" class="btn btn-primary" (click)="ok()">OK</button>
        </div>
    </div>
    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass]
})
export class ErrorDialog implements OnInit, ICustomModalComponent {
    context: any;
    dialog: ModalDialogInstance;

    ngOnInit() {
    }

    onInit() {
        this.ngOnInit();
    }

    constructor(dialog: ModalDialogInstance, data: ICustomModal) {
        this.dialog = dialog;
        this.context = data;
    }


    beforeDismiss(): boolean {
        return true;
    }

    beforeClose(): boolean {
        return false;
    }

    protected ok() {
        this.dialog.close(this.context);
    }

    protected cancel() {
        this.dialog.close(null);
    }


}

