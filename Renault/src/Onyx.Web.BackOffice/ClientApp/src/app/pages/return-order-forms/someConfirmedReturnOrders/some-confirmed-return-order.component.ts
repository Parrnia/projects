
export interface ITab {
  id: number;
  title: string;
}

import { Component, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, of, switchAll } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CostRefundType, ReturnOrdersClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { SomeConfirmedReturnOrderGridService } from './some-confirmed-return-order-grid.service';
import { NgbdSomeConfirmedReturnOrderSortableHeader, SortEvent } from './some-confirmed-return-order-sortable.directive';
import { ReturnOrderModel } from '../return-order.model';
export type CostRefundTypeKey = keyof typeof CostRefundType;
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";


@Component({
  selector: "app-some-confirmed-return-order",
  templateUrl: "./some-confirmed-return-order.component.html",
  styleUrls: ["./some-confirmed-return-order.component.scss"],
  providers: [SomeConfirmedReturnOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class SomeConfirmedReturnOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  refundCostForm!: FormGroup;

  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderModel;
  gridjsList$!: Observable<ReturnOrderModel[]>;
  total$: Observable<number>;
  selectedReturnOrder?: any;
  selectedReturnOrderInfo?: any;
  inProgress = false;
  costRefundTypes = CostRefundType;
  inProgressAllExportbtn = false;

  @ViewChildren(NgbdSomeConfirmedReturnOrderSortableHeader)
  returnOrders!: QueryList<NgbdSomeConfirmedReturnOrderSortableHeader>;
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];

  @ViewChild("refundCostFormModal") refundCostFormModal: any = [];


  formData = new FormData();
  activeTab = 1;

  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ReturnOrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: SomeConfirmedReturnOrderGridService,
    private exportService: ExportFileService,
    private formBuilder: UntypedFormBuilder,
    private returnOrdersClient: ReturnOrdersClient
  ) {
    this.gridjsList$ = service.returnOrders$;
    this.total$ = service.total$;

    this.refundCostForm = this.fb.group({
      id: 0,
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
      costRefundType: [null, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: 'سفارش بازگشت' },
      { label: 'لیست سفارشات بازگشت با بعضی کالاها تاییدشده', active: true }
    ];
  }

  getNumericKeys(): CostRefundTypeKey[] {
    return Object.entries(this.costRefundTypes)
      .filter(([key, value]) => typeof value === 'number')
      .map(([key, value]) => key as CostRefundTypeKey);
  }

  mapNumericKeyToString(key: CostRefundTypeKey): string {
    return this.mapNemberKeyToString(this.costRefundTypes[key].toString());
  }
  mapNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case CostRefundType.NotDetermined:
        return 'مشخص نشده';
      case CostRefundType.Cash:
        return 'نقد';
      case CostRefundType.Credit:
        return 'اعتباری';
      case CostRefundType.Online:
        return 'اینترنتی';
      default:
        return '';
    }
  }


  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other brands
    this.returnOrders.forEach((product) => {
      if (product.sortable !== column) {
        product.direction = "";
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }

  get refundCostFormControl() {
    return this.refundCostForm.controls;
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }

  resetForm(type: string): void {
    switch (type) {
      case "refundCostForm":
        this.refundCostForm.reset();
        break;
    }
  }

  //#region RefundCost
  onSubmitRefundCostForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.refundCostForm);
    this.submit = true;

    if (this.refundCostForm.valid) {
      this.refundCostForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.refundCostModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openRefundCostReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.refundCostFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  refundCostReurnOrder() {
    this.inProgress = true;
    this.client
      .refundCost(this.selectedId, this.refundCostForm.value)
      .subscribe(
        (result) => {
          this.inProgress = false;
          if (result == null) {
            this.refreshGrid();
            Swal.fire({
              title: "بازگشت وجه سفارش بازگشت با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.refundCostForm.reset();
          } else {
            Swal.fire({
              title: " بازگشت وجه سفارش بازگشت با خطا مواجه شد",
              icon: "error",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
          }
        },
        (error) => {
          this.inProgress = false;
          console.error(error);
        }
      );
  }
  //#endregion

  openMoreDetailReturnOrder(item: any) {
    this.selectedReturnOrder = item;
    this.returnOrdersClient
      .getReturnOrderInfo(item.id)
      .subscribe((c) => (this.selectedReturnOrderInfo = c));
    this.modalService.open(this.productTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }

  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control) => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }

  refreshGrid() {
    this.service.refreshReturnOrders();
    this.gridjsList$ = this.service.returnOrders$;
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelSomeConfirmedQuery(null, 1, null, null, null, null, null)
      .subscribe({
        next: (response) => {
          this.inProgressAllExportbtn = false;
          this.exportService.exportFile(response);
        },
        error: (error) => {
          this.inProgressAllExportbtn = false;
          console.error(error);
        },
      });
  }

  openGetDetailExportExcelModal() {
    this.modalService.open(this.getDetailExportTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }
}

