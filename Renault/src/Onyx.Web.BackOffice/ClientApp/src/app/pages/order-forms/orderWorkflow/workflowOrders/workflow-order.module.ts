import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import lottie from "lottie-web";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import {
  NgbAlertModule,
  NgbDropdownModule,
  NgbNavModule,
  NgbPaginationModule,
  NgbTooltipModule,
  NgbTypeaheadModule,
} from "@ng-bootstrap/ng-bootstrap";
import { FlatpickrModule } from "angularx-flatpickr";
import { SharedModule } from "src/app/shared/shared.module";
import { defineElement } from "lord-icon-element";
import { OrderItemsComponent } from "./order-items/order-item.component";
import { OrderTotalsComponent } from "./order-totals/order-total.component";
import { OptionComponent } from "./order-items/option/option.component";
import { OrderStateComponent } from "./order-status/order-state.component";
import { OrderInfoComponent } from "./order-info/order-info.component";
import { NgPersianDatepickerModule } from "ng-persian-datepicker";
import { WorkflowOrderComponent } from "./workflow-order.component";
import { TablesRoutingModule } from "src/app/pages/tables/tables-routing.module";

@NgModule({
  declarations: [
    WorkflowOrderComponent,
    OrderItemsComponent,
    OrderTotalsComponent,
    OrderStateComponent,
    OptionComponent,
    OrderInfoComponent,
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
    NgbTooltipModule,
    NgPersianDatepickerModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class WorkflowOrderModule {
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}
