import {provide, ElementRef, Injector, Renderer, Inject, Injectable, IterableDiffers, KeyValueDiffers} from 'angular2/core';
import {ICustomModal, ModalDialogInstance, ModalConfig, Modal} from 'angular2-modal/angular2-modal';
import {ErrorDialog} from './../error-dialog'
import {IDialogService, IDataService} from 'app-core/app-core'

@Injectable()
export class WebDialogService implements IDialogService {


    constructor(
        private modal: Modal,
        private injector: Injector,
        private _renderer: Renderer) {
    }


    showErrorDialog(error) {
        let dialog: Promise<ModalDialogInstance>;
        let component = ErrorDialog;
        var self = this;


        let bindings = Injector.resolve([
            provide(ICustomModal, { useValue: error }),
            provide(IterableDiffers, { useValue: this.injector.get(IterableDiffers) }),
            provide(KeyValueDiffers, { useValue: this.injector.get(KeyValueDiffers) }),
            provide(Renderer, { useValue: this._renderer })
        ]);

        dialog = this.modal.open(
            <any>component,
            bindings,
            new ModalConfig("lg", false, 27));


        dialog.then((resultPromise) => {
            return resultPromise.result.then((result) => {
                // ship logs
            }, () => {
                //
            });
        });
    }



    showDialog(sender: any, dialogComponent: any, data: any, dataService: IDataService, success: (sender: any, result: any) => any, cancel: (sender: any) => any) {

        let dialog: Promise<ModalDialogInstance>;
        let component = dialogComponent;
        var self = this;


        let bindings = Injector.resolve([
            provide(IDataService, { useValue: dataService }),
            provide(ICustomModal, { useValue: data }),
            provide(IterableDiffers, { useValue: this.injector.get(IterableDiffers) }),
            provide(KeyValueDiffers, { useValue: this.injector.get(KeyValueDiffers) }),
            provide(Renderer, { useValue: this._renderer })
        ]);

        dialog = this.modal.open(
            <any>component,
            bindings,
            new ModalConfig("lg", false, 27));


        dialog.then((resultPromise) => {
            return resultPromise.result.then((result) => {
                success(sender, result);
            }, () => {
                cancel(sender);
            });
        });
    }

}

