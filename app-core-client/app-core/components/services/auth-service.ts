import {Inject, Injectable} from 'angular2/core';
import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {Subscription} from 'rxjs/Rx';
import {IRemoteService} from './remote-service';
import {Account} from  '../models'

export class TransientInfo {
    isLoggedIn: boolean;
    fullName: string;
    userName: string;
    Tenants:  Account.TenantInfo[];
    activeTenant: Account.TenantInfo;

    constructor() {
        this.isLoggedIn = false;
        this.fullName = "";
    }
}

export abstract class IAuthService {
    public userInfo: TransientInfo;
    abstract login(model: any, sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any>;
    abstract logout(sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any>;
    abstract resume(sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any>;
    abstract register(model: any, sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any>;
    abstract changePassword(model: any, sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any>;
}

@Injectable()
export abstract class AuthService implements IAuthService {

    public userInfo: TransientInfo;
    protected remoteService: IRemoteService;

    constructor(
        @Inject(Router) router: Router,
        @Inject(Location) location: Location,
        @Inject(IRemoteService) remoteService: IRemoteService) {

        this.remoteService = remoteService;
        this.userInfo = new TransientInfo();
        //this.resume(this, null);
    }

    protected abstract populateUserInfo(sender: IAuthService, data: any);
    protected abstract clearUserInfo(sender: IAuthService);

    public login(model: any, sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any> {
        var self = this;
        var result = this.remoteService.post(this, 'Account/Login', model, (sender1: IAuthService, data: any, status: number) => {
            if (status == 200) {
                self.populateUserInfo(self, data);
                self.remoteService.userInfo = self.userInfo;
            }
            if (callback)
                callback(sender, data, status);
        });
        return result;
    }

    public logout(sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any> {
        var self = this;
        var result = this.remoteService.post(this, 'Account/Logout', null, (sender1: any, data: any, status: number) => {
            if (status == 200) {
                self.clearUserInfo(self);
                self.remoteService.userInfo = self.userInfo;
            }
            if (callback)
                callback(sender, data, status);
        });
        return result;
    }

    public resume(sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any> {
        var self = this;
        var result = this.remoteService.post(this, 'Account/Resume', null, (sender1: any, data: any, status: number) => {
            if (status == 200) {
                self.populateUserInfo(self, data);
                self.remoteService.userInfo = self.userInfo;
            }
            if (status == 401) {
                self.clearUserInfo(self);
                self.remoteService.userInfo = self.userInfo;
            }
            if (callback)
                callback(sender, data, status);
        });
        return result;
    }

    public register(model: any, sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any> {
        var self = this;
        var result = this.remoteService.post(this, 'Account/Register', model, (sender1: any, data: any, status: number) => {
            if (callback)
                callback(sender, data, status);
        });
        return result;
    }

    public changePassword(model: any, sender: any, callback: (sender: any, data: any, status: number) => any): Subscription<any> {
        var self = this;
        var result = this.remoteService.post(this, 'Account/ChangePassword', model, (sender1: any, data: any, status: number) => {
            if (callback)
                callback(sender, data, status);
        });
        return result;
    }


}