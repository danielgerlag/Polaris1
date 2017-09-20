import {FormBuilder, Validators, ControlGroup, Control} from 'angular2/common';

export class CustomValidators {

    static serverValidate(serverState: any, name: string): Function {
        return (control: Control): { [key: string]: any } => {

            for (var p in serverState.modelState) {
                var p2 = serverState.modelState[p].fieldName.replace("model.", "");
                //p2 = p2.replace("Party.", "");
                console.log("v-" + p2);

                if (p2 == name) {
                    var error = serverState.modelState[p].message;
                    //delete serverState.modelState[p];
                    return { "server": { message: error, value: control.value } };
                }
            }
            return null;
        };
    }
}

export function Validate(validator: Function): PropertyDecorator {
    return (target, key) => {
        target[key] = null;
        Reflect.defineMetadata("custom:addToForm", true, target, key);
        var existing = Reflect.getMetadata("custom:validator", target, key);
        if (existing)
            Reflect.defineMetadata("custom:validator", existing.concat(validator), target, key);
        else
            Reflect.defineMetadata("custom:validator", [validator], target, key);
    }
}


export function ServerValidate(target: any, key: string) {
    target[key] = null;
    Reflect.defineMetadata("custom:serverValidate", true, target, key);
    Reflect.defineMetadata("custom:addToForm", true, target, key);
}

export function AddToForm(target: any, key: string) {
    target[key] = null;
    Reflect.defineMetadata("custom:addToForm", true, target, key);
}

export function ChildForm(target: any, key: string) {
    Reflect.defineMetadata("custom:childForm", true, target, key);
}
