
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
import { CanceledReturnOrderGridService } from './canceled-return-order-grid.service';
import { NgbdCanceledReturnOrderSortableHeader, SortEvent } from './canceled-return-order-sortable.directive';
import { ReturnOrderModel } from '../return-order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-canceled-return-order",
  templateUrl: "./canceled-return-order.component.html",
  styleUrls: ["./canceled-return-order.component.scss"],
  providers: [CanceledReturnOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class CanceledReturnOrderComponent {
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

  @ViewChildren(NgbdCanceledReturnOrderSortableHeader)
  returnOrders!: QueryList<NgbdCanceledReturnOrderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];

  @ViewChild("completeModel") completeModel: any = [];

  @ViewChild("completeFormModal") completeFormModal: any = [];

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
    public service: CanceledReturnOrderGridService,
    private exportService: ExportFileService,
    private formBuilder: UntypedFormBuilder,
    private returnOrdersClient: ReturnOrdersClient
  ) {
    this.gridjsList$ = service.returnOrders$;
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
      { label: 'سفارش بازگشت' },
      { label: 'لیست سفارشات بازگشت لغوشده', active: true }
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

  get completeFormControl() {
    return this.completeForm.controls;
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }

  resetForm(type: string): void {
    switch (type) {
      case "completeForm":
        this.completeForm.reset();
        break;
    }
  }

  //#region Complete
  onSubmitCompleteForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.completeForm);
    this.submit = true;

    if (this.completeForm.valid) {
      this.completeForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.completeModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openCompleteReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.completeFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  completeReurnOrder() {
    this.inProgress = true;
    this.client.complete(this.selectedId, this.completeForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.refreshGrid();
          Swal.fire({
            title: "تکمیل فرایند سفارش بازگشت با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.completeForm.reset();
        } else {
          Swal.fire({
            title: " تکمیل فرایند سفارش بازگشت با خطا مواجه شد",
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
    this.client.exportExcelCanceledQuery(null, 1, null, null, null, null, null)
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

