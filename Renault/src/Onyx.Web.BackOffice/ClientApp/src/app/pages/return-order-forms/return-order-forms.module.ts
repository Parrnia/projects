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

import { ReturnOrderFormsRoutingModule } from './return-order-forms.routing';
import { TablesRoutingModule } from '../tables/tables-routing.module';
import { AllReturnOrderModule } from './allReturnOrders/all-return-order.module';
import {MatRadioModule} from '@angular/material/radio';
import { MatIconModule } from '@angular/material/icon';
import { AcceptedReturnOrderModule } from './acceptedReturnOrders/accepted-return-order.module';
import { AllConfirmedReturnOrderModule } from './allConfirmedReturnOrders/all-confirmed-return-order.module';
import { CanceledReturnOrderModule } from './canceledReturnOrders/canceled-return-order.module';
import { CompletedReturnOrderModule } from './completedReturnOrders/completed-return-order.module';
import { CostRefundedReturnOrderModule } from './costRefundedReturnOrders/cost-refunded-return-order.module';
import { ReceivedReturnOrderModule } from './receivedReturnOrders/received-return-order.module';
import { RegisteredReturnOrderModule } from './registeredReturnOrders/registered-return-order.module';
import { RejectedReturnOrderModule } from './rejectedReturnOrders/rejected-return-order.module';
import { SentReturnOrderModule } from './sentReturnOrders/sent-return-order.module';
import { SomeConfirmedReturnOrderModule } from './someConfirmedReturnOrders/some-confirmed-return-order.module';
import { ReturnOrderWorkflowComponent } from './returnOrderWorkflow/return-order-workflow.component';
import { WorkflowReturnOrderModule } from './returnOrderWorkflow/workflowReturnOrders/workflow-return-order.module';

@NgModule({
  declarations: [
    ReturnOrderWorkflowComponent,
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
    AllReturnOrderModule,
    ReturnOrderFormsRoutingModule,
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
    AcceptedReturnOrderModule,
    AllConfirmedReturnOrderModule,
    CanceledReturnOrderModule,
    CompletedReturnOrderModule,
    CostRefundedReturnOrderModule,
    ReceivedReturnOrderModule,
    RegisteredReturnOrderModule,
    RejectedReturnOrderModule,
    SentReturnOrderModule,
    SomeConfirmedReturnOrderModule,
    WorkflowReturnOrderModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
  ,
})


export class ReturnOrderFormsModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}



