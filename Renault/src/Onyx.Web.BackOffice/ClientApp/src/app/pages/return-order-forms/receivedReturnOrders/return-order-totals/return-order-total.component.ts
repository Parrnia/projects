
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { ReturnOrderTotalModel } from "./return-order-total.model";
import { CreateReturnOrderTotalCommand, DeleteRangeReturnOrderTotalCommand, ProductOptionColorsClient, ReturnOrderTotalApplyType, ReturnOrderTotalsClient, ReturnOrderTotalType, UpdateReturnOrderTotalCommand } from "src/app/web-api-client";
import { ReturnOrderTotalsGridService } from "./return-order-total-grid.service";

export type ReturnOrderTotalTypeKey = keyof typeof ReturnOrderTotalType;
export type ReturnOrderTotalApplyTypeKey = keyof typeof ReturnOrderTotalApplyType;

@Component({
  selector: 'app-return-order-total',
  templateUrl: './return-order-total.component.html',
  styleUrls: ['./return-order-total.component.scss'],
  providers: [ReturnOrderTotalsGridService, DecimalPipe, NgbAlertConfig],
})
export class ReturnOrderTotalsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderTotalModel;


  inProgress = false;
  returnOrderTotalTypes = ReturnOrderTotalType;
  returnOrderTotalApplyTypes = ReturnOrderTotalApplyType;

  gridjsList$!: Observable<ReturnOrderTotalModel[]>;
  total$: Observable<number>;
  nano!: string;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('documentModal') documentModal: any = [];
  @Input() returnOrder?: any;
  private modalRef: NgbModalRef | null = null;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ReturnOrderTotalsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: ReturnOrderTotalsGridService) {
      
    this.gridjsList$ = service.returnOrderTotals$;
    this.total$ = service.total$;
    
    this.myForm = this.fb.group({
      id: 0,
      title: ["", [Validators.required]],
      price: [0, [Validators.required]],
      type: [null, [Validators.required]],
      returnOrderTotalApplyType: [null, [Validators.required]],
      returnOrderId: [0, [Validators.required]],
    });
  }

  ngOnInit(): void {
    this.service.getReturnOrderTotalsByReturnOrderId(this.returnOrder.id);
  }

  getNumericKeys(): ReturnOrderTotalTypeKey[] {
    return Object.entries(this.returnOrderTotalTypes)
      .filter(([key, value]) => typeof value === 'number')
      .map(([key, value]) => key as ReturnOrderTotalTypeKey);
  }

  mapNumericKeyToString(key: ReturnOrderTotalTypeKey): string {
    return this.mapNemberKeyToString(this.returnOrderTotalTypes[key].toString());
  }
  mapNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case ReturnOrderTotalType.Shipping:
        return 'ارسال';
      case ReturnOrderTotalType.ReturnShipping:
        return 'بازگشت';
      case ReturnOrderTotalType.ShippingAgain:
        return 'ارسال دوباره';
      case ReturnOrderTotalType.Tax:
        return 'مالیات';
      case ReturnOrderTotalType.TotalDiscount:
        return 'تخفیف کل';
      case ReturnOrderTotalType.Other:
        return 'سایر';
      default:
        return '';
    }
  }

  getApplyTypeNumericKeys(): ReturnOrderTotalApplyTypeKey[] {
    return Object.entries(this.returnOrderTotalApplyTypes)
      .filter(([key, value]) => typeof value === 'number')
      .map(([key, value]) => key as ReturnOrderTotalApplyTypeKey);
  }

  mapApplyTypeNumericKeyToString(key: ReturnOrderTotalApplyTypeKey): string {
    return this.mapApplyTypeNemberKeyToString(this.returnOrderTotalApplyTypes[key].toString());
  }
  mapApplyTypeNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case ReturnOrderTotalApplyType.ReduceFromTotal:
        return 'کم کردن از کل';
      case ReturnOrderTotalApplyType.AddToTotal:
        return 'اضافه کردن به کل';
      case ReturnOrderTotalApplyType.DoNothing:
        return 'نادیده گرفته شود';
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
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    this.myForm.controls['returnOrderId'].setValue(this.returnOrder.id);
    debugger;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        debugger;
        this.myForm.controls['id'].setValue(this.selectedId);
        let cmd = new UpdateReturnOrderTotalCommand();
        cmd.id = this.myForm.controls['id'].value;
        cmd.title = this.myForm.controls['title'].value;
        cmd.price = this.myForm.controls['price'].value;
        cmd.type = this.myForm.controls['type'].value;
        cmd.returnOrderTotalApplyType = this.myForm.controls['returnOrderTotalApplyType'].value;
        cmd.returnOrderId = this.myForm.controls['returnOrderId'].value;

        this.client.update(
           this.myForm.controls['id'].value,
           cmd
          ).subscribe(result => {
            this.inProgress = false;
          if (result == null) {

             
            this.service.getReturnOrderTotalsByReturnOrderId(this.returnOrder.id);
            Swal.fire({
              title: 'ذخیره هزینه جانبی سفارش بازگشت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalRef?.dismiss();
            this.handleCloseFormModal();

          } else {

            Swal.fire({
              title: 'ذخیره هزینه جانبی سفارش بازگشت با خطا مواجه شد',
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

      } else {
        this.myForm.controls['id'].setValue(this.selectedId);
        this.myForm.controls['returnOrderId'].setValue(this.returnOrder.id);
        let cmd = new CreateReturnOrderTotalCommand();
        cmd.title = this.myForm.controls['title'].value;
        cmd.price = this.myForm.controls['price'].value;
        cmd.type = this.myForm.controls['type'].value;
        cmd.returnOrderTotalApplyType = this.myForm.controls['returnOrderTotalApplyType'].value;
        cmd.returnOrderId = this.myForm.controls['returnOrderId'].value;
        this.client.create(cmd).subscribe(result => {
          this.inProgress = false;
          if (result > 0) {
            this.service.getReturnOrderTotalsByReturnOrderId(this.returnOrder.id);
            Swal.fire({
              title: 'ذخیره هزینه جانبی سفارش بازگشت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.modalRef?.dismiss();
            this.handleCloseFormModal();

          } else {
            Swal.fire({
              title: 'ذخیره هزینه جانبی سفارش بازگشت با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalRef?.dismiss();
          }
        }, error => {
          this.inProgress = false;
         console.error(error)});
      }
    }else{
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

  edit(item: ReturnOrderTotalModel) {

    debugger;
    this.selectedId = item.id;

    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['title'].setValue(item.title ?? null);
    this.myForm.controls['price'].setValue(item.price ?? null);
    this.myForm.controls['type'].setValue(item.type ?? null);
    this.myForm.controls['returnOrderTotalApplyType'].setValue(item.returnOrderTotalApplyType ?? null);

    this.modalRef = this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });
  }

  openInsertModal() {
    this.modalRef = this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }


  deleteItems() {
    this.inProgress = true;
    let cmd = new DeleteRangeReturnOrderTotalCommand();
    cmd.returnOrderId = this.returnOrder.id;
    cmd.ids = [...this.checkedItems];
    this.client.deleteRange(cmd).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف هزینه جانبی سفارش بازگشت با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.modalRef?.dismiss();
        this.service.getReturnOrderTotalsByReturnOrderId(this.returnOrder.id);
      } else {

        Swal.fire({
          title: 'حذف هزینه جانبی سفارش بازگشت با خطا مواجه شد',
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

  public handleCloseFormModal(){
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

  openMoreDetail(item: any)
  {
    this.selectedItem =item;
    this.modalService.open(this.documentModal, { size: 'lg', backdrop: 'static' });
  }
}



