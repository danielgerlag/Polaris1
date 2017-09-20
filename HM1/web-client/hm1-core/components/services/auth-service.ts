import {Inject, Injectable} from 'angular2/core';
import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {Subscription} from 'rxjs/Rx';
import {IRemoteService, TransientInfo, AuthService, IAuthService, Account} from 'app-core/app-core';
import {RegisterRequest} from '../binding-models';



@Injectable()
export class HM1AuthService extends AuthService {

    constructor(
        @Inject(Router) router: Router,
        @Inject(Location) location: Location,
        @Inject(IRemoteService) remoteService: IRemoteService) {
        super(router, location, remoteService);
    }

    public populateUserInfo(sender:IAuthService, data:Account.LoginResponse) {
        sender.userInfo.isLoggedIn = true;
        sender.userInfo.Tenants = data.Tenants;
        if (data.Tenants.length > 0)
            sender.userInfo.activeTenant = data.Tenants[0];
    }

    public clearUserInfo(sender:IAuthService) {
        sender.userInfo.isLoggedIn = false;
        sender.userInfo.Tenants = [];
        sender.userInfo.activeTenant = null;
    }

}