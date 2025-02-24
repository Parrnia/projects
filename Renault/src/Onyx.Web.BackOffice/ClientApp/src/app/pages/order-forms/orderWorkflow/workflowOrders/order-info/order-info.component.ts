
import { EventEmitter, Input, Output } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { Component, ViewChild, ViewChildren } from '@angular/core';
import { OrdersClient } from "src/app/web-api-client";
import { OrderInfoModel } from './order-info.model';


@Component({
  selector: 'app-order-info',
  templateUrl: './order-info.component.html',
  styleUrls: ['./order-info.component.scss'],
  providers: [DecimalPipe, NgbAlertConfig],
})
export class OrderInfoComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  nano!: string;
  inProgress = false;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @Input() order?: OrderInfoModel;
  @Output() afterSubmit: EventEmitter<void> = new EventEmitter<void>();
  
  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: OrdersClient,
    private fb: FormBuilder, private modalService: NgbModal) {
    debugger;
    

    alertConfig.type = 'success';


  }

  ngOnInit(): void {
    this.myForm = this.fb.group({
      id: this.order?.id,
      addressDetails1: [this.order?.addressDetails1, [Validators.required]],
      addressDetails2: [this.order?.addressDetails2, [Validators.required]],
      postcode: [this.order?.postcode, [Validators.required]],
      phoneNumber: [this.order?.phoneNumber, [Validators.required]],
      customerFirstName: [this.order?.customerFirstName, [Validators.required]],
      customerLastName: [this.order?.customerLastName, [Validators.required]],
    });
  }
  get form() {
    return this.myForm.controls;
  }
  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    if (this.myForm.valid) {
      this.client.update(
        this.myForm.value.id,
        this.myForm.value
      ).subscribe(result => {
        this.inProgress = false;
        if (result == null) {
          this.afterSubmit.emit();
          Swal.fire({
            title: 'به روزرسانی اطلاعات سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.modalService.dismissAll();
        } else {
          Swal.fire({
            title: 'به روزرسانی اطلاعات سفارش با خطا مواجه شد',
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
    }else{
      this.inProgress = false;
    }
  }

  resetForm(): void {
    Object.keys(this.myForm.controls).forEach(controlName => {
      const control = this.myForm.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
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



