
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { ReturnOrderItemsGridService } from "./return-order-item-grid.service";
import { ReturnOrderItemModel } from "./return-order-item.model";
import { CreateReturnOrderItemCommand, DeleteRangeReturnOrderItemCommand, ProductOptionColorsClient, ReturnOrderCustomerReasonType, ReturnOrderItemsClient, ReturnOrderOrganizationReasonType, UpdateReturnOrderItemCommand } from "src/app/web-api-client";

export type ReturnOrderCustomerReasonTypeKey = keyof typeof ReturnOrderCustomerReasonType;
export type ReturnOrderOrganizationReasonTypeKey = keyof typeof ReturnOrderOrganizationReasonType;

@Component({
  selector: 'app-return-order-item',
  templateUrl: './return-order-item.component.html',
  styleUrls: ['./return-order-item.component.scss'],
  providers: [ReturnOrderItemsGridService, DecimalPipe, NgbAlertConfig],
})
export class ReturnOrderItemsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderItemModel;

  inProgress = false;
  returnOrderCustomerReasonTypes = ReturnOrderCustomerReasonType;
  returnOrderOrganizationReasonTypes = ReturnOrderOrganizationReasonType;

  gridjsList$!: Observable<ReturnOrderItemModel[]>;
  total$: Observable<number>;
  @ViewChild('deleteItem') deleteItem: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('documentModal') documentModal: any = [];
  @Input() returnOrderItemGroup?: any;
  @Input() returnOrderId?: any;
  private modalRef: NgbModalRef | null = null;
  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ReturnOrderItemsClient,
    private fb: FormBuilder, private modalService: NgbModal, public service: ReturnOrderItemsGridService) {

    this.gridjsList$ = service.returnOrderItems$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      quantity: [0, [Validators.required]],
      details: ["", [Validators.required]],
      customerType: null,
      organizationType: null,
      isAccepted: false,
    });
  }

  ngOnInit(): void {

    this.service.getAllReturnOrderItemsByReturnOrderItemGroupId(this.returnOrderItemGroup.id);
  }

  openMoreDetail(item: any) {
    this.selectedItem = item;
    this.modalService.open(this.documentModal, { size: 'lg', backdrop: 'static' });
  }

  getCustomerNumericKeys(): ReturnOrderCustomerReasonTypeKey[] {
    return Object.entries(this.returnOrderCustomerReasonTypes)
      .filter(([key, value]) => typeof value === 'number')
      .map(([key, value]) => key as ReturnOrderCustomerReasonTypeKey);
  }

  mapCustomerNumericKeyToString(key: ReturnOrderCustomerReasonTypeKey): string {
    return this.mapCustomerNemberKeyToString(this.returnOrderCustomerReasonTypes[key].toString());
  }
  mapCustomerNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case ReturnOrderCustomerReasonType.CustomerCancelation:
        return 'انصراف مشتری';
      default:
        return '';
    }
  }

  getOrganizationNumericKeys(): ReturnOrderOrganizationReasonTypeKey[] {
    return Object.entries(this.returnOrderOrganizationReasonTypes)
      .filter(([key, value]) => typeof value === 'number')
      .map(([key, value]) => key as ReturnOrderOrganizationReasonTypeKey);
  }

  mapOrganizationNumericKeyToString(key: ReturnOrderOrganizationReasonTypeKey): string {
    return this.mapOrganizationNemberKeyToString(this.returnOrderOrganizationReasonTypes[key].toString());
  }
  mapOrganizationNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case ReturnOrderOrganizationReasonType.WrongProduct:
        return 'کالای اشتباه';
      case ReturnOrderOrganizationReasonType.DefectiveProduct:
        return 'کالای معیوب';
      case ReturnOrderOrganizationReasonType.IncompleteProduct:
        return 'کالای ناقص';
      case ReturnOrderOrganizationReasonType.Other:
        return 'سایر';
      default:
        return '';
    }
  }

  toggleCheckbox(itemId: number) {
    if (this.checkedItems.has(itemId)) {
      this.checkedItems.delete(itemId);
    } else {
      this.checkedItems.add(itemId);
    }
  }

  isChecked(itemId: number): boolean {
    return this.checkedItems.has(itemId);
  }

  deleteMultiple(content: any) {
    if ([...this.checkedItems].length > 0) {
      this.modalRef = this.modalService.open(content, { centered: true });
    }
    else {
      Swal.fire({
        title: 'حداقل یک مورد را انتخاب کنید!',
        icon: 'question',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

    }
  }

  get form() {
    return this.myForm.controls;
  }

  onSubmit(): void {
    debugger;
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    let bothNull = this.myForm.value.customerType == null && this.myForm.value.organizationType == null;
    let bothNotNull = this.myForm.value.customerType != null && this.myForm.value.organizationType != null;
    if (bothNull || bothNotNull) {
      Swal.fire({
        title: 'لطفا دلیل را درست انتخاب کنید',
        icon: 'question',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      });
      this.inProgress = false;
      return;
    }
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls['id'].setValue(this.selectedId);
        let cmd = new UpdateReturnOrderItemCommand();
        cmd.id = this.myForm.controls['id'].value;
        cmd.quantity = this.myForm.controls['quantity'].value;
        cmd.isAccepted = this.myForm.controls['isAccepted'].value;
        cmd.details = this.myForm.controls['details'].value;
        cmd.customerType = this.myForm.controls['customerType'].value;
        cmd.organizationType = this.myForm.controls['organizationType'].value;
        cmd.returnOrderItemGroupId = this.returnOrderItemGroup.id;
        cmd.returnOrderId = this.returnOrderId;

        this.client.update(
          this.myForm.controls['id'].value,
          cmd
        ).subscribe(result => {
          this.inProgress = false;
          if (result == null) {
            this.service.getAllReturnOrderItemsByReturnOrderItemGroupId(this.returnOrderItemGroup.id);
            Swal.fire({
              title: 'ذخیره آیتم سفارش بازگشت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalRef?.dismiss();
            this.handleCloseFormModal();

          } else {

            Swal.fire({
              title: 'ذخیره آیتم سفارش بازگشت با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalRef?.dismiss();
          }
        }, error => {
          this.inProgress = false;
          console.error(error);
        });

      } else {
        let cmd = new CreateReturnOrderItemCommand();
        cmd.quantity = +this.myForm.controls['quantity'].value;
        cmd.isAccepted = this.myForm.controls['isAccepted'].value;
        cmd.details = this.myForm.controls['details'].value;
        cmd.customerType = this.myForm.controls['customerType'].value;
        cmd.organizationType = this.myForm.controls['organizationType'].value;
        cmd.returnOrderItemGroupId = this.returnOrderItemGroup.id;
        cmd.returnOrderId = this.returnOrderId;
        debugger;
        this.client.create(cmd).subscribe(result => {
          this.inProgress = false;
          if (result > 0) {
            this.service.getAllReturnOrderItemsByReturnOrderItemGroupId(this.returnOrderItemGroup.id);
            Swal.fire({
              title: 'ذخیره آیتم سفارش بازگشت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.modalRef?.dismiss();
            this.handleCloseFormModal();

          } else {
            Swal.fire({
              title: 'ذخیره آیتم سفارش بازگشت با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalRef?.dismiss();
          }
        }, error => {
          this.inProgress = false;
          console.error(error)
        });
      }
    } else {
      this.inProgress = false;
    }
  }

  resetForm(): void {
    this.myForm.reset();
    Object.keys(this.myForm.controls).forEach(controlName => {
      const control = this.myForm.controls[controlName];
      control.markAsPristine();
      control.markAsUntouched();
    });
  }

  edit(item: ReturnOrderItemModel) {

    debugger;
    this.selectedId = item.id;

    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['quantity'].setValue(item.quantity ?? null);
    this.myForm.controls['isAccepted'].setValue(item.isAccepted ?? null);
    this.myForm.controls['details'].setValue(item.returnOrderReason.details ?? null);
    this.myForm.controls['customerType'].setValue(item.returnOrderReason.customerType ?? null);
    this.myForm.controls['organizationType'].setValue(item.returnOrderReason.organizationType ?? null);

    this.modalRef = this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });
  }

  openInsertModal() {
    this.modalRef = this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    this.modalService.open(this.deleteItem);
  }


  deleteItems() {
    this.inProgress = true;
    let cmd = new DeleteRangeReturnOrderItemCommand();
    cmd.returnOrderId = this.returnOrderId;
    cmd.returnOrderItemGroupId = this.returnOrderItemGroup.id;
    cmd.ids = [...this.checkedItems];
    this.client.deleteRange(cmd).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف آیتم سفارش بازگشت با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.modalRef?.dismiss();
        this.service.getAllReturnOrderItemsByReturnOrderItemGroupId(this.returnOrderItemGroup.id);

      } else {

        Swal.fire({
          title: 'حذف آیتم سفارش بازگشت با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.modalRef?.dismiss();


      }
    }, error => {
      this.inProgress = false;
      console.error(error)
    });

    this.checkedItems = new Set<number>();
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.selectedId = 0;
  }
  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }

}



