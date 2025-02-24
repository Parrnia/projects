import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';;
import { NgbdCarouselSortableHeader } from './carousel-sortable.directive';
import { CarouselComponent } from './carousel.component';
import { TablesRoutingModule } from '../../tables/tables-routing.module';




@NgModule({
  declarations: [
    NgbdCarouselSortableHeader,
    CarouselComponent
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


export class CarouselModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



