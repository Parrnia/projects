
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
import { PreparedOrderGridService } from './prepared-order-grid.service';
import { NgbdPreparedOrderSortableHeader, SortEvent } from './prepared-order-sortable.directive';
import { OrderModel } from '../order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-prepared-Order",
  templateUrl: "./prepared-Order.component.html",
  styleUrls: ["./prepared-Order.component.scss"],
  providers: [PreparedOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class PreparedOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  cancelForm!: FormGroup;
  shipForm!: FormGroup;
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
  @ViewChildren(NgbdPreparedOrderSortableHeader)
  orders!: QueryList<NgbdPreparedOrderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("shipModel") shipModel: any = [];
  @ViewChild("unConfirmModel") unConfirmModal: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("unSuccessfulPaymentModel") unSuccessfulPaymentModel: any = [];

  @ViewChild("cancelFormModal") cancelFormModal: any = [];
  @ViewChild("shipFormModal") shipFormModal: any = [];
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
    public service: PreparedOrderGridService,
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
    this.shipForm = this.fb.group({
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
      { label: "لیست سفارشات در انتظار ارسال ", active: true },
    ];
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelPreparedQuery(null, 1, null, null, null, null, null)
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

  get shipFormControl() {
    return this.shipForm.controls;
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
      case "shipForm":
        this.shipForm.reset();
        break;
    }
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

  onSubmitShipForm(): void {
    this.markAllControlsAsTouched(this.shipForm);
    this.submit = true;

    if (this.shipForm.valid) {
      this.shipForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.shipForm.value);
      this.modalService.open(this.shipModel, {
        size: "md",
        backdrop: "static",
      });
    }
  }

  openCancelOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.cancelFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  openShipOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.shipFormModal, {
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

  shipOrder() {
    this.inProgress = true;
    this.client.ship(this.selectedId, this.shipForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.service.refreshOrders();
          Swal.fire({
            title: "ارسال سفارش با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.cancelForm.reset();
        } else {
          Swal.fire({
            title: " ارسال سفارش با خطا مواجه شد",
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

