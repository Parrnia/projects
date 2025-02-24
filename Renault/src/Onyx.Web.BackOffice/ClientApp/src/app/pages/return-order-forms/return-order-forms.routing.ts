import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AllReturnOrderComponent } from './allReturnOrders/all-return-order.component';
import { AcceptedReturnOrderComponent } from './acceptedReturnOrders/accepted-return-order.component';
import { AllConfirmedReturnOrderComponent } from './allConfirmedReturnOrders/all-confirmed-return-order.component';
import { CanceledReturnOrderComponent } from './canceledReturnOrders/canceled-return-order.component';
import { CompletedReturnOrderComponent } from './completedReturnOrders/completed-return-order.component';
import { CostRefundedReturnOrderComponent } from './costRefundedReturnOrders/cost-refunded-return-order.component';
import { ReceivedReturnOrderComponent } from './receivedReturnOrders/received-return-order.component';
import { RegisteredReturnOrderComponent } from './registeredReturnOrders/registered-return-order.component';
import { RejectedReturnOrderComponent } from './rejectedReturnOrders/rejected-return-order.component';
import { SentReturnOrderComponent } from './sentReturnOrders/sent-return-order.component';
import { SomeConfirmedReturnOrderComponent } from './someConfirmedReturnOrders/some-confirmed-return-order.component';
import { ReturnOrderWorkflowComponent } from './returnOrderWorkflow/return-order-workflow.component';


// Component Pages

const routes: Routes = [
  {
    path: "allReturnOrders",
    component: AllReturnOrderComponent
  },
  {
    path: "acceptedReturnOrders",
    component: AcceptedReturnOrderComponent
  },
  {
    path: "allConfirmedReturnOrders",
    component: AllConfirmedReturnOrderComponent
  },
  {
    path: "canceledReturnOrders",
    component: CanceledReturnOrderComponent
  },
  {
    path: "completedReturnOrders",
    component: CompletedReturnOrderComponent
  },
  {
    path: "costRefundedReturnOrders",
    component: CostRefundedReturnOrderComponent
  },
  {
    path: "receivedReturnOrders",
    component: ReceivedReturnOrderComponent
  },
  {
    path: "registeredReturnOrders",
    component: RegisteredReturnOrderComponent
  },
  {
    path: "rejectedReturnOrders",
    component: RejectedReturnOrderComponent
  },
  {
    path: "sentReturnOrders",
    component: SentReturnOrderComponent
  },
  {
    path: "someConfirmedReturnOrders",
    component: SomeConfirmedReturnOrderComponent
  },
  {
    path: "returnOrderWorkflow/:id",
    component: ReturnOrderWorkflowComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ReturnOrderFormsRoutingModule { }
