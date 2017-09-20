import {OnInit, EventEmitter} from 'angular2/core';
import {IDataService} from './services/data-service';
import {IDialogService} from './services/dialog-service';


export abstract class SubView implements OnInit {

    protected isInit: boolean;
    protected dataService: IDataService;
    protected  dialogService: IDialogService;
    public entity: breeze.Entity;
    public valueChange: EventEmitter<any> = new EventEmitter();


    constructor(dialogService: IDialogService) {
        this.isInit = false;
        this.dialogService = dialogService;
    }

    ngOnInit() {
        this.isInit = true;
        this.onEntityChanged();
    }


    public get value() {
        return this.entity;
    }
    public set value(value) {
        this.entity = value;
        if (this.isInit)
            this.onEntityChanged();
    }


    protected onEntityChanged() {

    }


    protected loadNavigationGraph(navProperty, navExpand) {
        if (this.entity.entityAspect.entityState.isUnchangedOrModified()) {
            if (!this.entity.entityAspect.isNavigationPropertyLoaded(navProperty)) {
                var np = this.entity.entityType.getNavigationProperty(navProperty);
                this.dataService.loadNavigationGraph(this, this.entity, np, navExpand, this.navFail);
            }
        }
    }

    protected navFail(sender, data) {
        alert(data);
    }

}

