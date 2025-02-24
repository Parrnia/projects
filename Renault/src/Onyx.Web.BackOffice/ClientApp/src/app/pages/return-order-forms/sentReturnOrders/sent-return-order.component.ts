
export interface ITab {
  id: number;
  title: string;
}

import { Component, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, of, switchAll } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReturnOrdersClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { SentReturnOrderGridService } from './sent-return-order-grid.service';
import { NgbdSentReturnOrderSortableHeader, SortEvent } from './sent-return-order-sortable.directive';
import { ReturnOrderModel } from '../return-order.model';
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";



@Component({
  selector: "app-sent-return-order",
  templateUrl: "./sent-return-order.component.html",
  styleUrls: ["./sent-return-order.component.scss"],
  providers: [SentReturnOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class SentReturnOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  acceptForm!: FormGroup;
  cancelForm!: FormGroup;
  completeForm!: FormGroup;
  confirmAllForm!: FormGroup;
  confirmSomeForm!: FormGroup;
  receiveForm!: FormGroup;
  refundCostForm!: FormGroup;
  rejectForm!: FormGroup;
  sendForm!: FormGroup;

  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderModel;
  gridjsList$!: Observable<ReturnOrderModel[]>;
  total$: Observable<number>;
  selectedReturnOrder?: any;
  selectedReturnOrderInfo?: any;
  inProgress = false;
  inProgressAllExportbtn = false;

  @ViewChildren(NgbdSentReturnOrderSortableHeader)
  returnOrders!: QueryList<NgbdSentReturnOrderSortableHeader>;
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("receiveModel") receiveModel: any = [];

  @ViewChild("cancelFormModal") cancelFormModal: any = [];
  @ViewChild("receiveFormModal") receiveFormModal: any = [];

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

  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ReturnOrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: SentReturnOrderGridService,
    private formBuilder: UntypedFormBuilder,
    private exportService: ExportFileService,
    private returnOrdersClient: ReturnOrdersClient
  ) {
    this.gridjsList$ = service.returnOrders$;
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

    this.receiveForm = this.fb.group({
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
      { label: 'سفارش بازگشت' },
      { label: 'لیست سفارشات بازگشت ارسال شده', active: true }
    ];
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

  get cancelFormControl() {
    return this.cancelForm.controls;
  }

  get receiveFormControl() {
    return this.receiveForm.controls;
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
      case "receiveForm":
        this.receiveForm.reset();
        break;
    }
  }

  //#region Cancel
  onSubmitCancelForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.cancelForm);
    this.submit = true;

    if (this.cancelForm.valid) {
      this.cancelForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.cancelModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openCancelReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.cancelFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  cancelReurnOrder() {
    this.inProgress = true;
    this.client.cancel(this.selectedId, this.cancelForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.refreshGrid();
          Swal.fire({
            title: "انصراف از سفارش بازگشت با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.cancelForm.reset();
        } else {
          Swal.fire({
            title: " انصراف از سفارش بازگشت با خطا مواجه شد",
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

  //#region Receive
  onSubmitReceiveForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.receiveForm);
    this.submit = true;

    if (this.receiveForm.valid) {
      this.receiveForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.receiveModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openReceiveReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.receiveFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  receiveReurnOrder() {
    this.inProgress = true;
    this.client.receive(this.selectedId, this.receiveForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.refreshGrid();
          Swal.fire({
            title: "تایید دریافت کالاهای سفارش بازگشت با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.receiveForm.reset();
        } else {
          Swal.fire({
            title: " تایید دریافت کالاهای سفارش بازگشت با خطا مواجه شد",
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
      .exportExcelSentQuery(null, 1, null, null, null, null, null)
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

