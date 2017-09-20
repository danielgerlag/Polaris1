import {IDataService} from './data-service'

export abstract class IDialogService {
    abstract showErrorDialog(error);
    abstract showDialog(sender: any, dialogComponent: any, data: any, dataService: IDataService, success: (sender: any, result: any) => any, cancel: (sender: any) => any);
}

