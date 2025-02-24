import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { BaseInfoRoutingModule } from './base-info.routing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { TablesRoutingModule } from '../tables/tables-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';
import { KindComponent } from './brands-cluster/kinds/kind.component';
import { NgbdKindSortableHeader } from './brands-cluster/kinds/kind-sortable.directive';
import { ProductTypeComponent } from './products-cluster/product-types/product-type.component';
import { ProductsTypeModule } from './products-cluster/product-types/products-type.module';
import { FamilyComponent } from './brands-cluster/families/family.component';
import { KindModule } from './brands-cluster/kinds/kind.module';
import { FamilyModule } from './brands-cluster/families/family.module';
import { ModelModule } from './brands-cluster/models/model.module';
import { VehicleModule } from './brands-cluster/vehicles/vehicle.module';
import { ProductCategoryModule } from './categories-cluster/product-categories/product-category.module';
import { AboutUsModule } from './info-cluster/about-us-info/about-us.module';
import { CorporationInfoModule } from './info-cluster/corporation-infos/corporation-info.module';
import { CountryModule } from './info-cluster/countries/country.module';
import { QuestionModule } from './info-cluster/questions/question.module';
import { TeamMemberModule } from './info-cluster/team-members/team-member.module';
import { TestimonialModule } from './info-cluster/testimonials/testimonial.module';
import { BadgeModule } from './products-cluster/badges/badge.module';
import { CountingUnitTypeModule } from './products-cluster/counting-unit-types/counting-unit-type.module';
import { CountingUnitModule } from './products-cluster/counting-units/counting-unit.module';
import { ProductStatusModule } from './products-cluster/product-statuses/product-status.module';
import { ProviderModule } from './products-cluster/providers/provider.module';
import { TagModule } from './products-cluster/tags/tag.module';
import { ReviewModule } from './products-cluster/reviews/review.module';
import { ProductBrandModule } from './brands-cluster/product-brands/product-brand.module';
import { VehicleBrandModule } from './brands-cluster/vehicle-brands/vehicle-brand.module';
import { CustomerTicketModule } from './customer-support-cluster/customer-tickets/customer-ticket.module';
import { RequestLogModule } from './RequestsCluster/requestLogs/request-log.module';




@NgModule({
  imports: [
    TagModule,
    ReviewModule,
    ProviderModule,
    ProductStatusModule,
    CountingUnitModule,
    CountingUnitTypeModule,
    BadgeModule,
    TestimonialModule,
    TeamMemberModule,
    QuestionModule,
    CountryModule,
    CorporationInfoModule,
    AboutUsModule,
    ProductBrandModule,
    VehicleBrandModule,
    ProductCategoryModule,
    CustomerTicketModule,
    KindModule,
    FamilyModule,
    ModelModule,
    VehicleModule,
    RequestLogModule,
    ProductsTypeModule,
    CommonModule,
    BaseInfoRoutingModule,
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
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class BaseInfoModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



