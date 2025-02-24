
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
import { RegisteredOrderGridService } from './registered-order-grid.service';
import { NgbdRegisteredOrderSortableHeader, SortEvent } from './registered-order-sortable.directive';
import { OrderModel } from '../order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-registered-Order",
  templateUrl: "./registered-Order.component.html",
  styleUrls: ["./registered-Order.component.scss"],
  providers: [RegisteredOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class RegisteredOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  cancelForm!: FormGroup;
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
  @ViewChildren(NgbdRegisteredOrderSortableHeader)
  orders!: QueryList<NgbdRegisteredOrderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("confirmPaymentModel") confirmPaymentModel: any = [];
  @ViewChild("unConfirmPaymentModel") unConfirmPaymentModal: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("unSuccessfulPaymentModel") unSuccessfulPaymentModel: any = [];

  @ViewChild("cancelFormModal") cancelFormModal: any = [];
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
    public service: RegisteredOrderGridService,
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
    this.confirmPaymentForm = this.fb.group({
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

    this.unConfirmPaymentForm = this.fb.group({
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
      { label: "لیست سفارشات در انتظار تایید مالی", active: true },
    ];
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelRegisteredQuery(null, 1, null, null, null, null, null)
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

  get confirmPaymentFormControl() {
    return this.confirmPaymentForm.controls;
  }

  get unConfirmPaymentFormControl() {
    return this.unConfirmPaymentForm.controls;
  }

  resetForm(type: string): void {
    switch (type) {
      case "cancelForm":
        this.handleCloseFormModal(this.cancelForm);
        break;
      case "confirmPaymentForm":
        this.handleCloseFormModal(this.confirmPaymentForm);
        break;
      case "unConfirmPaymentForm":
        this.handleCloseFormModal(this.unConfirmPaymentForm);
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
      this.cancelForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.cancelForm.value);
      this.modalService.open(this.cancelModel, {
        size: "md",
        backdrop: "static",
      });
    }
  }

  onSubmitConfirmPaymentForm(): void {
    this.markAllControlsAsTouched(this.confirmPaymentForm);
    this.submit = true;
    if (this.confirmPaymentForm.valid) {
      this.confirmPaymentForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.confirmPaymentForm.value);
      this.modalService.open(this.confirmPaymentModel, {
        size: "md",
        backdrop: "static",
      });
    }
  }

  onSubmitUnConfirmPaymentForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.unConfirmPaymentForm);
    this.submit = true;
    if (this.unConfirmPaymentForm.valid) {
      this.inProgress = false;
      this.unConfirmPaymentForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.unConfirmPaymentForm.value);
      this.modalService.open(this.unConfirmPaymentModal, {
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

  openConfirmPaymentOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.confirmPaymentFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  openUnConfirmPaymentOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.unConfirmPaymentFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  cancelOrder() {
    this.client.cancel(this.selectedId, this.cancelForm.value).subscribe(
      (result) => {
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
      (error) => console.error(error)
    );
  }

  confirmPaymentOrder() {
    this.client
      .confirmPayment(this.selectedId, this.confirmPaymentForm.value)
      .subscribe(
        (result) => {
          if (result == null) {
            this.service.refreshOrders();
            Swal.fire({
              title: "تایید مالی سفارش با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.cancelForm.reset();
          } else {
            Swal.fire({
              title: " تایید مالی سفارش با خطا مواجه شد",
              icon: "error",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
          }
        },
        (error) => console.error(error)
      );
  }

  unConfirmPaymentOrder() {
    this.client
      .unConfirmPayment(this.selectedId, this.unConfirmPaymentForm.value)
      .subscribe(
        (result) => {
          if (result == null) {
            this.service.refreshOrders();
            Swal.fire({
              title: "عدم تایید مالی سفارش با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.cancelForm.reset();
          } else {
            Swal.fire({
              title: " عدم تایید مالی سفارش با خطا مواجه شد",
              icon: "error",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
          }
        },
        (error) => console.error(error)
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

