import {Input} from 'angular2/core';
import {DataComponent, IShellService, IDataService} from 'app-core/app-core';


export abstract class JournalViewModel extends DataComponent {

    @Input()
    protected entity: any;

    @Input()
    protected dataService: IDataService;

    constructor(shellService:IShellService, dataService:IDataService) {
        super(shellService, dataService);
    }


    public addTxn() {
        var item = this.dataService.createEntity("JournalTxn", {});
        this.entity.JournalTxns.push(item);
    }

}


