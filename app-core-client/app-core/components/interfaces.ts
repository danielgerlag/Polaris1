import {FormBuilder, Validators, ControlGroup, Control} from 'angular2/common';

export interface IValidatable {
    buildForm(fb: FormBuilder, serverState: any, prefix: string): ControlGroup;
}

export interface ODataWrapper<T> {
    value: T;
}

export interface ODataCollectionWrapper<T> {
    value: T[];
}

export class ModelErrorContainer {
    public fieldName: string;
    public errorKey: string;
    public message: string;
}