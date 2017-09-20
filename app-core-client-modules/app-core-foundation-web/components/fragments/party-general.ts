import {Component, View, OnInit, EventEmitter} from 'angular2/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, ControlGroup} from 'angular2/common';

import {TAB_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {FormInput, EntityDropdown, EntitySummary} from 'app-core-web/app-core-web';

import {IDataService, ILogService} from 'app-core/app-core';
import {PartyGeneralFragment} from 'app-core-foundation/fragments';

@Component({
    selector: 'partyGeneral',
    inputs: ['value', 'dataService'],
    outputs: ['valueChange'],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, NgClass, TAB_DIRECTIVES, EntityDropdown, FormInput, EntitySummary],
    template: `
<!--
<form-input [entity]="entity" name="PartyType" title="Type">
    <select class="form-control" [(ngModel)]="entity.PartyType">
        <option value="I">Individual</option>
        <option value="C">Company</option>
    </select>
</form-input>
-->
<div>
    <form-input [entity]="entity" name="FirstName" title="FirstName">
        <input type="text" class="form-control" name="FirstName" [(ngModel)]="entity.FirstName">
    </form-input>
    <form-input [entity]="entity" name="Surname" title="Surname">
        <input type="text" class="form-control" name="Surname" [(ngModel)]="entity.Surname">
    </form-input>
</div>

<div>
    <form-input [entity]="entity" name="Name" title="Name">
        <input type="text" class="form-control" name="Name" [(ngModel)]="entity.Name">
    </form-input>
</div>
    `
})
export class PartyGeneralFragmentWeb extends PartyGeneralFragment {
    constructor(logService:ILogService) {
        super(logService);
    }

}

