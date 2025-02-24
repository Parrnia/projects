
export interface ITab {
  id: number;
  title: string;
}

import { Component, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, switchAll } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { OrdersClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { CostRefundedOrderGridService } from './cost-refunded-order-grid.service';
import { NgbdCostRefundedOrderSortableHeader, SortEvent } from './cost-refunded-order-sortable.directive';
import { OrderModel } from '../order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-cost-refunded-Order",
  templateUrl: "./cost-refunded-Order.component.html",
  styleUrls: ["./cost-refunded-Order.component.scss"],
  providers: [CostRefundedOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class CostRefundedOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  completeForm!: FormGroup;
  processForm!: FormGroup;
  unConfirmForm!: FormGroup;
  cancelForm!: FormGroup;
  refundCostForm!: FormGroup;
  unSuccessfulPaymentForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderModel;
  gridjsList$!: Observable<OrderModel[]>;
  total$: Observable<number>;
  inProgress = false;
  selectedOrder?: any;
  @ViewChildren(NgbdCostRefundedOrderSortableHeader)
  orders!: QueryList<NgbdCostRefundedOrderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("completeModel") completeModel: any = [];
  @ViewChild("processModel") processModel: any = [];
  @ViewChild("unConfirmModel") unConfirmModal: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("unSuccessfulPaymentModel") unSuccessfulPaymentModel: any = [];
  @ViewChild("cancelFormModal") cancelFormModal: any = [];
  @ViewChild("completeFormModal") completeFormModal: any = [];
  @ViewChild("processFormModal") processFormModal: any = [];
  @ViewChild("unConfirmFormModal") unConfirmFormModal: any = [];
  @ViewChild("refundCostFormModal") refundCostFormModal: any = [];
  @ViewChild("unSuccessfulPaymentFormModal") unSuccessfulPaymentFormModal: any =
    [];

  providerId?: any;
  countryId?: any;
  productTypeId?: any;
  productStatusId?: any;
  brandId?: any;
  productCategoryId?: any;
  availabilityId?: any;
  productAttributeTypeId?: null;
  formData = new FormData();
  activeTab = 1;
  inProgressAllExportbtn = false;

  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: OrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: CostRefundedOrderGridService,
    private formBuilder: UntypedFormBuilder
  ) {
    this.gridjsList$ = service.orders$;
    this.total$ = service.total$;

    this.cancelForm = this.fb.group({
      id: 0,
      details: [
        "",
        [
          Validators.required,
          Validators.maxLength(250),
          Validators.minLength(2),
        ],
      ],
    });
    this.completeForm = this.fb.group({
      id: 0,
      details: [
        "",
        [
          Validators.required,
          Validators.maxLength(250),
          Validators.minLength(2),
        ],
      ],
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: " سفارش" },
      { label: "لیست سفارشات که وجه بازگشت داده شده", active: true },
    ];

  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelCostRefundedQuery(null, 1, null, null, null, null, null)
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

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other brands
    this.orders.forEach((product) => {
      if (product.sortable !== column) {
        product.direction = "";
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
  get form() {
    return this.completeForm.controls;
  }
  get cancelFormControl() {
    return this.cancelForm.controls;
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }

  resetForm(type: string): void {
    switch (type) {
      case "cancelForm":
        this.cancelForm.reset();
        break;
      case "completeForm":
        this.completeForm.reset();
        break;
    }
  }

  onSubmitCompleteForm(): void {
    this.inProgress = true;
    this.submit = true;

    if (this.completeForm.valid) {
      this.inProgress = false;
      this.completeForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.completeForm.value);
      this.modalService.open(this.completeModel, {
        size: "md",
        backdrop: "static",
      });
    } else {
      this.inProgress = false;
    }
  }

  openCompleteOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.completeFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  completeOrder() {
    this.inProgress = true;
    this.client.complete(this.selectedId, this.completeForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.service.refreshOrders();
          Swal.fire({
            title: "تکمیل سفارش با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.completeForm.reset();
        } else {
          Swal.fire({
            title: " تکمیل سفارش با خطا مواجه شد",
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

  openMoreDetailOrder(item: any) {
    this.selectedOrder = item;
    this.modalService.open(this.productTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }
}

