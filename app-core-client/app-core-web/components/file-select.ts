import {
    Component, View, OnInit, OnDestroy, OnChanges,
    Directive, EventEmitter, ElementRef, Renderer
} from 'angular2/core';

import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass} from 'angular2/common'

// todo: filters

@Directive({
    selector: '[ng2-file-select]',
    events: ['selectFile'],
    host: {
        '(change)': 'onChange()'
    }
})
export class FileSelect {
    private selectFile: EventEmitter<any> = new EventEmitter();

    constructor(private element: ElementRef) {
    }

    public getOptions() {
        //return this.uploader.options;
    }

    public getFilters() {
    }

    public isEmptyAfterSelection(): boolean {
        return !!this.element.nativeElement.attributes.multiple;
    }

    onChange() {
        // let files = this.uploader.isHTML5 ? this.element.nativeElement[0].files : this.element.nativeElement[0];
        let files = this.element.nativeElement.files;

        //let options = this.getOptions();
        //let filters = this.getFilters();

        // if(!this.uploader.isHTML5) this.destroy();

        this.selectFile.emit(files);
    }
}