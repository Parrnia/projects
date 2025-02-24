import { Component, ElementRef, Input, ViewChild } from '@angular/core';

import { Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { PricesClient, ModelsClient, UpdatePriceCommand, CreatePriceCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { PriceGridService } from './price-grid.service';
import { PriceModel } from './price.model';
import * as moment from 'moment-jalaali';

@Component({
  selector: 'app-price',
  templateUrl: './price.component.html',
  styleUrls: ['./price.component.scss'],
  providers: [PriceGridService, DecimalPipe, NgbAlertConfig]
})
export class PriceComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: PriceModel;
  gridjsList$!: Observable<PriceModel[]>;
  total$: Observable<number>;
  inProgress = false;
  priceTypeId?: any;
  //@ViewChildren(NgbdPriceSortableHeader) prices!: QueryList<NgbdPriceSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  @Input() productOption?: any;
  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig,
    public client: PricesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: PriceGridService,
    public modelsClient: ModelsClient) {
    this.gridjsList$ = service.prices$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      date: ['', [Validators.required]],
      mainPrice: [0, [Validators.required]],
      productAttributeOptionId: [0, [Validators.required]],
    });

    alertConfig.type = 'success';
  }

  ngOnInit(): void {

    this.service.getAllprice(this.productOption?.id);
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
      this.confirmationModalRef = this.modalService.open(content, { centered: true });
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



  /**
  * Sort table data
  * @param param0 sort the column
  *
  */
  // onSort({ column, direction }: SortEvent) {
  //   // resetting other prices
  //   this.prices.forEach(price => {
  //     if (price.sortable !== column) {
  //       price.direction = '';
  //     }
  //   });

  //   this.service.sortColumn = column;
  //   this.service.sortDirection = direction;
  // }
  get form() {
    return this.myForm.controls;
  }


  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    this.myForm.controls['productAttributeOptionId'].setValue(this.productOption?.id ?? null);
    if (this.myForm.valid) {
      console.log('form value', this.myForm.value);
      const gregorianDate = moment(this.myForm.controls['date'].value, 'jYYYY/jMM/jDD').utcOffset(0, true).toDate();

      if (this.selectedId > 0) {
        this.myForm.controls['id'].setValue(this.selectedId);
        let cmd = new UpdatePriceCommand();
        cmd.id = this.myForm.controls['id'].value;
        cmd.date = gregorianDate;
        cmd.mainPrice = this.myForm.controls['mainPrice'].value;
        cmd.productAttributeOptionId = this.myForm.controls['productAttributeOptionId'].value;
        this.client.update(
          this.myForm.controls['id'].value,
          cmd
        ).subscribe(result => {
          this.inProgress = false;
          if (result == null) {
            this.service.getAllprice(this.productOption?.id);
            Swal.fire({
              title: 'ذخیره قیمت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalRef?.close();
            this.handleCloseFormModal();

          } else {

            Swal.fire({
              title: 'ذخیره قیمت با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalRef?.close();
          }
        }, error => {
          this.inProgress = false;
         console.error(error)
        });

      } else {
        let cmd = new CreatePriceCommand();
        cmd.date = gregorianDate;
        cmd.mainPrice = this.myForm.controls['mainPrice'].value;
        cmd.productAttributeOptionId = this.myForm.controls['productAttributeOptionId'].value;
        this.client.create(cmd).subscribe(result => {
          this.inProgress = false;
          if (result > 0) {
            this.service.getAllprice(this.productOption?.id);

            Swal.fire({
              title: 'ذخیره قیمت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.formModalRef?.close();
            this.handleCloseFormModal();

          } else {
            Swal.fire({
              title: 'ذخیره قیمت با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalRef?.close();
          }
        }, error => {
          this.inProgress = false;
         console.error(error)
        });
      }
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

  edit(item: PriceModel) {

    debugger;
    this.selectedId = item.id;

    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['date'].setValue(item.orginalDate ?? null);
    this.myForm.controls['mainPrice'].setValue(item.mainPrice ?? null);
    this.myForm.controls['productAttributeOptionId'].setValue(this.productOption?.id ?? null);

    debugger;
    this.myForm.controls['date'].disable();
    this.myForm.controls['mainPrice'].disable();
    this.formModalRef = this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });

  }

  openInsertModal() {
    this.formModalRef = this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    console.log('    this.selectedId = id;', this.selectedId);
    this.modalService.open(this.confirmationModal);
  }


  deleteItems() {
    this.inProgress = true;
    this.client.deleteRange([...this.checkedItems]).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف قیمت با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.getAllprice(this.productOption?.id);


      } else {

        Swal.fire({
          title: 'حذف قیمت با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.confirmationModalRef?.close();



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
    this.myForm.controls['date'].enable();
    this.myForm.controls['mainPrice'].enable();
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

