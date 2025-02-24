import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { TablesRoutingModule } from '../tables/tables-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';
import { ProductsFormsRoutingModule } from './products-forms.routing';
import { ProductsModule } from './products/products.module';
import { ProductAttributeTypesModule } from './product-attribute-types/product-attribute-types.module';
import { ProductOptionColorModule } from './product-attribute-option-color/product-option-color.module';
// import { ProductOptionRoleModule } from './product-attribute-option-role/product-attribute-option-role.module';
import { ProductAttributeGroupModule } from './product-attribute-group/product-attribute-group.module';
import { ProductAttributeGroupAttributeModule } from './product-attribute-group-attribute/product-attribute-group-attribute.module';
import { ProductOptionMaterialModule } from './product-attribute-option-material/product-option-material.module';



@NgModule({
  declarations: [],
  imports: [
    ProductAttributeGroupModule,
    ProductAttributeGroupAttributeModule,
    ProductsModule,
    ProductOptionColorModule,
    ProductOptionMaterialModule,
    ProductAttributeTypesModule,
    ProductsFormsRoutingModule,
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
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class ProductsFormsModule {
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



