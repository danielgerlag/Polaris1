import {Component, OnInit, EventEmitter, Directive} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass} from 'angular2/common';
import {IDataService, ValidationContext, ValidationResponse} from 'app-core/app-core';

@Component({
    selector: 'childEntityList',
    inputs: ['data', 'canAdd', 'canEdit', 'canRemove'],
    outputs: ['onAdd', 'onEdit', 'onRemove'],
    template: `
    <table class="table">
        <tr>
            <th *ngFor="#col of columns">{{ col.title }}</th>
            <th>
                <button *ngIf="canAdd" type="button" class="btn" (click)="onAdd.emit(null)">Add</button>
            </th>
        </tr>
        <tr *ngFor="#item of data">
            <td *ngFor="#col of columns">
                {{ item[col.field] }}
            </td>                
            <td>
                <button *ngIf="canEdit" type="button" class="btn" (click)="onEdit.emit(item)"><i class="fa fa-edit fa-fw"></i></button>
                <button *ngIf="canRemove" type="button" class="btn" (click)="onRemove.emit(item)"><i class="fa fa-trash fa-fw"></i></button>
            </td>
        </tr>
    </table>      
      
  `,
    directives: [CORE_DIRECTIVES, NgClass]
})
export class ChildEntityList implements OnInit {

    public columns: ChildEntityListColumn[] = [];
    public data: any;

    public canAdd: boolean;
    public canEdit: boolean;
    public canRemove: boolean;

    public onAdd: EventEmitter<any> = new EventEmitter();
    public onEdit: EventEmitter<any> = new EventEmitter();
    public onRemove: EventEmitter<any> = new EventEmitter();

    constructor() {

    }

    ngOnInit() {

    }

    addColumn(col : ChildEntityListColumn) {
        this.columns.push(col);
    }
}


@Directive({
    selector: 'column',
    inputs: ['title', 'field']
})
export class ChildEntityListColumn implements OnInit {

    public title: string;
    public field: string;

    constructor(parent: ChildEntityList) {
        parent.addColumn(this);
    }

    ngOnInit() {

    }
}

export const CHILD_ENTITY_DIRECTIVES: any[] = [ChildEntityList, ChildEntityListColumn];