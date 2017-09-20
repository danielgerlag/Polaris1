
/*
export function ServerValidate(target: any, key: string) {
    target[key] = null;
    Reflect.defineMetadata("custom:serverValidate", true, target, key);
    Reflect.defineMetadata("custom:addToForm", true, target, key);
}
*/

export class LoadCollectionArgs {
    public entitySet: string;
    public expand: string = null;
    public orderBy: string = null;
    public filter: string = null;
}

export function LoadCollection(args: LoadCollectionArgs): PropertyDecorator {
    return (target, key) => {
        target[key] = null;
        Reflect.defineMetadata("ac:loadCollection", args, target, key);

    }
}