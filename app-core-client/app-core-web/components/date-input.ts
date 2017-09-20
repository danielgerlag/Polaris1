import {Directive, View, OnInit, EventEmitter, Input, Output, AfterContentInit, ElementRef} from 'angular2/core';

@Directive({
    selector: '[dateInput]',
    inputs: ['value'],
    outputs: ['valueChange']
})
export class DateInput implements AfterContentInit {
    _value: any;
    elementRef: ElementRef;

    private valueChange: EventEmitter<any> = new EventEmitter();

    constructor(elementRef: ElementRef) {
        this.elementRef = elementRef;

    }

    ngAfterContentInit() {
        var self = this;
        var element = jQuery(this.elementRef.nativeElement);
        element.kendoDatePicker({
            value: self._value,
            format: 'yyyy-MM-dd',
            change: (e: kendo.ui.DatePickerChangeEvent) => {
                var datepicker = element.data("kendoDatePicker");
                self.value = datepicker.value();
            }
        });
        //element.text(self.formatDate(self.value));
    }

    get value() {
        return this._value;
    }
    set value(value) {
        this._value = value;
        var element = jQuery(this.elementRef.nativeElement);
        var datepicker = element.data("kendoDatePicker");
        if (datepicker) {
            datepicker.value(value);
            //element.text(this.formatDate(value));
        }
        this.onValueChanged();
    }

    onValueChanged() {
        this.valueChange.emit(this._value);
    }

    formatDate(date: Date) {
        if (!date)
            return null;
        return date.getFullYear() + "-" + date.getMonth() + 1 + "-" + date.getDate();
    }
}