import {Component} from 'angular2/core';
import {NgClass, FORM_BINDINGS, CORE_DIRECTIVES, FORM_DIRECTIVES} from 'angular2/common';
import {Router} from 'angular2/router';
import {DataComponent, IShellService, IDataService, IAuthService} from 'app-core/app-core';
import {DataGrid} from 'app-core-web/app-core-web';
import * as moment from 'moment';

@Component({
    selector: 'accountingEntityLedgerBalances',
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, DataGrid],
    inputs: ['accountingEntityID', 'ledgerID', 'effectiveDate'],
    template: `    
    <div dataGrid
         [dataSource]="dataSource"
         [(selectedItem)]="selectedItem"
         [columns]="columns"
         height="300">
    </div>
    `
})
export class AccountingEntityLedgerBalances extends DataComponent {

    protected authService: IAuthService;

    public accountingEntityID: number;
    public ledgerID: number;
    public effectiveDate: Date;

    public columns: any;
    public dataSource: any;

    constructor(shellService: IShellService, dataService: IDataService, router: Router, authService: IAuthService) {
        super(shellService, dataService);
        this.authService = authService;
    }

    public ngOnInit() {
        super.ngOnInit();
        this.columns = this.getColumns();
        this.dataSource = this.getDataSource();
    }

    protected getQuery() {
        var dateStr = moment(this.effectiveDate).format('YYYY-MM-DD');
        return "GetLedgerAccountBalances?accountingEntityID=" + this.accountingEntityID + "&ledgerID=" + this.ledgerID + "&effectiveDate='" + dateStr + "'";
    }

    protected getSchemaModel() {
        return {
            fields: {
                'AccountingEntityName': { type: "string" },
                'AccountingEntityID': { type: "number" },
                'LedgerAccountID': { type: "number" },
                'LedgerAccountName': { type: "string" },
                'PartyName': { type: "string" },
                'PartyID': { type: "number" },
                'Balance': { type: "number" }
            }
        };
    }

    protected getDataSource():any {
        var query = this.getQuery();
        var servicePath = this.dataService.servicePath + "/";
        var tenantID = 0;

        if (this.authService.userInfo.activeTenant) {
            tenantID = this.authService.userInfo.activeTenant.ID;
        }

        var result = {
            //type: "odata",
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

        return result;
    }


    protected getColumns(): any {
        return [
            { title: 'Account', field: 'LedgerAccountName' },
            { title: 'Balance', field: 'Balance' }
        ];
    }

}
