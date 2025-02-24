import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';;
import { TablesRoutingModule } from '../../../tables/tables-routing.module';
import { NgbdCorporationInfoSortableHeader } from './corporation-info-sortable.directive';
import { CorporationInfoComponent } from './corporation-info.component';
import { EmailAddressComponent } from './email-addresses/email-address.component';
import { LocationAddressComponent } from './location-address/location-address.component';
import { PhoneNumberComponent } from './phone-number/phone-number.component';
import { WorkingHourComponent } from './working-hour/working-hour.component';



@NgModule({
  declarations: [
    NgbdCorporationInfoSortableHeader,
    CorporationInfoComponent,
    EmailAddressComponent,
    LocationAddressComponent,
    PhoneNumberComponent,
    WorkingHourComponent,
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


export class CorporationInfoModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



