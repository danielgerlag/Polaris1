import {Component, OnInit, EventEmitter} from 'angular2/core';
import {CORE_DIRECTIVES, NgClass} from 'angular2/common';

@Component({
    selector: 'listHeader',
    inputs: ['title', 'canAdd', 'canOpen', 'canDelete'],
    outputs: ['add', 'open', 'delete'],
    template: `    
    
    <div class="panel panel-default">
        <div class="panel-heading">
            <h3 class="panel-title">{{title}}</h3>
            
        </div>
        <div class="panel-body">
            <ng-content></ng-content>
            
            <div class="modal-footer">
                <button type="button" class="btn btn-default" (click)="add.emit(null)" [disabled]="!canAdd">
                    Add
                </button>
                <button type="button" class="btn btn-primary" (click)="open.emit(null)" [disabled]="!canOpen">
                    Edit
                </button>
                <button type="button" class="btn btn-danger" (click)="delete.emit(null)" [disabled]="!canDelete">
                    Delete
                </button>
            </div>
        </div>         
    </div>
    
  `,
    directives: [CORE_DIRECTIVES, NgClass]
})
export class ListHeader implements OnInit {

    public title: string;
    public add: EventEmitter<any> = new EventEmitter();
    public open: EventEmitter<any> = new EventEmitter();
    public delete: EventEmitter<any> = new EventEmitter();

    public canAdd: boolean;
    public canOpen: boolean;
    public canDelete: boolean;

    constructor() {
    }

    ngOnInit() {

    }
}

