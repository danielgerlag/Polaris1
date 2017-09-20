import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {RouteParams} from 'angular2/router';

import {Component, View} from 'angular2/core';

import {IDialogService, ILogService, IDataService, IAuthService, IShellService} from 'app-core/app-core';
import {EntitySummary, EntityDropdown, FormInput} from 'app-core-web/app-core-web';

import {NewParty} from 'app-core-foundation/core';

import {PartyWizardWeb} from './wizards/party-wizard';

@Component({
    template: `
    <div *ngIf="isLoaded">
        <partyWizard [(value)]="entity" [dataService]="dataService" (onFinish)="save()"></partyWizard>
    </div>
    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, FormInput, NgClass, EntitySummary, EntityDropdown, PartyWizardWeb]
})
export class NewPartyWeb extends NewParty {

    constructor(params: RouteParams, router: Router, location: Location, dataService: IDataService, shellService: IShellService, authService: IAuthService, fb: FormBuilder, logService: ILogService, dialogService: IDialogService) {
        super(params, router, location, dataService, shellService, authService, fb, logService, dialogService);

    }

}