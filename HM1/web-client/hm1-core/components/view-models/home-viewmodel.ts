import {Router} from 'angular2/router';
import {ILogService, IAuthService, IShellService, IDialogService, Account} from 'app-core/app-core';

export class HomeViewModel {

    protected authService:IAuthService;
    protected logService:ILogService;
    protected shellService:IShellService;
    protected router: Router;

    constructor(shellService:IShellService, authService:IAuthService, logService:ILogService, dialogService:IDialogService, router: Router) {
        this.shellService = shellService;
        this.logService = logService;
        this.authService = authService;
        this.router = router;
    }



}