import {Component} from 'angular2/core';
import {Validators, ControlGroup, Control, NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {Router} from 'angular2/router';
import {Typeahead} from 'ng2-bootstrap/ng2-bootstrap';
import {FormController, ILogService, IDataService, IAuthService, IShellService, IDialogService} from 'app-core/app-core';
import {FormInput, SearchControllerWeb} from 'app-core-web/app-core-web'
import {HomeViewModel} from 'hm1-core/hm1-core';


@Component({
    selector: 'home',
    template: `

        <p>you are home</p>

        <search></search>
    `,
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, FormInput, Typeahead, SearchControllerWeb]
})
export class HomeViewModelWeb extends HomeViewModel {
    constructor(shellService:IShellService, authService:IAuthService, logService:ILogService, dialogService:IDialogService, router:Router) {
        super(shellService, authService,logService, dialogService, router);
    }

}

