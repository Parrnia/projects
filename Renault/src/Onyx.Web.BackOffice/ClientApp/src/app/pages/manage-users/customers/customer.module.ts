import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';;
import { NgbdCustomerSortableHeader } from './customer-sortable.directive';
import { CustomerComponent } from './customer.component';
import { TablesRoutingModule } from '../../tables/tables-routing.module';
import { AddressesModule } from './addresses/addresses.module';
import { AddressesComponent } from './addresses/addresses.component';



@NgModule({
  declarations: [
    NgbdCustomerSortableHeader,
    CustomerComponent,
    AddressesComponent

  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    NgbPaginationModule,
    NgbTypeaheadModule,
    NgbAlertModule,
    NgbNavModule,
    FlatpickrModule,
    TablesRoutingModule,
    SharedModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class CustomerModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



