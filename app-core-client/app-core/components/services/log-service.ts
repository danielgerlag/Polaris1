/**
 * Created by dgerlag on 03'Mar'2016.
 */
import {Component, OnInit, EventEmitter, View, provide, ElementRef, Injector, Renderer, Inject, Injectable, IterableDiffers, KeyValueDiffers} from 'angular2/core';

export abstract class ILogService {
    abstract logError(error);
    abstract logDebug(msg);

}

@Injectable()
export class LogService implements ILogService {


    constructor() {
    }


    public logError(error) {
    }

    public logDebug(msg) {
        console.log(msg);
    }


}
