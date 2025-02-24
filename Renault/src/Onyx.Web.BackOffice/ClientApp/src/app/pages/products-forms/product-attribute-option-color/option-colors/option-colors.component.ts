


import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { OptionColorsGridService } from "./option-colors-grid.service";
import { OptionColorsModel } from "./option-colors.model";
import { ProductOptionColorsClient, ProductOptionValueColorsClient } from "src/app/web-api-client";


@Component({
  selector: 'app-option-colors',
  templateUrl: './option-colors.component.html',
  styleUrls: ['./option-colors.component.scss'],
  providers: [OptionColorsGridService, DecimalPipe, NgbAlertConfig],
})
export class OptionColorsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OptionColorsModel;
  gridjsList$!: Observable<OptionColorsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  optionColorList?: any[];
  productAttributesList?: any[];
  optionColorValuesList?: any[];
  productAttributesGroupId?: any;
  productAttributes?:any;
  nano!: string;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;

  @Input() optionColor?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, 
    public client: ProductOptionColorsClient, 
    public clientValue: ProductOptionValueColorsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: OptionColorsGridService) {
    this.gridjsList$ = service.optionColors$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      productOptionColorId: [0, [Validators.required]],
      color: ['', [Validators.required]],
      name:  ['', [Validators.required]]
    });


  }

  ngOnInit(): void {

    this.service.getAllProductOptionColorsByColorId(this.optionColor.id);
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
      this.modalService.open(content, { centered: true });
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
    this.myForm.controls['color'].setValue(this.nano);
    this.myForm.controls['productOptionColorId'].setValue(this.optionColor.id);

    this.markAllControlsAsTouched(this.myForm);

    console.log('nano' , this.nano);
    debugger;
   
    this.submit = true;
    if (this.myForm.valid) {
      
      if (this.selectedId > 0) {
        this.clientValue.update(this.selectedId, this.myForm.value).subscribe(result => {
          this.inProgress = false;
          if (result == null) {
            this.service.getAllProductOptionColorsByColorId(this.optionColor.id);
            Swal.fire({
              title: 'ذخیره مقدار آپشن رنگ با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalRef?.close();
            this.myForm.reset();

          } else {

            Swal.fire({
              title: 'ذخیره مقدار آپشن رنگ با خطا مواجه شد',
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

      this.clientValue.create(this.myForm.value).subscribe(result => {
        this.inProgress = false;
        if (result > 0) {
          this.service.getAllProductOptionColorsByColorId(this.optionColor.id);
          Swal.fire({
            title: 'ذخیره مقدار آپشن رنگ با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.formModalRef?.close();
          this.myForm.reset();

        } else {
          Swal.fire({
            title: 'ذخیره مقدار آپشن رنگ با خطا مواجه شد',
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
    this.nano = '';
  }

  editMenu(item: any) {


    this.selectedId = item.id;
    
    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['productOptionColorId'].setValue(item.productOptionColorId ?? null);
    this.myForm.controls['color'].setValue(item.color ?? null);
    this.myForm.controls['name'].setValue(item.name ?? null);

    this.nano = item.color;

    this.formModalRef = this.modalService.open(this.formModal);

  }

  openInsertModal() {
    this.formModalRef = this.modalService.open(this.formModal);
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }


  deleteItems() {
    this.inProgress = true;
    this.clientValue.deleteRange([...this.checkedItems]).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف مقدار آپشن رنگ با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.getAllProductOptionColorsByColorId(this.optionColor.id);

      } else {

        Swal.fire({
          title: 'حذف مقدار آپشن رنگ با خطا مواجه شد',
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
    this.confirmationModalRef?.close();
    this.checkedItems = new Set<number>();
  }


  public handleCloseFormModal(){
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.selectedId = 0;
    this.nano = '';
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



