import {Component, OnInit} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ControlGroup} from 'angular2/common';
import {Alert} from 'ng2-bootstrap/ng2-bootstrap';
import {ValidationContext} from 'app-core/app-core';
//import {TranslateService, TranslatePipe} from '../../../node_modules/ng2-translate/ng2-translate';
@Component({
    selector: 'form-summary',
    properties: ['validationContext'],
    template: `    
    <div *ngIf="validationContext.errorContainer.length > 0">
        <alert *ngFor="#error of validationContext.errorContainer" type="danger" dismissible="true">
            <i class="fa fa-exclamation-triangle"></i>&nbsp; {{ error.message }}
        </alert>
    </div>
  `,
    directives: [CORE_DIRECTIVES, NgClass, Alert]
})
export class FormSummary implements OnInit {
    validationContext: ValidationContext;

    //private translateService: TranslateService;
    constructor() {
        //this.translateService = translateService;
    }
    onInit() {
        this.ngOnInit();
    }
    ngOnInit() {
    }
}