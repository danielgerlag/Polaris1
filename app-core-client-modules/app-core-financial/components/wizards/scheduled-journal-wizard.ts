import {Input} from 'angular2/core';
import {ILogService, IDataService, IShellService, WizardController} from 'app-core/app-core';
import {LoadCollection} from 'app-core/app-core';


export abstract class ScheduledJournalWizard extends WizardController {

    protected originKey: string;
    protected templateFilter: any[];

    @LoadCollection({ entitySet: "JournalTemplates", orderBy: "Name", filter: "templateFilter", expand: null })
    public templates: any = [];


    constructor(shellService:IShellService, dataService:IDataService, logService: ILogService) {
        super(shellService, dataService, logService);
    }

    protected ngOnInit() {
        this.templateFilter = [
            {property: "OriginKey", operator: "==", value: this.originKey},
            {property: "AccountingEntityID", operator: "==", value: this.entity.AccountingEntityID}
        ];
        super.ngOnInit();
    }

}

