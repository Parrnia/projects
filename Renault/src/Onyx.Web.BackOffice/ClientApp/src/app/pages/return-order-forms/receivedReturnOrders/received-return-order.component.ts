
export interface ITab {
  id: number;
  title: string;
}

import { Component, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, of, switchAll } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmAllReturnOrderCommand, ReturnOrdersClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ReceivedReturnOrderGridService } from './received-return-order-grid.service';
import { NgbdReceivedReturnOrderSortableHeader, SortEvent } from './received-return-order-sortable.directive';
import { ReturnOrderModel } from '../return-order.model';
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";


@Component({
  selector: "app-received-return-order",
  templateUrl: "./received-return-order.component.html",
  styleUrls: ["./received-return-order.component.scss"],
  providers: [ReceivedReturnOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class ReceivedReturnOrderComponent {
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

  @ViewChildren(NgbdReceivedReturnOrderSortableHeader)
  returnOrders!: QueryList<NgbdReceivedReturnOrderSortableHeader>;
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmAllModel") confirmAllModel: any = [];
  @ViewChild("confirmSomeModel") confirmSomeModel: any = [];

  @ViewChild("confirmAllFormModal") confirmAllFormModal: any = [];
  @ViewChild("confirmSomeFormModal") confirmSomeFormModal: any = [];

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

  private modalRef: NgbModalRef | null = null;

  constructor(
    alertConfig: NgbAlertConfig,
    public client: ReturnOrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ReceivedReturnOrderGridService,
    private formBuilder: UntypedFormBuilder,
    private exportService: ExportFileService,
    private returnOrdersClient: ReturnOrdersClient
  ) {
    this.gridjsList$ = service.returnOrders$;
    this.total$ = service.total$;

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
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: 'سفارش بازگشت' },
      { label: 'لیست سفارشات بازگشت دریافت تاییدشده ', active: true }
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

  get confirmAllFormControl() {
    return this.confirmAllForm.controls;
  }

  get confirmSomeFormControl() {
    return this.confirmSomeForm.controls;
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }

  resetForm(type: string): void {
    switch (type) {
      case "confirmAllForm":
        this.confirmAllForm.reset();
        break;
      case "confirmSomeForm":
        this.confirmSomeForm.reset();
        break;
    }
  }

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
    // let cmd = new ConfirmAllReturnOrderCommand();
    // cmd.id = this.confirmAllFormControl.id.value;
    // cmd.details = this.confirmAllFormControl.details.value;
    
    this.client.confirmAll(this.selectedId, this.confirmAllForm.value).subscribe(result => {
      this.inProgress = false;
      if (result == null) {


        this.refreshGrid();
        Swal.fire({
          title: 'تایید همه کالاهای سفارش بازگشت با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        });
        this.modalService.dismissAll();
        this.confirmAllForm.reset();

      } else {

        Swal.fire({
          title: ' تایید همه کالاهای سفارش بازگشت با خطا مواجه شد',
          icon: 'error',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        });
        this.modalService.dismissAll();
      }
    }, error => {
      this.inProgress = false;
      console.error(error)
    });
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

  openMoreDetailReturnOrder(item: any) {
    this.selectedReturnOrder = item;
    this.returnOrdersClient
      .getReturnOrderInfo(item.id)
      .subscribe((c) => (this.selectedReturnOrderInfo = c));
    this.modalRef = this.modalService.open(this.productTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }

  closeMoreDetailReturnOrder() {
    this.service.refreshReturnOrders();
    this.modalRef?.dismiss();
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
      .exportExcelReceivedQuery(null, 1, null, null, null, null, null)
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

