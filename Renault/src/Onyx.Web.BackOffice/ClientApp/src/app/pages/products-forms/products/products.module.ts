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
import { ProductsComponent } from './products.component';
import { NgbdProductsSortableHeader } from './products-sortable.directive';
import { ProductImagesModule } from './product-images/product-images.module';
import { ProductImagesComponent } from './product-images/product-images.component';
import { ProductKindsComponent } from './product-kinds/product-kinds/product-kinds.component';
import { ProductAttributesComponent } from './product-attributes/product-attributes.component';
import { KindsComponent } from './product-kinds/kinds/kinds.component';
import { PriceComponent } from './product-attribute-options/prices/price.component';
import { ProductAttributeOptionComponent } from './product-attribute-options/product-attribute-option.component';
import { ProductOptionRoleComponent } from './product-attribute-options/product-option-role/product-option-role.component';
import { BadgesComponent } from './product-attribute-options/product-badges/badges/badges.component';
import { ProductBadgesComponent } from './product-attribute-options/product-badges/product-badges/product-badges.component';
import { SummaryPipe } from 'src/app/shared/pipes/summary.pipe';
import { ProductDisplayVariantsModule } from './product-display-variants/product-display-variants.module';
import { ProductDisplayVariantsComponent } from './product-display-variants/product-display-variants.component';

@NgModule({
  declarations: [
    NgbdProductsSortableHeader,
    ProductsComponent,
    ProductImagesComponent,
    ProductDisplayVariantsComponent,
    ProductKindsComponent,
    ProductAttributesComponent,
    ProductAttributeOptionComponent,
    ProductOptionRoleComponent,
    KindsComponent,
    PriceComponent,
    BadgesComponent,
    ProductBadgesComponent,
    SummaryPipe,
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
    ProductImagesModule,
    ProductDisplayVariantsModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class ProductsModule {
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



