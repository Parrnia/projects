
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
import { CanceledOrderGridService } from './canceled-order-grid.service';
import { NgbdCanceledOrderSortableHeader, SortEvent } from './canceled-order-sortable.directive';
import { OrderModel } from '../order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-canceled-Order",
  templateUrl: "./canceled-Order.component.html",
  styleUrls: ["./canceled-Order.component.scss"],
  providers: [CanceledOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class CanceledOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  refundCostForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderModel;
  gridjsList$!: Observable<OrderModel[]>;
  total$: Observable<number>;
  inProgress = false;
  selectedOrder?: any;
  @ViewChildren(NgbdCanceledOrderSortableHeader)
  orders!: QueryList<NgbdCanceledOrderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("processModel") processModel: any = [];
  @ViewChild("unConfirmModel") unConfirmModal: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("unSuccessfulPaymentModel") unSuccessfulPaymentModel: any = [];

  @ViewChild("cancelFormModal") cancelFormModal: any = [];
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
  inProgressAllExportbtn = false;
  activeTab = 1;

  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: OrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: CanceledOrderGridService,
    private formBuilder: UntypedFormBuilder,
    private exportService: ExportFileService
  ) {
    this.gridjsList$ = service.orders$;
    this.total$ = service.total$;

    this.refundCostForm = this.fb.group({
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
      { label: "لیست سفارشات لغوشده ", active: true },
    ];
    
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelCanceledQuery(null, 1, null, null, null, null, null)
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

  onSubmitRefundCostForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.refundCostForm);

    this.submit = true;

    if (this.refundCostForm.valid) {
      this.refundCostForm.value.id = parseInt(this.selectedId);
      // Form submission logic goes here
      console.log("form value", this.refundCostForm.value);
      this.modalService.open(this.refundCostModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openRefundCostOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.refundCostFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  refundCostOrder() {
    this.inProgress = true;
    this.client
      .refundCost(this.selectedId, this.refundCostForm.value)
      .subscribe(
        (result) => {
          this.inProgress = false;
          if (result == null) {
            this.service.refreshOrders();
            Swal.fire({
              title: "بازگشت وجه سفارش با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.refundCostForm.reset();
          } else {
            Swal.fire({
              title: " بازگشت وجه سفارش با خطا مواجه شد",
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

