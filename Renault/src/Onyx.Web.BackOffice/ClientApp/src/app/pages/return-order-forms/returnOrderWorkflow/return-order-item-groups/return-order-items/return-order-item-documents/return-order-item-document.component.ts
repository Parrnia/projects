import { Component, ElementRef, Input, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ReturnOrdersClient, CountryDto, FileParameter, ModelDto, ModelsClient, UpdateReturnOrderItemDocumentCommand, CreateReturnOrderItemDocumentCommand, DeleteRangeReturnOrderItemDocumentCommand, ReturnOrderItemDocumentsClient, FileUploadMetadataDto, ReturnOrderDto } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ReturnOrderItemDocumentGridService } from './return-order-item-document-grid.service';
import { ReturnOrderItemDocumentModel } from './return-order-item-document.model';
import { NgbdReturnOrderItemDocumentSortableHeader, SortEvent } from './return-order-item-document-sortable.directive';
import { ImageService } from "src/app/core/services/image.service";
import { OrderItemModel } from 'src/app/pages/order-forms/allOrders/order-items/order-item.model';
import { ReturnOrderItemModel } from '../return-order-item.model';

@Component({
  selector: 'app-return-order-item-document',
  templateUrl: './return-order-item-document.component.html',
  styleUrls: ['./return-order-item-document.component.scss'],
  providers: [ReturnOrderItemDocumentGridService, DecimalPipe, NgbAlertConfig]
})
export class ReturnOrderItemDocumentComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderItemDocumentModel;
  gridjsList$!: Observable<ReturnOrderItemDocumentModel[]>;
  total$: Observable<number>;
  inProgress = false;
  selectedFileUrl: string | undefined = undefined;
  @ViewChildren(NgbdReturnOrderItemDocumentSortableHeader) returnOrderItemDocuments!: QueryList<NgbdReturnOrderItemDocumentSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  modelId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  @Input('returnOrderItem') returnOrderItem?: ReturnOrderItemModel;
  @Input('returnOrderItemGroupId') returnOrderItemGroupId?: number;
  @Input('returnOrder') returnOrder?: ReturnOrderDto;
  private modalRef: NgbModalRef | null = null;

  constructor(alertConfig: NgbAlertConfig,
    public client: ReturnOrderItemDocumentsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ReturnOrderItemDocumentGridService) {
    this.gridjsList$ = service.returnOrderItemDocuments$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      description: ['', [Validators.required]],
      image: ['', [Validators.required]],
    });

    alertConfig.type = 'success';


  }

  ngOnInit(): void {
    this.service.getAllReturnOrderItemDocumentsByReturnOrderItemId(
      this.returnOrderItem?.id ?? 0
    );
  }

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["image"].setValue(event.fileId);
    } else {
      this.myForm.controls["image"].setValue(null);
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
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls['id'].setValue(this.selectedId);
        let cmd = new UpdateReturnOrderItemDocumentCommand();
        cmd.id = this.myForm.controls['id'].value;
        cmd.image = this.myForm.controls['image'].value;
        cmd.description = this.myForm.controls['description'].value;
        cmd.returnOrderItemId = this.returnOrderItem?.id;
        cmd.returnOrderItemGroupId = this.returnOrderItemGroupId;
        cmd.returnOrderId = this.returnOrder!.id;
        this.client.update(
          this.myForm.controls['id'].value,
          cmd
        ).subscribe(result => {
          this.inProgress = false;
          if (result == null) {
            this.service.getAllReturnOrderItemDocumentsByReturnOrderItemId(this.returnOrderItem?.id ?? 0);
            Swal.fire({
              title: 'ذخیره مستند آیتم سفارش بازگشت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalRef?.dismiss();
            this.handleCloseFormModal();

          } else {

            Swal.fire({
              title: 'ذخیره مستند آیتم سفارش بازگشت با خطا مواجه شد',
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
        let cmd = new CreateReturnOrderItemDocumentCommand();
        cmd.image = this.myForm.controls['image'].value;
        cmd.description = this.myForm.controls['description'].value;
        cmd.returnOrderItemId = this.returnOrderItem?.id;
        cmd.returnOrderItemGroupId = this.returnOrderItemGroupId;
        cmd.returnOrderId = this.returnOrder!.id;
        this.client.create(cmd).subscribe(result => {
          this.inProgress = false;
          if (result > 0) {
            this.service.getAllReturnOrderItemDocumentsByReturnOrderItemId(this.returnOrderItem?.id ?? 0);
            Swal.fire({
              title: 'ذخیره مستند آیتم سفارش بازگشت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.modalRef?.dismiss();
            this.handleCloseFormModal();

          } else {
            Swal.fire({
              title: 'ذخیره مستند آیتم سفارش بازگشت با خطا مواجه شد',
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

  edit(item: ReturnOrderItemDocumentModel) {

    debugger;
    this.selectedId = item.id;

    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['image'].setValue(item.image ?? null);
    this.myForm.controls['description'].setValue(item.description ?? null);
    this.selectedFileUrl = item.imageSrc;
    this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });
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
    let cmd = new DeleteRangeReturnOrderItemDocumentCommand();
    cmd.returnOrderId = this.returnOrder!.id;
    cmd.returnOrderItemGroupId = this.returnOrderItemGroupId;
    cmd.returnOrderItemId = this.returnOrderItem?.id;;
    cmd.ids = [...this.checkedItems];
    this.client.deleteRange(cmd).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف مستند آیتم سفارش بازگشت با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.modalRef?.dismiss();
        this.service.getAllReturnOrderItemDocumentsByReturnOrderItemId(this.returnOrderItem?.id ?? 0);

      } else {

        Swal.fire({
          title: 'حذف مستند آیتم سفارش بازگشت با خطا مواجه شد',
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

