import {Inject, Injectable, Injector, Component, provide} from 'angular2/core';
import {IAuthService, IConfigService, ILogService} from 'app-core/app-core';
import {DataService} from 'app-core/app-core';


@Injectable()
export class HR1DataService extends DataService {
    constructor(configService:IConfigService, logService:ILogService, authService: IAuthService) {
        super(configService, logService, authService);
    }

    protected getRelativePath():string {
        return "Services/odata.svc";
    }
}