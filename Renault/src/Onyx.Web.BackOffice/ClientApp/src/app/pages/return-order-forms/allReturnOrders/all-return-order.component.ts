
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
import { AllReturnOrderGridService } from './all-return-order-grid.service';
import { NgbdAllReturnOrderSortableHeader, SortEvent } from './all-return-order-sortable.directive';
import { ReturnOrderModel } from '../return-order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';
import { Router } from '@angular/router';

@Component({
  selector: "app-all-return-order",
  templateUrl: "./all-return-order.component.html",
  styleUrls: ["./all-return-order.component.scss"],
  providers: [AllReturnOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class AllReturnOrderComponent {
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

  @ViewChildren(NgbdAllReturnOrderSortableHeader)
  returnOrders!: QueryList<NgbdAllReturnOrderSortableHeader>;
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("acceptModel") acceptModel: any = [];
  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("completeModel") completeModel: any = [];
  @ViewChild("confirmAllModel") confirmAllModel: any = [];
  @ViewChild("confirmSomeModel") confirmSomeModel: any = [];
  @ViewChild("receiveModel") receiveModel: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("rejectModel") rejectModel: any = [];
  @ViewChild("sendModel") sendModel: any = [];

  @ViewChild("acceptFormModal") acceptFormModal: any = [];
  @ViewChild("cancelFormModal") cancelFormModal: any = [];
  @ViewChild("completeFormModal") completeFormModal: any = [];
  @ViewChild("confirmAllFormModal") confirmAllFormModal: any = [];
  @ViewChild("confirmSomeFormModal") confirmSomeFormModal: any = [];
  @ViewChild("receiveFormModal") receiveFormModal: any = [];
  @ViewChild("refundCostFormModal") refundCostFormModal: any = [];
  @ViewChild("rejectFormModal") rejectFormModal: any = [];
  @ViewChild("sendFormModal") sendFormModal: any = [];

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
    public service: AllReturnOrderGridService,
    private exportService: ExportFileService,
    private formBuilder: UntypedFormBuilder,
    private returnOrdersClient: ReturnOrdersClient,
    private router: Router
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

    this.confirmAllForm = this.fb.group({
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

    this.confirmSomeForm = this.fb.group({
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

    this.sendForm = this.fb.group({
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
      { label: "سفارش بازگشت" },
      { label: "لیست همه سفارشات بازگشت ", active: true },
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

  get cancelFormControl() {
    return this.cancelForm.controls;
  }

  get completeFormControl() {
    return this.completeForm.controls;
  }

  get confirmAllFormControl() {
    return this.confirmAllForm.controls;
  }

  get confirmSomeFormControl() {
    return this.confirmSomeForm.controls;
  }

  get receiveFormControl() {
    return this.receiveForm.controls;
  }

  get refundCostFormControl() {
    return this.refundCostForm.controls;
  }

  get rejectFormControl() {
    return this.rejectForm.controls;
  }

  get sendFormControl() {
    return this.sendForm.controls;
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
      case "cancelForm":
        this.cancelForm.reset();
        break;
      case "completeForm":
        this.completeForm.reset();
        break;
      case "confirmAllForm":
        this.confirmAllForm.reset();
        break;
      case "confirmSomeForm":
        this.confirmSomeForm.reset();
        break;
      case "receiveForm":
        this.receiveForm.reset();
        break;
      case "refundCostForm":
        this.refundCostForm.reset();
        break;
      case "rejectForm":
        this.rejectForm.reset();
        break;
      case "sendForm":
        this.sendForm.reset();
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

  //#region ConfirmAll
  onSubmitConfirmAllForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.confirmAllForm);
    this.submit = true;

    if (this.confirmAllForm.valid) {
      this.confirmAllForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.confirmAllModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openConfirmAllReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.confirmAllFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  confirmAllReurnOrder() {
    this.inProgress = true;
    this.client
      .confirmAll(this.selectedId, this.confirmAllForm.value)
      .subscribe(
        (result) => {
          this.inProgress = false;
          if (result == null) {
            this.refreshGrid();
            Swal.fire({
              title: "تایید همه کالاهای سفارش بازگشت با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.confirmAllForm.reset();
          } else {
            Swal.fire({
              title: " تایید همه کالاهای سفارش بازگشت با خطا مواجه شد",
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

  //#region ConfirmSome
  onSubmitConfirmSomeForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.confirmSomeForm);
    this.submit = true;

    if (this.confirmSomeForm.valid) {
      this.confirmSomeForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.confirmSomeModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openConfirmSomeReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.confirmSomeFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  confirmSomeReurnOrder() {
    this.inProgress = true;
    this.client
      .confirmSome(this.selectedId, this.confirmSomeForm.value)
      .subscribe(
        (result) => {
          this.inProgress = false;
          if (result == null) {
            this.refreshGrid();
            Swal.fire({
              title: "تایید بعضی کالاهای سفارش بازگشت با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.confirmSomeForm.reset();
          } else {
            Swal.fire({
              title: " تایید بعضی کالاهای سفارش بازگشت با خطا مواجه شد",
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

  //#region RefundCost
  onSubmitRefundCostForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.refundCostForm);
    this.submit = true;

    if (this.refundCostForm.valid) {
      this.refundCostForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.refundCostModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openRefundCostReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.refundCostFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  refundCostReurnOrder() {
    this.inProgress = true;
    this.client
      .refundCost(this.selectedId, this.refundCostForm.value)
      .subscribe(
        (result) => {
          this.inProgress = false;
          if (result == null) {
            this.refreshGrid();
            Swal.fire({
              title: "بازگشت وجه سفارش بازگشت با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.refundCostForm.reset();
          } else {
            Swal.fire({
              title: " بازگشت وجه سفارش بازگشت با خطا مواجه شد",
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

  //#region Send
  onSubmitSendForm(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.sendForm);
    this.submit = true;

    if (this.sendForm.valid) {
      this.sendForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.sendModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }
  }

  openSendReurnOrder(item: any) {
    this.selectedId = item.id;
    this.modalService.open(this.sendFormModal, {
      size: "md",
      backdrop: "static",
    });
  }

  sendReurnOrder() {
    this.inProgress = true;
    this.client.send(this.selectedId, this.sendForm.value).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          this.refreshGrid();
          Swal.fire({
            title: "تایید ارسال سفارش بازگشت با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.sendForm.reset();
        } else {
          Swal.fire({
            title: " تایید ارسال سفارش بازگشت با خطا مواجه شد",
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
      .exportExcelQuery(null, 1, null, null, null, null, null)
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
  navigateToReturnOrder(returOrderId: string) {
    this.router.navigate(["/returnOrderWorkflow", returOrderId]);
  }
}

