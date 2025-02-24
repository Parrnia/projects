import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';;
import { NgbdCustomerCreditSortableHeader } from './customer-credit-sortable.directive';
import { CustomerCreditComponent } from './customer-credit.component';
import { TablesRoutingModule } from '../../tables/tables-routing.module';
import { CreditModule } from './credits/credit.module';
import { CreditComponent } from './credits/credit.component';
import { AmountToWordsPipe } from 'src/app/shared/pipes/amountToWords.pipe';
import { MaxCreditComponent } from './max-credits/max-credit.component';
import { MaxCreditModule } from './max-credits/max-credit.module';


@NgModule({
  declarations: [
    NgbdCustomerCreditSortableHeader,
    CustomerCreditComponent,
    CreditComponent,
    MaxCreditComponent,
    AmountToWordsPipe,
  ],
  imports: [
    CreditModule,
    MaxCreditModule,
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
    NgbNavModule,
  ],
  exports : [
    AmountToWordsPipe,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class CustomerCreditModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



