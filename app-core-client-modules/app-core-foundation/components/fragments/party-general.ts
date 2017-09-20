import {ILogService, DetailFragment} from 'app-core/app-core';

export abstract class PartyGeneralFragment extends  DetailFragment {
    constructor(logService:ILogService) {
        super(logService);
    }

}

