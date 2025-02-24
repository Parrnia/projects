import { Component, ElementRef, Input, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ReturnOrdersClient, CountryDto, FileParameter, ModelDto, ModelsClient, FileUploadMetadataDto, UpdateReturnOrderTotalDocumentCommand, CreateReturnOrderTotalDocumentCommand, DeleteRangeReturnOrderTotalDocumentCommand, ReturnOrderTotalDocumentsClient, ReturnOrderDto } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ReturnOrderTotalDocumentGridService } from './return-order-total-document-grid.service';
import { ReturnOrderTotalDocumentModel } from './return-order-total-document.model';
import { NgbdReturnOrderTotalDocumentSortableHeader, SortEvent } from './return-order-total-document-sortable.directive';
import { ImageService } from "src/app/core/services/image.service";
import { ReturnOrderTotalModel } from '../return-order-total.model';
import { WorkflowReturnOrderModel } from '../../workflowReturnOrders/workflow-return-order.model';

@Component({
  selector: 'app-return-order-total-document',
  templateUrl: './return-order-total-document.component.html',
  styleUrls: ['./return-order-total-document.component.scss'],
  providers: [ReturnOrderTotalDocumentGridService, DecimalPipe, NgbAlertConfig]
})
export class ReturnOrderTotalDocumentComponent {
  entityState: number = 0;
  breadCrumbTotals!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedTotal?: ReturnOrderTotalDocumentModel;
  gridjsList$!: Observable<ReturnOrderTotalDocumentModel[]>;
  total$: Observable<number>;
  modelsList?: ModelDto[];
  inProgress = false;
  selectedFileUrl: string | undefined = undefined;
  @ViewChildren(NgbdReturnOrderTotalDocumentSortableHeader) returnOrderTotalDocuments!: QueryList<NgbdReturnOrderTotalDocumentSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  modelId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  private modalRef: NgbModalRef | null = null;
  
  @Input('returnOrderTotal') returnOrderTotal?: ReturnOrderTotalModel;
  @Input('returnOrder') returnOrder?: WorkflowReturnOrderModel;
  constructor(alertConfig: NgbAlertConfig,
    public client: ReturnOrderTotalDocumentsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private imageService: ImageService,
    public service: ReturnOrderTotalDocumentGridService) {
    this.gridjsList$ = service.returnOrderTotalDocuments$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      description: ['', [Validators.required]],
      image :  ['', [Validators.required]],
    });

    alertConfig.type = 'success';


  }

  ngOnInit(): void {
this.service.getAllReturnOrderTotalDocumentsByReturnOrderTotalId(
  this.returnOrderTotal?.id ?? 0
);
  }
  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["image"].setValue(event.fileId);
    } else {
      this.myForm.controls["image"].setValue(null);
    }
  }

  toggleCheckbox(totalId: number) {
    if (this.checkedItems.has(totalId)) {
      this.checkedItems.delete(totalId);
    } else {
      this.checkedItems.add(totalId);
    }
  }

  isChecked(totalId: number): boolean {
    return this.checkedItems.has(totalId);
  }

  deleteMultiple(content: any) {
    if ([...this.checkedItems].length > 0) {
      this.modalRef = this.modalService.open(content, { centered: true });
    } else {
      Swal.fire({
        title: "حداقل یک مورد را انتخاب کنید!",
        icon: "question",
        iconHtml: "!",
        confirmButtonText: "تایید",
      });
    }
  }
  get form() {
    return this.myForm.controls;
  }

  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    debugger;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls['id'].setValue(this.selectedId);
        let cmd = new UpdateReturnOrderTotalDocumentCommand();
        cmd.id = this.myForm.controls['id'].value;
        cmd.image = this.myForm.controls['image'].value;
        cmd.description = this.myForm.controls['description'].value;
        cmd.returnOrderTotalId = this.returnOrderTotal?.id;
        cmd.returnOrderId = this.returnOrder!.id;
        this.client.update(
           this.myForm.controls['id'].value,
           cmd
          ).subscribe(result => {
            this.inProgress = false;
          if (result == null) {
            this.service.getAllReturnOrderTotalDocumentsByReturnOrderTotalId(this.returnOrderTotal?.id ?? 0);
            Swal.fire({
              title: 'ذخیره مستند هزینه جانبی سفارش بازگشت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalRef?.dismiss();
            this.handleCloseFormModal();

          } else {

            Swal.fire({
              title: 'ذخیره مستند هزینه جانبی سفارش بازگشت با خطا مواجه شد',
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
        let cmd = new CreateReturnOrderTotalDocumentCommand();
        cmd.image = this.myForm.controls['image'].value;
        cmd.description = this.myForm.controls['description'].value;
        cmd.returnOrderTotalId = this.returnOrderTotal?.id;
        cmd.returnOrderId = this.returnOrder!.id;
        this.client.create(cmd).subscribe(result => {
          this.inProgress = false;
          if (result > 0) {
            this.service.getAllReturnOrderTotalDocumentsByReturnOrderTotalId(this.returnOrderTotal?.id ?? 0);
            Swal.fire({
              title: 'ذخیره مستند هزینه جانبی سفارش بازگشت با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.modalRef?.dismiss();
            this.handleCloseFormModal();

          } else {
            Swal.fire({
              title: 'ذخیره مستند هزینه جانبی سفارش بازگشت با خطا مواجه شد',
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

  edit(item: ReturnOrderTotalDocumentModel) {

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
    let cmd = new DeleteRangeReturnOrderTotalDocumentCommand();
    cmd.returnOrderId = this.returnOrder!.id;
    cmd.returnOrderTotalId = this.returnOrderTotal?.id;;
    cmd.ids = [...this.checkedItems];
    this.client.deleteRange(cmd).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف مستند هزینه جانبی سفارش بازگشت با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.modalRef?.dismiss();
        this.service.getAllReturnOrderTotalDocumentsByReturnOrderTotalId(this.returnOrderTotal?.id ?? 0);

      } else {

        Swal.fire({
          title: 'حذف مستند هزینه جانبی سفارش بازگشت با خطا مواجه شد',
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

  
}

