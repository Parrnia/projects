import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';;
import { NgbdThemeSortableHeader } from './theme-sortable.directive';
import { ThemeComponent } from './theme.component';
import { TablesRoutingModule } from '../../tables/tables-routing.module';
import { ColorPickerModule } from 'ngx-color-picker';



@NgModule({
  declarations: [
    NgbdThemeSortableHeader,
    ThemeComponent
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
    FlatpickrModule,
    TablesRoutingModule,
    SharedModule,
    ColorPickerModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class ThemeModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



