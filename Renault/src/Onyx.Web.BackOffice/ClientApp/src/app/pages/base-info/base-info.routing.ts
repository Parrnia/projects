import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductBrandComponent } from './brands-cluster/product-brands/product-brand.component';
import { KindComponent } from './brands-cluster/kinds/kind.component';
import { ProductTypeComponent } from './products-cluster/product-types/product-type.component';
import { FamilyComponent } from './brands-cluster/families/family.component';
import { ModelComponent } from './brands-cluster/models/model.component';
import { VehicleComponent } from './brands-cluster/vehicles/vehicle.component';
import { ProductCategoryComponent } from './categories-cluster/product-categories/product-category.component';
import { AboutUsComponent } from './info-cluster/about-us-info/about-us.component';
import { CorporationInfoComponent } from './info-cluster/corporation-infos/corporation-info.component';
import { CountryComponent } from './info-cluster/countries/country.component';
import { QuestionComponent } from './info-cluster/questions/question.component';
import { TeamMemberComponent } from './info-cluster/team-members/team-member.component';
import { TestimonialComponent } from './info-cluster/testimonials/testimonial.component';
import { BadgeComponent } from './products-cluster/badges/badge.component';
import { CountingUnitTypeComponent } from './products-cluster/counting-unit-types/counting-unit-type.component';
import { CountingUnitComponent } from './products-cluster/counting-units/counting-unit.component';
import { ProductStatusComponent } from './products-cluster/product-statuses/product-status.component';
import { ProviderComponent } from './products-cluster/providers/provider.component';
import { TagComponent } from './products-cluster/tags/tag.component';
import { ReviewComponent } from './products-cluster/reviews/review.component';
import { VehicleBrandComponent } from './brands-cluster/vehicle-brands/vehicle-brand.component';
import { CustomerTicketComponent } from './customer-support-cluster/customer-tickets/customer-ticket.component';
import { RequestLogComponent } from './RequestsCluster/requestLogs/request-log.component';


// Component Pages

const routes: Routes = [
  {
    path: "productBrands",
    component: ProductBrandComponent
  },
  {
    path: "vehicleBrands",
    component: VehicleBrandComponent
  },
  {
    path: "families",
    component: FamilyComponent
  },
  {
    path: "models",
    component: ModelComponent
  },
  {
    path: "kinds",
    component: KindComponent
  },
  {
    path: "vehicles",
    component: VehicleComponent
  },
  {
    path: "productCategories",
    component: ProductCategoryComponent
  },
  {
    path: "customerTickets",
    component: CustomerTicketComponent
  },
  {
    path: "aboutUs",
    component: AboutUsComponent
  },
  {
    path: "corporationInfo",
    component: CorporationInfoComponent
  },
  {
    path: "country",
    component: CountryComponent
  },
  {
    path: "question",
    component: QuestionComponent
  },
  {
    path: "teamMember",
    component: TeamMemberComponent
  },
  {
    path: "testimonial",
    component: TestimonialComponent
  },
  {
    path: "badge",
    component: BadgeComponent  
  },
  {
    path: "countingUnit",
    component: CountingUnitComponent  
  },
  {
    path: "countingUnitType",
    component: CountingUnitTypeComponent  
  },
  {
    path: "productStatus",
    component: ProductStatusComponent  
  },
  {
    path: "productType",
    component: ProductTypeComponent  
  },
  {
    path: "provider",
    component: ProviderComponent  
  },
  {
    path: "tag",
    component: TagComponent  
  },
  {
    path: "review",
    component: ReviewComponent  
  },
  {
    path: "requestLogs",
    component: RequestLogComponent  
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class BaseInfoRoutingModule { }
