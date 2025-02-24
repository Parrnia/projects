
export interface ITab {
  id: number;
  title: string;
}

import { Component, Input, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, of, switchAll } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReturnOrdersClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ReturnOrderModel } from '../../return-order.model';
import { WorkflowReturnOrderGridService } from './workflow-return-order-grid.service';
import { WorkflowReturnOrderModel } from './workflow-return-order.model';


@Component({
  selector: 'app-workflow-return-order',
  templateUrl: './workflow-return-order.component.html',
  styleUrls: ['./workflow-return-order.component.scss'],
  providers: [WorkflowReturnOrderGridService, DecimalPipe, NgbAlertConfig]
})
export class WorkflowReturnOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;

  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderModel;
  gridjsList$!: Observable<WorkflowReturnOrderModel>;
  selectedReturnOrder?: WorkflowReturnOrderModel;
  selectedReturnOrderInfo?: any;
  inProgress = false;

  @ViewChild('productTabModel') productTabModel: any = [];


  activeTab = 1;


  checkedItems: Set<number> = new Set<number>();

  @Input() returnOrderId?: any;
  @Input() stepId?: any;

  constructor(
    public client: ReturnOrdersClient,
    private modalService: NgbModal,
    public service: WorkflowReturnOrderGridService,
    private returnOrdersClient: ReturnOrdersClient
  ) {
    this.gridjsList$ = service.returnOrder$;
  }

  ngOnInit(): void {
    this.service.refreshReturnOrders(this.returnOrderId);
  }


  openMoreDetailReturnOrder(item: Observable<WorkflowReturnOrderModel>) {
    item.subscribe({
      next: (item) => {
        this.selectedReturnOrder = item;
        this.returnOrdersClient.getReturnOrderInfo(item.id).subscribe(
          c => this.selectedReturnOrderInfo = c
        )
        this.modalService.open(this.productTabModel, { size: 'lg', backdrop: 'static' });
      }
    });
  }

  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }

  refreshGrid() {
    this.service.refreshReturnOrders(this.returnOrderId);
    this.gridjsList$ = this.service.returnOrder$;
  }

}

