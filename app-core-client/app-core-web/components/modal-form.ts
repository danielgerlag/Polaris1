import {Component, OnInit, EventEmitter} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass} from 'angular2/common';
import {IDataService, ValidationContext, ValidationResponse} from 'app-core/app-core';
//import {}
//import {TranslateService, TranslatePipe} from '../../../node_modules/ng2-translate/ng2-translate';

@Component({
    selector: 'modalForm',
    inputs: ['title'],
    outputs: ['ok', 'cancel'],
    template: `
    <div class="modal-content">
        <div class="modal-header">
            <button type="button" class="close" (click)="cancel.emit()">&times;</button>
            <h4 class="modal-title">{{title}}</h4>
        </div>
        <div class="modal-body form-horizontal">    
            <ng-content></ng-content>    
        </div>
        <div class="modal-footer">
            <button type="button" class="btn" (click)="cancel.emit()">Cancel</button>
            <button type="button" class="btn btn-primary" (click)="ok.emit()">OK</button>
        </div>
    </div>    
  `,
    directives: [CORE_DIRECTIVES, NgClass]
})
export class ModalForm implements OnInit {

    public title: string;
    public ok: EventEmitter<any> = new EventEmitter();
    public cancel: EventEmitter<any> = new EventEmitter();

    //private translateService: TranslateService;

    constructor() {
        //this.translateService = translateService;

    }

    ngOnInit() {

    }
}

