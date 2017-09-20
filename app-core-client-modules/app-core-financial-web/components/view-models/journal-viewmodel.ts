import {Input, Component} from 'angular2/core';
import {IShellService, IDataService} from 'app-core/app-core';
import {JournalViewModel} from 'app-core-financial/view-models';


@Component({
    selector: 'journal',
    template: ''
})
export class JournalViewModelWeb extends JournalViewModel {

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


