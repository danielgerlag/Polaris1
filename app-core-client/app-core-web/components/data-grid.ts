import {Directive, OnInit, EventEmitter, Input, Output, AfterContentInit, ElementRef} from 'angular2/core';
import {ILogService} from 'app-core/app-core';

@Directive({
    selector: '[dataGrid]'
})
export class DataGrid implements AfterContentInit {

    @Input()
    dataSource: any;

    @Input()
    columns: any;

    @Input()
    selectedItem: any;

    @Input()
    height: number;

    @Output()
    public selectedItemChange: EventEmitter<any> = new EventEmitter();

    elementRef: ElementRef;
    logService: ILogService;

    constructor(elementRef: ElementRef, logService: ILogService) {
        this.elementRef = elementRef;
        this.logService = logService;

    }

    ngAfterContentInit() {
        var self = this;
        var element = jQuery(this.elementRef.nativeElement);

        element.kendoGrid({
            dataSource: self.dataSource,
            columns: self.columns,
            height: self.height,
            //pageSize: 20,
            selectable: "row",
            change: function (e) {
                var selectedRows = this.select();
                if (selectedRows.length > 0) {
                    var dataItem = this.dataItem(selectedRows[0]);
                    self.selectedItem = dataItem;
                    self.selectedItemChange.emit(dataItem);
                }
            },
            sortable: true,
            filterable: true
        });
    }

}