import {Router} from 'angular2/router';
import {IShellService} from './services/shell-service';
import {IDataService} from './services/data-service';
import {IAuthService} from './services/auth-service';
import {DataComponent} from './base-controller'

export abstract class ListController extends DataComponent {

    protected router: Router;

    public dataSource: any;
    public columns: any;

    public selectedItem: any;
    protected title: string;
    protected authService: IAuthService;

    constructor(shellService: IShellService, dataService: IDataService, router: Router, authService: IAuthService) {
        super(shellService, dataService);
        this.dataService = dataService;
        this.router = router;
        this.authService = authService;
        this.columns = this.getColumns();
        this.dataSource = this.getDataSource();
    }

    protected abstract getDataSource(): any;
    protected abstract getColumns(): any;
    protected abstract itemRoute(): string;
    protected abstract getQuery(): any;

    protected expandFields(): string[] {
        return [];
    }


    protected openItem() {
        this.router.navigateByUrl(this.itemRoute() + "/" + this.selectedItem.ID);
    }

    protected addItem() {
        this.router.navigateByUrl(this.itemRoute());
    }

    protected deleteItem() {
        //todo
    }

    protected canAdd() {
        return true;
    }

    protected canOpen() {
        if (this.selectedItem)
            return true;
        else
            return false;
    }

    protected canDelete() {
        if (this.selectedItem)
            return true;
        else
            return false;
    }

    protected drillObject(data: any, path: string): any {
        var delimeter = path.indexOf(".");
        if (delimeter == -1) {
            return data[path];
        }
        else {
            var newPath = path.slice(delimeter + 1);
            var newData = data[path.slice(0, delimeter)];
            return this.drillObject(newData, newPath);
        }
    }
}