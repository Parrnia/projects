
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
import { CompletedOrderGridService } from './completed-order-grid.service';
import { NgbdCompletedOrderSortableHeader, SortEvent } from './completed-order-sortable.directive';
import { OrderModel } from '../order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-completed-Order",
  templateUrl: "./completed-Order.component.html",
  styleUrls: ["./completed-Order.component.scss"],
  providers: [CompletedOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class CompletedOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  completeForm!: FormGroup;
  processForm!: FormGroup;
  unConfirmForm!: FormGroup;
  refundCostForm!: FormGroup;
  unSuccessfulPaymentForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderModel;
  gridjsList$!: Observable<OrderModel[]>;
  total$: Observable<number>;
  selectedOrder?: any;
  @ViewChildren(NgbdCompletedOrderSortableHeader)
  orders!: QueryList<NgbdCompletedOrderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("completeModel") completeModel: any = [];
  @ViewChild("processModel") processModel: any = [];
  @ViewChild("unConfirmModel") unConfirmModal: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("unSuccessfulPaymentModel") unSuccessfulPaymentModel: any = [];

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
    public service: CompletedOrderGridService,
    private formBuilder: UntypedFormBuilder,
    private exportService: ExportFileService
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
      { label: "لیست سفارشات ارسال شده ", active: true },
    ];
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelCompletedQuery(null, 1, null, null, null, null, null)
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

  resetForm(type: string): void {
    switch (type) {
      case "completeForm":
        this.completeForm.reset();
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
    this.submit = true;

    if (this.completeForm.valid) {
      this.completeForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.completeForm.value);
      this.modalService.open(this.completeModel, {
        size: "md",
        backdrop: "static",
      });
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
    this.client.complete(this.selectedId, this.completeForm.value).subscribe(
      (result) => {
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

