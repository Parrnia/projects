
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
import { RegisteredReturnOrderGridService } from './registered-return-order-grid.service';
import { NgbdRegisteredReturnOrderSortableHeader, SortEvent } from './registered-return-order-sortable.directive';
import { ReturnOrderModel } from '../return-order.model';
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";


@Component({
  selector: "app-registered-return-order",
  templateUrl: "./registered-return-order.component.html",
  styleUrls: ["./registered-return-order.component.scss"],
  providers: [RegisteredReturnOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class RegisteredReturnOrderComponent {
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

  @ViewChildren(NgbdRegisteredReturnOrderSortableHeader)
  returnOrders!: QueryList<NgbdRegisteredReturnOrderSortableHeader>;
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("acceptModel") acceptModel: any = [];
  @ViewChild("rejectModel") rejectModel: any = [];

  @ViewChild("acceptFormModal") acceptFormModal: any = [];
  @ViewChild("rejectFormModal") rejectFormModal: any = [];

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
    public service: RegisteredReturnOrderGridService,
    private formBuilder: UntypedFormBuilder,
    private exportService: ExportFileService,
    private returnOrdersClient: ReturnOrdersClient
  ) {
    this.gridjsList$ = service.returnOrders$;
    this.total$ = service.total$;

    this.acceptForm = this.fb.group({
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

    this.rejectForm = this.fb.group({
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
      { label: 'لیست سفارشات بازگشت ثبت شده ', active: true }
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

  get acceptFormControl() {
    return this.acceptForm.controls;
  }

  get rejectFormControl() {
    return this.rejectForm.controls;
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }

  resetForm(type: string): void {
    switch (type) {
      case "acceptForm":
        this.acceptForm.reset();
        break;
      case "rejectForm":
        this.rejectForm.reset();
        break;
    }
  }

  //#region Accept
  onSubmitAcceptForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.acceptForm);
    this.submit = true;

    if (this.acceptForm.valid) {
      this.acceptForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.acceptModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openAcceptReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.acceptFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  acceptReurnOrder() {
    this.inProgress = true;
    this.client.accept(this.selectedId, this.acceptForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.refreshGrid();
          Swal.fire({
            title: "پذیرفتن سفارش بازگشت با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.acceptForm.reset();
        } else {
          Swal.fire({
            title: " پذیرفتن سفارش بازگشت با خطا مواجه شد",
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

  //#region Reject
  onSubmitRejectForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.rejectForm);
    this.submit = true;

    if (this.rejectForm.valid) {
      this.rejectForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.rejectModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openRejectReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.rejectFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  rejectReurnOrder() {
    this.inProgress = true;
    this.client.reject(this.selectedId, this.rejectForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.refreshGrid();
          Swal.fire({
            title: "رد سفارش بازگشت با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.rejectForm.reset();
        } else {
          Swal.fire({
            title: " رد سفارش بازگشت با خطا مواجه شد",
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
}

