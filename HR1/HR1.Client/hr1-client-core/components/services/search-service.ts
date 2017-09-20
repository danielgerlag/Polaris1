import {Inject, Injectable, Injector, Component, provide} from 'angular2/core';
import {Router} from 'angular2/router';
import {IShellService, IConfigService, IRemoteService} from 'app-core/app-core';
import {SearchService, SearchResponseLine, SearchType} from 'app-core/app-core';


@Injectable()
export class HR1SearchService extends SearchService {

    constructor(router:Router, remoteService:IRemoteService, shellService: IShellService) {
        super(router, remoteService, shellService);
    }

    public selectResult(item: SearchResponseLine) {
        switch (item.EntityType) {
            case "Employment":
                this.router.navigateByUrl("Employment/" + item.ID);
                break;
        }
    }
    public getSearchTypes(): SearchType[] {
        var result = [];
        result.push(new SearchType("", "All"));

        return result;
    }

}