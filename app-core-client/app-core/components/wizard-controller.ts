import {EventEmitter} from 'angular2/core';
import {DataComponent} from './base-controller';
import {IDataService} from './services/data-service';
import {IShellService} from './services/shell-service';
import {ILogService} from './services/log-service';

export abstract class WizardController extends DataComponent {
    protected logService: ILogService;
    protected entity: any;
    public valueChange: EventEmitter<any> = new EventEmitter();
    public onFinish: EventEmitter<any> = new EventEmitter();
    public onCancel: EventEmitter<any> = new EventEmitter();

    protected step: number;

    constructor (shellService:IShellService, dataService:IDataService, logService: ILogService) {
        super(shellService, dataService);
        this.logService = logService;
        this.step = 1;
    }
    public get value() {
        return this.entity;
    }
    public set value(value) {
        this.entity = value;
        this.onDataChanged();
    }

    onDataChanged() {
        this.valueChange.emit(this.entity);
    }

    protected nextStep() {
        this.step++;
    }

    protected prevStep() {
        this.step--;
    }

    protected finish() {
        this.onFinish.emit(this.entity);
    }

    protected cancel() {
        this.onCancel.emit(null);
    }

}

