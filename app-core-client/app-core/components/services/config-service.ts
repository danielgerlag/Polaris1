import {Inject, Injectable, Component} from 'angular2/core';

export abstract class IConfigService {
    abstract getSettings(): any;
    
}

@Injectable()
export class ConfigService implements IConfigService {
    
    settings: any;

    constructor() {
        var windowUrl = window.location.href;
        var baseUrl = windowUrl.split('#')[0];
        if (baseUrl.substr(-1) != '/') baseUrl += '/';;
        this.settings = { api: baseUrl + "api/" };
    }


    getSettings(): any {
        return this.settings;
    }


}
