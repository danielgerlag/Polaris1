import {OnInit, EventEmitter} from 'angular2/core';
import {IDataService} from './services/data-service';
import {ILogService} from './services/log-service';

export abstract class DetailFragment implements OnInit {

    protected dataService: IDataService;
    protected entity: any;
    protected logService: ILogService;
    public valueChange: EventEmitter<any> = new EventEmitter();

    constructor(logService: ILogService) {
        this.logService = logService;
    }

    ngOnInit() {
        if (this.dataService) {
            this.dataService.loadCollections(this);
        }
    }

    onInit() {
        this.ngOnInit();
    }

    public get value() {
        return this.entity;
    }
    public set value(value) {
        this.entity = value;
        this.onEntityChanged();
    }

    public changeValue(value) {
        this.value = value;
    }

    onEntityChanged() {
        this.valueChange.emit(this.entity);
    }

}

