import {Component, OnInit, EventEmitter} from 'angular2/core';
import {ODataWrapper} from './interfaces';
import {IDataService} from './services/data-service';
import {IDialogService} from './services/dialog-service';


export abstract class SubViewList implements OnInit {

    protected isInit: boolean;
    protected dataService: IDataService;
    protected  dialogService: IDialogService;
    public entity: breeze.Entity;
    public valueChange: EventEmitter<any> = new EventEmitter();


    constructor(dialogService: IDialogService) {
        this.isInit = false;
        this.dialogService = dialogService;
    }

    protected abstract getNewSubView(): any;
    protected abstract getEditSubView(): any;
    protected abstract createChildEntity(type): any;
    protected abstract onAdd(item);

    ngOnInit() {
        this.isInit = true;
        this.onEntityChanged();
    }

    onInit() {
        this.ngOnInit();
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


    protected add(type) {
        var item = { entity: this.createChildEntity(type) };
        this.beforeDialog(item);
        let component = this.getNewSubView();
        var self = this;

        this.dialogService.showDialog(this, component, item, this.dataService, (sender, result) => {
            if (result)
                self.onAdd(result);
            else
                item.entity.entityAspect.setDetached();
        }, (sender) => {
            item.entity.entityAspect.setDetached();
        });
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

    protected edit(item: breeze.Entity) {
        var data = { entity: item };
        this.beforeDialog(data);
        let component = this.getEditSubView();
        var self = this;

        this.dialogService.showDialog(this, component, data, this.dataService, (sender, result) => {
            //
        }, (sender) => {
            //
        });
    }

    protected remove(item: breeze.Entity) {
        item.entityAspect.setDeleted();
    }

    protected beforeDialog(data: any) {
    }

}

