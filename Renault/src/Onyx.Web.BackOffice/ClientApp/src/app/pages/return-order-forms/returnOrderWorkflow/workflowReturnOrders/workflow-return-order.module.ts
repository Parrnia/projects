import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTooltipModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';
import { WorkflowReturnOrderComponent } from './workflow-return-order.component';
import { ReturnOrderItemsComponent } from '../return-order-item-groups/return-order-items/return-order-item.component';
import { ReturnOrderItemGroupComponent } from '../return-order-item-groups/return-order-item-group.component';
import { ReturnOrderItemDocumentComponent } from '../return-order-item-groups/return-order-items/return-order-item-documents/return-order-item-document.component';
import { ReturnOrderItemOptionComponent } from '../return-order-item-groups/return-order-item-options/return-order-item-option.component';
import { ReturnOrderTotalDocumentComponent } from '../return-order-totals/return-order-total-documents/return-order-total-document.component';
import { ReturnOrderStateComponent } from '../return-order-status/return-order-state.component';
import { ReturnOrderInfoComponent } from '../return-order-info/return-order-info.component';
import { TablesRoutingModule } from 'src/app/pages/tables/tables-routing.module';
import { ReturnOrderTotalsComponent } from '../return-order-totals/return-order-total.component';




@NgModule({
  declarations: [
    WorkflowReturnOrderComponent,
    ReturnOrderItemGroupComponent,
    ReturnOrderItemsComponent,
    ReturnOrderItemDocumentComponent,
    ReturnOrderItemOptionComponent,
    ReturnOrderTotalsComponent,
    ReturnOrderTotalDocumentComponent,
    ReturnOrderStateComponent,
    ReturnOrderInfoComponent,
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
  exports:[
    WorkflowReturnOrderComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})


export class WorkflowReturnOrderModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



