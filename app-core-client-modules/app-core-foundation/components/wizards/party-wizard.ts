import {ILogService, IShellService, IDataService, WizardController} from 'app-core/app-core';

export abstract class PartyWizard extends WizardController {

    constructor(shellService:IShellService, dataService:IDataService, logService: ILogService) {
        super(shellService, dataService, logService);
    }

}

