import {Directive, EventEmitter, AfterContentInit, ElementRef} from 'angular2/core';

@Directive({
    selector: '[richTextInput]',
    inputs: ['value'],
    outputs: ['valueChange']
})
export class RichTextInput implements AfterContentInit {
    _value: any;
    elementRef: ElementRef;

    private valueChange: EventEmitter<any> = new EventEmitter();

    constructor(elementRef: ElementRef) {
        this.elementRef = elementRef;

    }

    ngAfterContentInit() {
        var self = this;
        var element = jQuery(this.elementRef.nativeElement);
        element.kendoEditor({
            resizable: {
                content: true,
                toolbar: true
            },
            change: (e) => {
                var control = element.data("kendoEditor");
                self._value = control.value();
                self.onValueChanged();
            }
        });
    }

    get value() {
        return this._value;
    }
    set value(value) {
        this._value = value;
        var element = jQuery(this.elementRef.nativeElement);
        var control = element.data("kendoEditor");
        if (control) {
            control.value(value);
        }
        //this.onValueChanged();
    }

    onValueChanged() {
        this.valueChange.emit(this._value);
    }

}