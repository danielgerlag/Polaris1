import {ROUTER_DIRECTIVES, RouteConfig, Location, ROUTER_PROVIDERS, LocationStrategy, HashLocationStrategy, Route, AsyncRoute, Router} from 'angular2/router';
import {FormBuilder, Validators, ControlGroup, Control, NgClass} from 'angular2/common';
import {RouteParams} from 'angular2/router';
import {IShellService} from './services/shell-service';
import {IAuthService} from './services/auth-service';
import {IDataService} from './services/data-service';
import {ILogService} from './services/log-service';
import {IDialogService} from './services/dialog-service';
import {DataComponent} from './base-controller';
import {ModelErrorContainer} from './interfaces';

export abstract class CRUDController extends DataComponent {


    protected entity: any;
    protected serverState: any;
    protected form: ControlGroup;
    protected showErrorSummary: boolean;
    //protected dataService: IDataService;
    protected logService: ILogService;
    protected params: RouteParams;
    protected router: Router;
    protected location: Location;
    protected authService: IAuthService;
    protected isLoaded: boolean;

    constructor(
        params: RouteParams,
        router: Router,
        location: Location,
        dataService: IDataService,
        shellService: IShellService,
        authService: IAuthService,
        fb: FormBuilder,
        logService: ILogService,
        protected dialogService: IDialogService
    ) {

        super(shellService, dataService);
        this.isLoaded = false;
        dataService.reset();
        //this.dataService = dataService;
        this.logService = logService;
        this.showErrorSummary = false;
        this.serverState = { modelState: {} };
        this.params = params;
        this.location = location;
        this.router = router;
        this.authService = authService;
        this.entity = this.dataService.createEntity(this.typeName(), this.intialValues());

        //this.form = this.entity.buildForm(fb, this.serverState, "");
        this.form = fb.group({});

        this.init();
    }


    protected abstract typeName(): string;
    protected abstract setName(): string;

    protected intialValues(): any {
        return {};
    }

    protected title(): string {
        return "";
    }


    preInit(): Promise<{}> {
        var self = this;
        var promise = new Promise(function (resolve, reject) {
            try {
                resolve();
            }
            catch (e) {
                reject(e)
            }
        });
        return promise;
    }

    init() {
        var self = this;

        this.preInit().then(function () {
            var id: number = parseInt(self.params.get('Id'));
            if (id > 0) {
                self.load(id);
                self.onLoading(id);
            }
            else {
                self.isLoaded = true;
            }
        });
    }



    protected prepareData(): any {
        return this.entity;
    }

    protected beforeSave() {
    }


    protected expandFields(): string[] {
        return [];
    }

    protected afterSave(sender: CRUDController, data: any) {
        this.shellService.toastSuccess("Saved", "Data successfully saved");
    }

    protected onLoading(id: number) {
    }

    protected save() {
        this.beforeSave();
        this.dataService.saveChanges(this, this.onSaveSuccess, this.onSaveFailure);
    };


    protected onSaveFailure(sender: CRUDController, data: any): any {
        sender.shellService.hideLoader();
        sender.showErrorSummary = true;

        if (data.message) {
            //sender.shellService.toastError("Error", data.message);
            sender.dialogService.showErrorDialog({ message: data.message });
        }
    }

    protected onSaveSuccess(sender: CRUDController, data: breeze.SaveResult): any {
        sender.shellService.hideLoader();
        sender.afterSave(sender, data);
    }

    private recurseUpdateControls(sender, group) {
        if (group.controls) {
            for (var x in group.controls) {
                group.controls[x].updateValueAndValidity();
                sender.recurseUpdateControls(sender, group.controls[x]);
            }
        }
    }

    protected load(id) {
        var expandList = this.expandFields();
        var expandQuery = null;

        if (expandList.length > 0) {
            expandQuery = "";
            var first = true;
            expandList.forEach(function (navPath) {
                if (!first)
                    expandQuery = expandQuery + ",";
                expandQuery = expandQuery + navPath;
                first = false;
            });
        }

        this.shellService.showLoader("Loading...");
        this.dataService.getEntity(this, this.setName(), id, expandQuery, true, this.onLoadSuccess, this.onLoadFailure);
    }

    protected onLoadSuccess(sender: CRUDController, data: breeze.QueryResult): any {
        sender.shellService.hideLoader();
        sender.entity = data.results[0];
        sender.isLoaded = true;
    }

    protected onLoadFailure(sender: CRUDController, data: any): any {
        sender.shellService.hideLoader();
        sender.dialogService.showErrorDialog({ message: data });
    }


    protected loadNavigationGraph(entity: breeze.Entity, navProperty, navExpand) {
        if (entity.entityAspect.entityState.isUnchangedOrModified()) {
            if (!entity.entityAspect.isNavigationPropertyLoaded(navProperty)) {
                var np = entity.entityType.getNavigationProperty(navProperty);
                this.dataService.loadNavigationGraph(this, entity, np, navExpand, this.navFail);
            }
        }
    }

    protected navFail(sender, data) {
        alert(data);
    }
}




