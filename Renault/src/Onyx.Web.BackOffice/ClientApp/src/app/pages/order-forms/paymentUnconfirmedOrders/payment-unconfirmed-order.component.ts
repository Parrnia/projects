
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
import { UnconfirmPaymentedOrderGridService } from './payment-unconfirmed-order-grid.service';
import { NgbdUnconfirmedPaymentOrderSortableHeader, SortEvent } from './payment-unconfirmed-order-sortable.directive';
import { OrderModel } from '../order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-payment-unconfirmed-Order",
  templateUrl: "./payment-unconfirmed-Order.component.html",
  styleUrls: ["./payment-unconfirmed-Order.component.scss"],
  providers: [UnconfirmPaymentedOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class UnconfirmedPaymentOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  completeForm!: FormGroup;
  confirmPaymentForm!: FormGroup;
  unConfirmPaymentForm!: FormGroup;
  refundCostForm!: FormGroup;
  unSuccessfulPaymentForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderModel;
  gridjsList$!: Observable<OrderModel[]>;
  total$: Observable<number>;
  inProgress = false;
  selectedOrder?: any;
  @ViewChildren(NgbdUnconfirmedPaymentOrderSortableHeader)
  orders!: QueryList<NgbdUnconfirmedPaymentOrderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("completeModel") completeModel: any = [];
  @ViewChild("confirmPaymentModel") confirmPaymentModel: any = [];
  @ViewChild("unConfirmPaymentModel") unConfirmPaymentModal: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("unSuccessfulPaymentModel") unSuccessfulPaymentModel: any = [];

  @ViewChild("completeFormModal") completeFormModal: any = [];
  @ViewChild("confirmPaymentFormModal") confirmPaymentFormModal: any = [];
  @ViewChild("unConfirmPaymentFormModal") unConfirmPaymentFormModal: any = [];
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
    public service: UnconfirmPaymentedOrderGridService,
    private formBuilder: UntypedFormBuilder
  ) {
    this.gridjsList$ = service.orders$;
    this.total$ = service.total$;

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
      { label: "لیست سفارشات با پرداخت تایید نشده", active: true },
    ];
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelPaymentUnconfirmedQuery(null, 1, null, null, null, null, null)
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
  get completeFormControl() {
    return this.completeForm.controls;
  }

  resetForm(type: string): void {
    switch (type) {
      case "completeForm":
        this.handleCloseFormModal(this.completeForm);
        break;
    }
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }

  onSubmitCompleteForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.completeForm);
    this.submit = true;
    if (this.completeForm.valid) {
      this.completeForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.completeForm.value);
      this.modalService.open(this.completeModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
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

  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control) => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }
}

