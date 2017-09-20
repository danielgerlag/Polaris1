import {Inject, Injectable, Component} from 'angular2/core';
import {Http, Headers} from 'angular2/http'
import {RouteParams, ROUTER_DIRECTIVES, Location} from 'angular2/router';
import {ShellInfo, IShellService, ShellService} from 'app-core/app-core';


@Injectable()
export class WebShellService extends ShellService {

    toastSvc: Toastr;
    toastLocation = 'toast-top-center';

    constructor( @Inject(Location) location: Location) {
        super(location);
        this.toastSvc = toastr;
    }


    showLoader(message) {
        this.info.isLoading = true;
        this.info.loadingMessage = message;
    }

    hideLoader() {
        this.info.isLoading = false;
        this.info.loadingMessage = "";
    }

    toastSuccess(title, text) {
        var self = this;
        self.toastSvc.success(text, title, {
            closeButton: true,
            timeOut: 3000,
            positionClass: self.toastLocation
        });
    }

    toastError(title, text) {
        var self = this;
        self.toastSvc.error(text, title, {
            closeButton: true,
            timeOut: 3000,
            positionClass: self.toastLocation
        });
    }

    toastInfo(title, text) {
        var self = this;
        self.toastSvc.info(text, title, {
            closeButton: true,
            timeOut: 3000,
            positionClass: self.toastLocation
        });
    }

    toastWarning(title, text) {
        var self = this;
        self.toastSvc.warning(text, title, {
            closeButton: true,
            timeOut: 3000,
            positionClass: self.toastLocation
        });
    }

}
