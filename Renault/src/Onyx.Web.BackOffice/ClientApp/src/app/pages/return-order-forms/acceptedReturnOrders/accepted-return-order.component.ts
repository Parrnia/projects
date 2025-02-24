import { ImageService } from './../../../core/services/image.service';

export interface ITab {
  id: number;
  title: string;
}

import { Component, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, of, switchAll } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DocumentCommandForTotal, FileUploadMetadataDto, ReturnOrdersClient, ReturnOrderTransportationType, SendReturnOrderCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { AcceptedReturnOrderGridService } from './accepted-return-order-grid.service';
import { NgbdAcceptedReturnOrderSortableHeader, SortEvent } from './accepted-return-order-sortable.directive';
import { ReturnOrderModel } from '../return-order.model';
import { ReturnOrderTotalDocumentModel } from './return-order-total-document.model';
export type ReturnOrderTransportationTypeKey = keyof typeof ReturnOrderTransportationType;
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";


@Component({
  selector: "app-accepted-return-order",
  templateUrl: "./accepted-return-order.component.html",
  styleUrls: ["./accepted-return-order.component.scss"],
  providers: [AcceptedReturnOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class AcceptedReturnOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  cancelForm!: FormGroup;
  sendForm!: FormGroup;
  documentForm!: FormGroup;

  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderModel;
  gridjsList$!: Observable<ReturnOrderModel[]>;
  total$: Observable<number>;
  selectedReturnOrder?: any;
  selectedReturnOrderInfo?: any;
  inProgress = false;
  docbtnInProgress = false;
  inProgressCancelBtn = false;
  returnOrderTransportationTypes = ReturnOrderTransportationType;
  returnOrderTransportationTypeId?: any;
  inProgressAllExportbtn = false;

  @ViewChildren(NgbdAcceptedReturnOrderSortableHeader)
  returnOrders!: QueryList<NgbdAcceptedReturnOrderSortableHeader>;
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];

  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("sendModel") sendModel: any = [];
  @ViewChild("documentModel") documentModel: any = [];

  @ViewChild("cancelFormModal") cancelFormModal: any = [];
  @ViewChild("sendFormModal") sendFormModal: any = [];
  @ViewChild("documentFormModal") documentFormModal: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;

  formData = new FormData();
  activeTab = 1;

  documentList: ReturnOrderTotalDocumentModel[] = [];
  selectedFileUrl: string | undefined = undefined;
  checkedItems: Set<string> = new Set<string>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ReturnOrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: AcceptedReturnOrderGridService,
    private exportService: ExportFileService,
    private imageService: ImageService,
    private formBuilder: UntypedFormBuilder,
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
      returnOrderTransportationTypeId: [0, [Validators.required]],
      returnShippingPrice: null,
      isReturnShippingPriceEntered: [false],
    });

    this.documentForm = this.fb.group({
      id: 0,
      image: ["", [Validators.required]],
      description: ["", [Validators.required]],
    });

    this.sendForm
      .get("isReturnShippingPriceEntered")
      ?.valueChanges.subscribe((checked: boolean) => {
        this.toggleValidators(checked);
      });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "سفارش بازگشت" },
      { label: "لیست سفارشات بازگشت تاییدشده ", active: true },
    ];
  }

  toggleValidators(isChecked: boolean) {
    if (isChecked) {
      this.sendForm
        .get("returnShippingPrice")
        ?.setValidators([Validators.required]);
    } else {
      this.sendForm.get("returnShippingPrice")?.clearValidators();
    }

    this.sendForm.get("returnShippingPrice")?.updateValueAndValidity();
  }

  getNumericKeys(): ReturnOrderTransportationTypeKey[] {
    return Object.entries(this.returnOrderTransportationTypes)
      .filter(([key, value]) => typeof value === "number")
      .map(([key, value]) => key as ReturnOrderTransportationTypeKey);
  }

  mapNumericKeyToString(key: ReturnOrderTransportationTypeKey): string {
    return this.mapNemberKeyToString(
      this.returnOrderTransportationTypes[key].toString()
    );
  }
  mapNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case ReturnOrderTransportationType.NotDetermined:
        return "مشخص نشده";
      case ReturnOrderTransportationType.CustomerReturn:
        return "ارسال بر عهده مشتری";
      case ReturnOrderTransportationType.OrganizationReturn:
        return "ارسال بر عهده سازمان";
      case ReturnOrderTransportationType.OnLocation:
        return "سازمان کالا را در محل تحویل گرفته";
      default:
        return "";
    }
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

  get sendFormControl() {
    return this.sendForm.controls;
  }

  get documentFormControl() {
    return this.documentForm.controls;
  }

  resetForm(type: string): void {
    switch (type) {
      case "cancelForm":
        this.cancelForm.reset();
        break;
      case "sendForm":
        this.sendForm.reset();
        break;
    }
  }

  //#region Cancel
  onSubmitCancelForm(): void {
    this.inProgressCancelBtn = true;
    this.markAllControlsAsTouched(this.cancelForm);
    this.submit = true;

    if (this.cancelForm.valid) {
      this.cancelForm.value.id = parseInt(this.selectedId);
      this.modalService.open(this.cancelModel, {
        size: "md",
        backdrop: "static",
      });
      this.inProgressCancelBtn = false;
    } else {
      this.inProgressCancelBtn = false;
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
    debugger;
    if (
      this.sendFormControl.isReturnShippingPriceEntered.value &&
      (this.documentList.length === 0 || this.documentList === undefined)
    ) {
      this.inProgress = false;
      Swal.fire({
        title: "لطفا حداقل یک مستند برای هزینه بازگشت کالا وارد نمایید",
        icon: "error",
        iconHtml: "!",
        confirmButtonText: "تایید",
      });
    }
    let cmd = new SendReturnOrderCommand();
    debugger;
    cmd.id = this.selectedId;
    cmd.details = this.sendFormControl.details.value;
    cmd.returnOrderTransportationType = parseInt(
      this.sendFormControl.returnOrderTransportationTypeId.value
    );
    cmd.returnShippingPrice = this.sendFormControl.returnShippingPrice.value;
    if (cmd.returnShippingPrice != null) {
      debugger;
      cmd.documentCommandForTotals = this.documentList.map((c) => {
        let docCmd = new DocumentCommandForTotal();
        docCmd.image = c.image;
        docCmd.description = c.description;
        return docCmd;
      });
    } else {
      cmd.documentCommandForTotals = [];
    }
    if (this.sendForm.valid) {
      this.client.send(this.selectedId, cmd).subscribe(
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
    } else {
      this.inProgress = false;
    }
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

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.documentForm.controls["image"].setValue(event.fileId);
    } else {
      this.documentForm.controls["image"].setValue(null);
    }
  }

  toggleCheckbox(itemId: string) {
    if (this.checkedItems.has(itemId)) {
      this.checkedItems.delete(itemId);
    } else {
      this.checkedItems.add(itemId);
    }
  }

  isChecked(itemId: string): boolean {
    return this.checkedItems.has(itemId);
  }

  deleteMultiple(content: any) {
    if ([...this.checkedItems].length > 0) {
      this.modalService.open(content, { centered: true });
    } else {
      Swal.fire({
        title: "حداقل یک مورد را انتخاب کنید!",
        icon: "question",
        iconHtml: "!",
        confirmButtonText: "تایید",
      });
    }
  }

  onSubmit(): void {
    this.docbtnInProgress = true;
    this.markAllControlsAsTouched(this.documentForm);
    this.submit = true;
    if (this.documentForm.valid) {
      this.docbtnInProgress= false;
      let doc = new ReturnOrderTotalDocumentModel();
      doc.description = this.documentForm.controls["description"].value;
      doc.image = this.documentForm.controls["image"].value;
      doc.imageSrc = this.imageService.getImageSrcByIdTemp(doc.image);
      this.documentList.push(doc);
      this.handleCloseFormModalSecondLayer(this.documentForm)
    } else {
      this.docbtnInProgress = false;
    }
  }

  resetAddForm(form: FormGroup): void {
    Object.keys(form.controls).forEach((controlName) => {
      const control = form.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
  }

  openInsertModal() {
    this.modalService.open(this.documentFormModal, {
      size: "lg",
      backdrop: "static",
    });
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    let ids = [...this.checkedItems];
    this.documentList = this.documentList.filter(
      (record) => !ids.includes(record.image)
    );
    this.checkedItems = new Set<string>();
  }

  public handleCloseFormModal(form: FormGroup) {
    debugger;
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
    this.selectedId = 0;
    this.selectedFileUrl = undefined;
  }

  public handleCloseFormModalSecondLayer(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
    this.selectedFileUrl = undefined;
  }
  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelAcceptedQuery(null, 1, null, null, null, null, null)
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

