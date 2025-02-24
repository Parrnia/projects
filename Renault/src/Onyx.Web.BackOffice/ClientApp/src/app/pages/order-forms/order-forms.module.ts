import { MatButtonModule } from '@angular/material/button';
import { MatStepperModule } from '@angular/material/stepper';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';

import { OrderFormsRoutingModule } from './order-forms.routing';
import { TablesRoutingModule } from '../tables/tables-routing.module';
import { AllOrderModule } from './allOrders/all-order.module';
import { CanceledOrderModule } from './canceledOrders/canceled-order.module';
import { ConfirmedOrderModule } from './confirmedOrders/confirmed-order.module';
import { CostRefundedOrderModule } from './costRefundedOrders/cost-refunded-order.module';
import { PreparedOrderModule } from './preparedOrders/prepared-order.module';
import { RegisteredOrderModule } from './registeredOrders/registered-order.module';
import { ShippedOrderModule } from './shippedOrders/shipped-order.module';
import { UnconfirmedOrderModule } from './unconfirmedOrders/unconfirmed-order.module';
import { ConfirmedPaymentOrderModule } from './paymentConfirmedOrders/payment-confirmed-order.module';
import { CompletedOrderComponent } from './completedOrders/completed-order.component';
import { UnconfirmedPaymentOrderModule } from './paymentUnconfirmedOrders/payment-unconfirmed-order.module';
import { CompletedOrderModule } from './completedOrders/completed-order.module';
import { OrderWorkflowComponent } from './orderWorkflow/order-workflow.component';
import {MatRadioModule} from '@angular/material/radio';
import { MatIconModule } from '@angular/material/icon';
import { WorkflowOrderModule } from './orderWorkflow/workflowOrders/workflow-order.module';
import { PendingForRegisterOrderModule } from './pendingForRegisterOrders/pending-for-register-order.module';

@NgModule({
  declarations: [
    OrderWorkflowComponent,
  ],
  imports: [
    MatButtonModule,      
    MatStepperModule,      
    MatInputModule,
    MatRadioModule,
    MatFormFieldModule,
    MatIconModule,
    FormsModule,
    ReactiveFormsModule,
    AllOrderModule,
    CanceledOrderModule,
    ConfirmedOrderModule,
    CostRefundedOrderModule,
    PreparedOrderModule,
    RegisteredOrderModule,
    ShippedOrderModule,
    UnconfirmedOrderModule,
    ConfirmedPaymentOrderModule,
    UnconfirmedPaymentOrderModule,
    WorkflowOrderModule,
    PendingForRegisterOrderModule,
    CompletedOrderModule,
    OrderFormsRoutingModule,
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


export class OrderFormsModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



