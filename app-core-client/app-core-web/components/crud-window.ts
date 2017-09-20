import {OnInit, EventEmitter} from 'angular2/core';
import {ICustomModal, ModalDialogInstance, ICustomModalComponent} from 'angular2-modal/angular2-modal';
import {IDataService} from 'app-core/app-core';

export abstract class CRUDWindow implements OnInit, ICustomModalComponent {

    dataService: IDataService;
    entity: any;
    dialog: ModalDialogInstance;

    ngOnInit() {

    }

    constructor(dialog: ModalDialogInstance, data: ICustomModal, dataService: IDataService) {
        this.dialog = dialog;

        if (data) {

        }
        else {

        }

        this.dataService = dataService;
        dataService.loadCollections(this);
    }


    protected abstract typeName(): string;
    protected abstract setName(): string;

    protected intialValues(): any {
        return {};
    }

    protected title(): string {
        return "";
    }


    protected prepareData(): any {
        return this.entity;
    }

    protected beforeSave() {
    }


    protected expandFields(): string[] {
        return [];
    }

    protected onLoading(id: number) {
    }

    protected save() {
        this.beforeSave();
        this.dataService.saveChanges(this, this.onSaveSuccess, this.onSaveFailure);
    };


    protected onSaveFailure(sender: CRUDWindow, data: any): any {
        sender.shellService.hideLoader();
        sender.showErrorSummary = true;

        if (data.message) {
            //sender.shellService.toastError("Error", data.message);
            sender.dialogService.showErrorDialog({ message: data.message });
        }
    }

    protected onSaveSuccess(sender: CRUDWindow, data: breeze.SaveResult): any {
        sender.shellService.hideLoader();
        this.dialog.close(this.entity);
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

    protected onLoadSuccess(sender: CRUDWindow, data: breeze.QueryResult): any {
        sender.shellService.hideLoader();
        sender.entity = data.results[0];
        sender.isLoaded = true;
    }

    protected onLoadFailure(sender: CRUDWindow, data: any): any {
        sender.shellService.hideLoader();
        sender.dialogService.showErrorDialog({ message: data });
    }


    beforeDismiss(): boolean {
        return true;
    }

    beforeClose(): boolean {
        return false;
    }

    //protected ok() {
    //    this.dialog.close(this.entity);
    //}

    protected cancel() {
        this.dialog.close(null);
    }

}

