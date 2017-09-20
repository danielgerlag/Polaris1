

import {FormBuilder, Validators, ControlGroup, Control, NgClass} from 'angular2/common';
import {IValidatable} from './interfaces';
import {CustomValidators, Validate, ServerValidate, ChildForm, AddToForm} from './validators';
//import 'reflect-metadata/Reflect'



export abstract class DecoratedModel implements IValidatable {
    buildForm(fb: FormBuilder, serverState: any, prefix: string): ControlGroup {
        var self = this;
        var result = fb.group({});
        for (var p in self) {
            var add = Reflect.getMetadata("custom:addToForm", self, p);
            if (add) {
                var validators: Function[] = [];
                var sv = Reflect.getMetadata("custom:serverValidate", self, p);
                if (sv)
                    validators.push(CustomValidators.serverValidate(serverState, prefix + p));
                var cv = Reflect.getMetadata("custom:validator", self, p);
                if (cv)
                    validators = validators.concat(cv);
                result.addControl(p, new Control(self[p], Validators.compose(validators)));
            }
            var child = Reflect.getMetadata("custom:childForm", self, p);
            if (child) {
                var childForm = self[p].buildForm(fb, serverState, prefix + p + ".");
                result.addControl(p, childForm);
            }
        }
        return result;
    }
}

export namespace Account {

    export class LoginRequest extends DecoratedModel {

        @ServerValidate
        @Validate(Validators.required)
        Username: string;

        @Validate(Validators.required)
        Password: string;
    }

    export class ChangePasswordRequest extends DecoratedModel  {
        OldPassword: string;
        NewPassword: string;
        ConfirmPassword: string;
    }


    export class LoginResponse  {
        UserName: boolean;
        PasswordExpired: boolean;
        Tenants:  TenantInfo[];
    }

    export class TenantInfo {
        ID : number;
        Name: string;
    }

}

export class SearchRequest {
    SearchQuery: string;
    SearchType: string;
}

export class SearchType {
    constructor(Key:string, Name:string) {
        this.Key = Key;
        this.Name = Name;
    }
    Key: string;
    Name: string;
}

export class SuggestionResponse {
    Suggestions: string[];
}

export class SearchResponse {
    Results: SearchResponseLine[] = [];
}

export class SearchResponseLine {
    ID: number;
    EntityType: string;
    Reference: string;
    Name: string;
    Number: string;
    Summary: string;

}

export interface IAppConfig {
    api: string;
}