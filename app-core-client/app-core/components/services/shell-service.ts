import {Inject, Injectable, Component} from 'angular2/core';
import {Http, Headers} from 'angular2/http'
import {RouteParams, ROUTER_DIRECTIVES, Location} from 'angular2/router';
import {IRemoteService} from './remote-service';

export class ShellInfo {
    isLoading: boolean;
    loadingMessage: string;

    constructor() {
        this.isLoading = false;
        this.loadingMessage = "";
    }

}

export abstract class IShellService {
    public info: ShellInfo;
    abstract showLoader(message);
    abstract hideLoader();
    abstract toastSuccess(title, text);
    abstract toastError(title, text);
    abstract toastInfo(title, text);
    abstract toastWarning(title, text);
}

@Injectable()
export abstract class ShellService implements IShellService {

    public info: ShellInfo;
    //translate: TranslateService;

    constructor( @Inject(Location) location: Location) {
        this.info = new ShellInfo();
    }


    showLoader(message) {
        this.info.isLoading = true;
        this.info.loadingMessage = message;
    }

    hideLoader() {
        this.info.isLoading = false;
        this.info.loadingMessage = "";
    }

    abstract toastSuccess(title, text);
    abstract toastError(title, text);
    abstract toastInfo(title, text);
    abstract toastWarning(title, text);

}
