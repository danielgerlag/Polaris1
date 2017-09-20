import {bootstrap} from 'angular2/platform/browser'
import {enableProdMode, Inject, Component, provide, Renderer} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {FormBuilder, Validators, ControlGroup } from 'angular2/common';
import {ROUTER_DIRECTIVES, RouteConfig, Location,ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {Modal, ModalConfig} from 'angular2-modal/angular2-modal';


import {IConfigService, ConfigService} from 'app-core/app-core';
import {HTTP_PROVIDERS} from 'app-core/app-core';
import {TransientInfo} from 'app-core/app-core';
import {IShellService, ShellInfo} from 'app-core/app-core';
import {IRemoteService, RemoteService, ISearchService} from 'app-core/app-core';
import {IDialogService, IDataService, IAuthService, ILogService, LogService} from 'app-core/app-core';
import {WebDialogService, WebShellService} from 'app-core-web/app-core-web'
import {NewPartyWeb} from 'app-core-foundation-web/core'
import {AccountingEntityListWeb, AccountingEntityCRUD} from 'app-core-financial-web/setup';
import {S1DataService, S1AuthService, S1SearchService} from 's1-core/s1-core'

import
{
    HomeViewModelWeb,
    RegisterViewModelWeb,
    LoginViewModelWeb,
    WorkItemListWeb,
    WorkItemCRUD
} from './view-models/view-models';


enableProdMode();

@Component(
    {
        selector: 'app-shell',
        templateUrl: './templates/shell.html',
        directives: [ROUTER_DIRECTIVES, CORE_DIRECTIVES, FORM_DIRECTIVES],
        pipes: []
    })

@RouteConfig([
    new Route({ path: '/', component: HomeViewModelWeb, name: 'Home' }),
    new Route({ path: '/Register', component: RegisterViewModelWeb, name: 'Register' }),
    new Route({ path: '/Login', component: LoginViewModelWeb, name: 'Login' }),

    new Route({ path: '/WorkItems', component: WorkItemListWeb, name: 'WorkItems' }),
    new Route({ path: '/WorkItem', component: WorkItemCRUD, name: 'WorkItem' }),
    new Route({ path: '/WorkItem/:Id', component: WorkItemCRUD, name: 'WorkItem' }),

    //new Route({ path: '/AccountingEntities', component: AccountingEntityListWeb, name: 'AccountingEntities' }),

    new Route({ path: '/AccountingEntity', component: AccountingEntityCRUD, name: 'AccountingEntity' }),
    new Route({ path: '/AccountingEntity/:Id', component: AccountingEntityCRUD, name: 'AccountingEntity' }),

    new Route({ path: '/PartyWizard', component: NewPartyWeb, name: 'PartyWizard' })

    //new Route({ path: '/NewPolicy', component: Components.NewPolicy, name: 'NewPolicy' }),
    //new Route({ path: '/EditPolicy/:Id', component: Components.EditPolicy, name: 'EditPolicy' }),
    //new Route({ path: '/Login', component: LoginController, name: 'Login' })
])

class App {

    router: Router;
    location: Location;

    userInfo: TransientInfo;
    shellInfo: ShellInfo;

    authService: IAuthService;
    shellService: IShellService;
    ds: IDataService;

    constructor(
        @Inject(Router) router: Router,
        @Inject(Location) location: Location,
        @Inject(IAuthService) authService: IAuthService,
        @Inject(IShellService) shellService: IShellService,
        @Inject(IDataService) dataService: IDataService

    ){

        this.router = router;
        this.location = location;
        this.authService = authService;
        this.shellService = shellService;
        this.ds = dataService;
        this.userInfo = authService.userInfo;
        this.shellInfo = shellService.info;

        shellService.showLoader("Resuming session...");
        authService.resume(this, () => {
            shellService.hideLoader();
        });
    }


    private onLogoutResponse(sender: App, data: any, status: number): any {
        sender.router.navigate(["Home"]);
        sender.shellService.toastInfo("LOGOUT_TITLE", "LOGOUT_TEXT");
    }

    public getLinkStyle(path) {
        if (path === this.location.path()) {
            return true;
        }
        else if (path.length > 0) {
            return this.location.path().indexOf(path) > -1;
        }
    }

    public signOut() {
        this.authService.logout(this, this.onLogoutResponse);
    }


}


bootstrap(App, [
    HTTP_PROVIDERS,
    ROUTER_PROVIDERS,
    FormBuilder,
    Modal,
    Renderer,
    provide(IConfigService, { useClass: ConfigService }),
    provide(IRemoteService, { useClass: RemoteService }),
    provide(IDataService, { useClass: S1DataService }),
    provide(IAuthService, { useClass: S1AuthService }),
    provide(IDialogService, { useClass: WebDialogService }),
    provide(IShellService, { useClass: WebShellService }),
    provide(ISearchService, { useClass: S1SearchService }),
    provide(ILogService, { useClass: LogService }),
    provide(LocationStrategy, { useClass: HashLocationStrategy }),
    provide(ModalConfig, { useValue: new ModalConfig('lg', true, 81) })
]);
