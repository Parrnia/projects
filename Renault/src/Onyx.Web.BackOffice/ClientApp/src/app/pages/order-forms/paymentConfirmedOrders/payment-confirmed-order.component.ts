
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
import { ConfirmPaymentOrderGridService } from './payment-confirmed-order-grid.service';
import { NgbdConfirmedPaymentOrderSortableHeader, SortEvent } from './payment-confirmed-order-sortable.directive';
import { OrderModel } from '../order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-payment-confirmed-Order",
  templateUrl: "./payment-confirmed-Order.component.html",
  styleUrls: ["./payment-confirmed-Order.component.scss"],
  providers: [ConfirmPaymentOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class ConfirmedPaymentOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  cancelForm!: FormGroup;
  confirmForm!: FormGroup;
  unConfirmForm!: FormGroup;
  refundCostForm!: FormGroup;
  unSuccessfulPaymentForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderModel;
  gridjsList$!: Observable<OrderModel[]>;
  total$: Observable<number>;
  inProgress = false;
  selectedOrder?: any;
  @ViewChildren(NgbdConfirmedPaymentOrderSortableHeader)
  orders!: QueryList<NgbdConfirmedPaymentOrderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("confirmedModel") confirmedModel: any = [];
  @ViewChild("unConfirmModel") unConfirmModal: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("unSuccessfulPaymentModel") unSuccessfulPaymentModel: any = [];

  @ViewChild("cancelFormModal") cancelFormModal: any = [];
  @ViewChild("confirmedFormModal") confirmedFormModal: any = [];
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
    public service: ConfirmPaymentOrderGridService,
    private exportService: ExportFileService,
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
    this.confirmForm = this.fb.group({
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

    this.unConfirmForm = this.fb.group({
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
      { label: " لیست سفارشات در انتظار تایید", active: true },
    ];
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelPaymentConfirmedQuery(null, 1, null, null, null, null, null)
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
  get cancelFormControl() {
    return this.cancelForm.controls;
  }

  get confirmFormControl() {
    return this.confirmForm.controls;
  }

  get unConfirmFormControl() {
    return this.unConfirmForm.controls;
  }

  resetForm(type: string): void {
    switch (type) {
      case "cancelForm":
        this.handleCloseFormModal(this.cancelForm);
        break;
      case "confirmForm":
        this.handleCloseFormModal(this.confirmForm);
        break;
      case "unConfirmForm":
        this.handleCloseFormModal(this.unConfirmForm);
        break;
    }
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }
  onSubmitCancelForm(): void {
    this.markAllControlsAsTouched(this.cancelForm);
    this.submit = true;
    if (this.cancelForm.valid) {
      this.inProgress = false;
      this.cancelForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.cancelForm.value);
      this.modalService.open(this.cancelModel, {
        size: "md",
        backdrop: "static",
      });
    } else {
      this.inProgress = false;
    }
  }

  onSubmitConfirmForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.confirmForm);
    this.submit = true;
    if (this.confirmForm.valid) {
      this.inProgress = false;
      this.confirmForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.confirmForm.value);
      this.modalService.open(this.confirmedModel, {
        size: "md",
        backdrop: "static",
      });
    } else {
      this.inProgress = false;
    }
  }

  onSubmitUnConfirmForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.unConfirmForm);
    this.submit = true;
    if (this.unConfirmForm.valid) {
      this.inProgress = false;
      this.unConfirmForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.unConfirmForm.value);
      this.modalService.open(this.unConfirmModal, {
        size: "md",
        backdrop: "static",
      });
    } else {
      this.inProgress = false;
    }
  }

  openCancelOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.cancelFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  openConfirmedOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.confirmedFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  openUnConfirmOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.unConfirmFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  cancelOrder() {
    this.inProgress = true;
    this.client.cancel(this.selectedId, this.cancelForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.service.refreshOrders();
          Swal.fire({
            title: "لغو سفارش با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.cancelForm.reset();
        } else {
          Swal.fire({
            title: " لغو سفارش با خطا مواجه شد",
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

  confirmedOrder() {
    debugger;
    this.inProgress = true;
    this.client.confirm(this.selectedId, this.confirmForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.service.refreshOrders();
          Swal.fire({
            title: "تایید سفارش با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.cancelForm.reset();
        } else {
          Swal.fire({
            title: " تایید سفارش با خطا مواجه شد",
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

  unConfirmOrder() {
    this.inProgress = true;
    this.client.unConfirm(this.selectedId, this.unConfirmForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.service.refreshOrders();
          Swal.fire({
            title: "عدم تایید سفارش با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.cancelForm.reset();
        } else {
          Swal.fire({
            title: " عدم تایید سفارش با خطا مواجه شد",
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

