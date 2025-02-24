import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AllOrderComponent } from './allOrders/all-order.component';
import { CanceledOrderComponent } from './canceledOrders/canceled-order.component';
import { ConfirmedOrderComponent } from './confirmedOrders/confirmed-order.component';
import { CostRefundedOrderComponent } from './costRefundedOrders/cost-refunded-order.component';
import { PreparedOrderComponent } from './preparedOrders/prepared-order.component';
import { RegisteredOrderComponent } from './registeredOrders/registered-order.component';
import { ShippedOrderComponent } from './shippedOrders/shipped-order.component';
import { UnconfirmedOrderComponent } from './unconfirmedOrders/unconfirmed-order.component';
import { ConfirmedPaymentOrderComponent } from './paymentConfirmedOrders/payment-confirmed-order.component';
import { UnconfirmedPaymentOrderComponent } from './paymentUnconfirmedOrders/payment-unconfirmed-order.component';
import { CompletedOrderComponent } from './completedOrders/completed-order.component';
import { OrderWorkflowComponent } from './orderWorkflow/order-workflow.component';
import { PendingForRegisterOrderComponent } from './pendingForRegisterOrders/pending-for-register-order.component';


// Component Pages

const routes: Routes = [
  {
    path: "allOrders",
    component: AllOrderComponent,
  },
  {
    path: "canceledOrders",
    component: CanceledOrderComponent,
  },
  {
    path: "confirmedOrders",
    component: ConfirmedOrderComponent,
  },
  {
    path: "costRefundedOrders",
    component: CostRefundedOrderComponent,
  },
  {
    path: "preparedOrders",
    component: PreparedOrderComponent,
  },
  {
    path: "registeredOrders",
    component: RegisteredOrderComponent,
  },
  {
    path: "shippedOrders",
    component: ShippedOrderComponent,
  },
  {
    path: "unconfirmedOrders",
    component: UnconfirmedOrderComponent,
  },
  {
    path: "paymentUnconfirmedOrders",
    component: UnconfirmedPaymentOrderComponent,
  },
  {
    path: "paymentConfirmedOrders",
    component: ConfirmedPaymentOrderComponent,
  },
  {
    path: "completeOrders",
    component: CompletedOrderComponent,
  },
  {
    path: "pendingForRegister",
    component: PendingForRegisterOrderComponent,
  },
  {
    path: "orderWorkflow/:id",
    component: OrderWorkflowComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class OrderFormsRoutingModule { }
