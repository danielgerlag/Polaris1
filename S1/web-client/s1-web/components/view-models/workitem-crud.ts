import {Component} from 'angular2/core';
import {Location, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {EntitySummary, EntityDropdown, FormInput, CRUDForm, RichTextInput} from 'app-core-web/app-core-web';
import {CRUDController, ILogService, IAuthService, IShellService, IDialogService, IDataService} from 'app-core/app-core';

//import {LedgerAccountChildView} from './ledger-accounts-childview';

import {LoadCollection} from 'app-core/app-core';


@Component({
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, FormInput, NgClass, EntitySummary, EntityDropdown, CRUDForm, RichTextInput],
    template: `
<crudForm [title]="title()" (onSave)="save()">    
    <div class="row">
        <div class="tabbable">
            <ul class="nav nav-pills nav-stacked col-md-3">
                <li class="active"><a href="#General" data-toggle="tab">General</a></li>
                <li><a href="#Attachments" data-toggle="tab">Attachments</a></li>

            </ul>
            <div class="tab-content col-md-9">
                <div class="tab-pane active" id="General">


                    <form-input [entity]="entity.Subject" title="Subject">
                        <input type="text" class="form-control" [(ngModel)]="entity.Subject">
                    </form-input>
                    
                    <form-input [entity]="entity.Description" title="Description">
                        <textarea richTextInput [(value)]="entity.Description" rows="10" cols="30" style="height:440px">
                        </textarea>
                    </form-input>


                </div>

                <div class="tab-pane" id="Attachments">
                    
                </div>

            </div>
        </div>
    </div>

</crudForm>
    `
})
export class WorkItemCRUD extends CRUDController {


    constructor(params: RouteParams, router: Router, location: Location, dataService: IDataService, shellService: IShellService, authService: IAuthService, fb: FormBuilder, logService: ILogService, dialogService: IDialogService) {
        super(params, router, location, dataService, shellService, authService, fb, logService, dialogService);
    }

    protected intialValues(): any {
        return { };
    }

    protected title(): string {
        return "Work Item" + this.entity.Subject;
    }

    protected typeName(): string {
        return "WorkItem";
    }

    protected setName(): string {
        return "WorkItems";
    }

    protected expandFields(): string[] {
        var result = super.expandFields();
        //result.push("Party");
        return result;
    }

    protected afterSave(sender: WorkItemCRUD, data: any) {
        super.afterSave(sender, data);
        sender.router.navigate(["Home"]);
    }
}