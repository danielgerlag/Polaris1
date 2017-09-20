import {Router} from 'angular2/router';
import {ListController, IShellService, IDataService, IAuthService} from 'app-core/app-core';


export abstract class WebListController extends ListController {

    constructor(shellService: IShellService, dataService: IDataService, router: Router, authService: IAuthService) {
        super(shellService, dataService, router, authService);
    }

    protected getSchemaModel() {
        return {};
    }

    protected getDataSource():any {
        var query = this.getQuery();
        var servicePath = this.dataService.servicePath + "/";
        var tenantID = 0;

        if (this.authService.userInfo.activeTenant) {
            tenantID = this.authService.userInfo.activeTenant.ID;
        }

        var result = {
            type: "odata",
            transport: {
                read: {
                    url: servicePath + query,
                    dataType: "json",
                    beforeSend: function(xhr) {
                        xhr.setRequestHeader('TenantID', tenantID)
                    }
                }
            },

            schema: {
                model: this.getSchemaModel(),
                data: function (data) {
                    return data.value;
                },
                total: function (data) {
                    return data['odata.count'];
                }
            },
            pageSize: 20,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true
        };


        // var manager = this.dataService.createManager();
        // var query = this.getQuery();
        // var result = new kendo.data.breeze.Source({
        //     manager: manager,
        //     query: query,
        //     serverSorting: true,
        //     serverPaging: true,
        //     serverFiltering: true,
        //     pageSize: 20
        //
        // });
        return result;
    }


}
