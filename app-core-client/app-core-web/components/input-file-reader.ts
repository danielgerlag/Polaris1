import {Component, ElementRef, EventEmitter} from 'angular2/core';

@Component({
    selector: 'filereader',
    providers: [ElementRef],
    inputs: ['accept'],
    events: ['complete'],
    template: `<input class="form-control" type="file" (change)="changeListener($event)" accept="{{accept}}" />`
})
export class InputFileReader {
    complete: EventEmitter<any> = new EventEmitter<any>();
    accept: string;
    constructor(public elementRef: ElementRef) {
    }

    changeListener($event: any) {
        var self = this;
        var file: File = $event.target.files[0];
        var myReader: any = new FileReader();
        myReader.readAsBinaryString(file);
        myReader.onload = function (e) {
            if (myReader.result) myReader.content = myReader.result;
            self.complete.emit({
                data: window.btoa(myReader.content),
                fileName: file.name
        });
        };
    }
}