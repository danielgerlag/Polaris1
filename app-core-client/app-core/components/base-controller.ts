import {OnInit} from 'angular2/core';
import {FormBuilder, Validators, ControlGroup, Control, NgClass} from 'angular2/common';
import {IShellService} from './services/shell-service';
import {IDataService, ValidationContext, ValidationResponse} from './services/data-service';
import {IRemoteService} from './services/remote-service';
import {IValidatable} from './interfaces';
import {ModelErrorContainer} from './interfaces';

import {LoadCollection, LoadCollectionArgs} from './annotations';


export abstract class DataComponent implements OnInit {

    protected shellService:IShellService;
    protected dataService:IDataService;
    public validationContext: ValidationContext;

    constructor(shellService:IShellService, dataService:IDataService) {
        this.shellService = shellService;
        this.dataService = dataService;
        this.validationContext = new ValidationContext();

    }

    public ngOnInit() {
        this.dataService.loadCollections(this);
    }

    public validate(validationKey: string):ValidationResponse {
        return this.dataService.validate(validationKey, this.validationContext);
    }


}

export abstract class FormController<T extends IValidatable> extends DataComponent {

    protected model: T;
    protected serverState: any;
    protected form: ControlGroup;


    constructor(shellService: IShellService, fb: FormBuilder, dataService: IDataService) {
        super(shellService, dataService);
        this.serverState = { modelState: {} };
        this.model = this.createModel();
        this.form = this.model.buildForm(fb, this.serverState, "");
        this.validationContext.form = this.form;

    }

    protected abstract createModel(): T;

    protected prepareData(): any {
        return this.model;
    }

    protected beforeSubmit() {
    }

    protected abstract doSubmit(data: any);

    public submit() {
        this.beforeSubmit();
        var data = this.prepareData();
        this.doSubmit(data);
        //this.remoteService.post(this, this.getPath(), data, this.onResponse);
    }

    protected onResponse(sender: FormController<T>, data: any, status: number): any {

        sender.validationContext.errorContainer = [];
        sender.serverState.modelState = {};

        if (status == 400) {
            sender.serverState.modelState = sender.dataService.decodeServerModelState(data);
            sender.validationContext.errorContainer = sender.serverState.modelState;

            for (var x in sender.form.controls) {
                sender.form.controls[x].updateValueAndValidity(true, true);
            }
        }
    }

}


