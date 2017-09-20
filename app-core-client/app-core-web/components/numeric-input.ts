import {Directive, View, OnInit, EventEmitter, Input, Output, AfterContentInit, ElementRef} from 'angular2/core';

@Directive({
    selector: '[numericInput]',
    inputs: ['value', 'format', 'min' , 'max', 'step'],
    outputs: ['valueChange']
})
export class NumericInput implements AfterContentInit {
    _value: any;
    elementRef: ElementRef;

    format: string;
    decimals: number;
    min: number;
    max: number;
    step: number;

    private valueChange: EventEmitter<any> = new EventEmitter();

    constructor(elementRef: ElementRef) {
        this.elementRef = elementRef;

    }

    ngAfterContentInit() {
        var self = this;
        var element = jQuery(this.elementRef.nativeElement);
        element.kendoNumericTextBox({
            format: self.format,
            decimals: self.decimals,
            min: self.min,
            max: self.max,
            step: self.step,
            change: (e: kendo.ui.NumericTextBoxChangeEvent) => {
                var control = element.data("kendoNumericTextBox");
                self._value = control.value();
                this.onValueChanged();
            }
        });
        var control = element.data("kendoNumericTextBox");
        control.value(self._value);
    }

    get value() {
        return this._value;
    }
    set value(value) {
        this._value = value;
        var element = jQuery(this.elementRef.nativeElement);
        var control = element.data("kendoNumericTextBox");
        if (control) {
            control.value(value);
        }
    }

    onValueChanged() {
        this.valueChange.emit(this._value);
    }

}