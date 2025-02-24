import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTooltipModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';
import { TablesRoutingModule } from '../../../tables/tables-routing.module';
import { NgbdProviderSortableHeader } from './provider-sortable.directive';
import { ProviderComponent } from './provider.component';



@NgModule({
  declarations: [
    NgbdProviderSortableHeader,
    ProviderComponent
  
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
    NgbTooltipModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class ProviderModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



