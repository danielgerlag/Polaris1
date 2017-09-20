import {Component, OnInit, EventEmitter} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass} from 'angular2/common';
import {IDataService, ValidationContext, ValidationResponse} from 'app-core/app-core';
//import {}
//import {TranslateService, TranslatePipe} from '../../../node_modules/ng2-translate/ng2-translate';

@Component({
    selector: 'crudForm',
    inputs: ['title'],
    outputs: ['onSave'],
    template: `    
    
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">{{title}}</h3>
            
        </div>
        <div class="panel-body">
            <ng-content></ng-content>
            
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" (click)="onSave.emit()">
                    Save
                </button>
            </div>
        </div>         
    </div>
    
  `,
    directives: [CORE_DIRECTIVES, NgClass]
})
export class CRUDForm implements OnInit {

    public title: string;
    public onSave: EventEmitter<any> = new EventEmitter();

    //private translateService: TranslateService;

    constructor() {
        //this.translateService = translateService;

    }

    ngOnInit() {

    }
}

