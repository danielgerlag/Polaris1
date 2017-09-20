import {Component, OnInit} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass} from 'angular2/common';
import {IDataService, ValidationContext, ValidationResponse} from 'app-core/app-core';
//import {}
//import {TranslateService, TranslatePipe} from '../../../node_modules/ng2-translate/ng2-translate';



@Component({
    selector: 'form-input',
    properties: ['title', 'validationKey', 'entity', 'validationContext', 'dataService', 'labelClass'],
    template: `
    <div class="form-group" [class.has-error]="hasError()">
        <label class="control-label {{labelClass}}" >{{ title }}</label>
        <ng-content></ng-content>
        <span class="help-block">
            {{helpMessage}}
      </span>
    </div>
  `,
    directives: [CORE_DIRECTIVES, NgClass]
})
export class FormInput implements OnInit {

    private title: string;
    private validationKey: string;
    private validationContext: ValidationContext;
    private labelClass: string;
    private entity: breeze.Entity;
    private dataService: IDataService;
    private helpMessage: string;

    //private translateService: TranslateService;

    constructor() {
        //this.translateService = translateService;
    }

    ngOnInit() {
        //this.type = this.type !== 'undefined' ? this.type : 'tabs';
    }

    onInit() {
        this.ngOnInit();
    }
    hasError() {
        if (this.dataService) {
            if (this.validationContext && this.validationKey) {
                var result = this.dataService.validate(this.validationKey, this.validationContext);
                this.helpMessage = result.message;
                return (!result.valid);
            }
        }

        return false;
        //if (this.entity) {
        //    if (this.entity.entityAspect) {
        //        var errors = this.entity.entityAspect.getValidationErrors(this.name);
        //        return (errors.length > 0);
        //    }
        //}
        //if (this.form) {
        //    return !this.form.find(this.name).valid;// && this.form.find(this.name).touched;
        //}
        //
        //return false;
    }

}

