import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';
import { TablesRoutingModule } from '../../tables/tables-routing.module';
import { ProductAttributeTypesComponent } from './product-attribute-types.component';
import { NgbdProductAttributeTypesSortableHeader } from './product-attribute-types-sortable.directive';
import { TypeGroupsComponent } from './type-groups/type-groups.component';
import { ProductAttributeGroupComponent } from './product-attribute-group/product-attribute-group.component';



@NgModule({
  declarations: [
    NgbdProductAttributeTypesSortableHeader,
    ProductAttributeTypesComponent,
    TypeGroupsComponent,
    ProductAttributeGroupComponent

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
    NgbNavModule,
    TablesRoutingModule,
    SharedModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class ProductAttributeTypesModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



