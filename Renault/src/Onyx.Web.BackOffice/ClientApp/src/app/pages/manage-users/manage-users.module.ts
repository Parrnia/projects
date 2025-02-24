import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';
import { TablesRoutingModule } from '../tables/tables-routing.module';
import { CustomerModule } from './customers/customer.module';
import { ManageUsersRoutingModule } from './manage-users.routing';
import { CustomerCreditModule } from './customers-credit/customer-credit.module';
import { EmployeeModule } from './employees/employee.module';
import { GroupModule } from './groups/group.module';
import { CustomerTypeModule } from './customer-types/customer-type.module';
import { AddressesModule } from './customers/addresses/addresses.module';

@NgModule({
  declarations: [
    
  
  ],
  imports: [
    CustomerTypeModule,
    GroupModule,
    EmployeeModule,
    CustomerCreditModule,
    AddressesModule,
    CustomerModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    NgbPaginationModule,
    NgbTypeaheadModule,
    NgbAlertModule,
    FlatpickrModule,
    TablesRoutingModule,
    SharedModule,
    ManageUsersRoutingModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class ManageUsersModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



