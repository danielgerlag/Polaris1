import {DecoratedModel} from 'app-core/app-core'
import {Validators} from "angular2/common";

import {CustomValidators, Validate, ServerValidate} from 'app-core/app-core';

export class RegisterRequest extends DecoratedModel {

    @ServerValidate
    Email: string;

    @ServerValidate
    CompanyName: string;

    @ServerValidate
    Password: string;

    @ServerValidate
    ConfirmPassword: string;
}

