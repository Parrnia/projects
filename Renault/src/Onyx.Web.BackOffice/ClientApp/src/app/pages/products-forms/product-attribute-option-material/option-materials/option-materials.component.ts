


import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { OptionMaterialsGridService } from "./option-materials-grid.service";
import { OptionMaterialsModel } from "./option-materials.model";
import { ProductOptionMaterialsClient, ProductOptionValueMaterialsClient } from "src/app/web-api-client";


@Component({
  selector: 'app-option-materials',
  templateUrl: './option-materials.component.html',
  styleUrls: ['./option-materials.component.scss'],
  providers: [OptionMaterialsGridService, DecimalPipe, NgbAlertConfig],
})
export class OptionMaterialsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OptionMaterialsModel;
  gridjsList$!: Observable<OptionMaterialsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  optionMaterialList?: any[];
  productAttributesList?: any[];
  optionMaterialValuesList?: any[];
  productAttributesGroupId?: any;
  productAttributes?:any;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;

  @Input() optionMaterial?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, 
    public client: ProductOptionMaterialsClient, 
    public clientValue: ProductOptionValueMaterialsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: OptionMaterialsGridService) {
    this.gridjsList$ = service.optionMaterials$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      productOptionMaterialId: [0, [Validators.required]],
      name:  ['', [Validators.required]]
    });


  }

  ngOnInit(): void {

    this.service.getAllProductOptionMaterialsByMaterialId(this.optionMaterial.id);
  }

  onSelectedMaterial(event:any) {
       
    if (event.target.value != undefined && event.target.value  != null && event.target.value  >0 )
    {
      var id = parseInt(event.target.value );
      this.getAllProductOptionValueByMaterialId(id);
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
    this.markAllControlsAsTouched(this.myForm);
    this.myForm.value.productOptionMaterialId = this.optionMaterial.id;
    this.submit = true;
    if (this.myForm.valid) {

      if (this.selectedId > 0) {
        this.clientValue.update(this.selectedId, this.myForm.value).subscribe(result => {
          this.inProgress = false;
          if (result == null) {
            this.service.getAllProductOptionMaterialsByMaterialId(this.optionMaterial.id);
            Swal.fire({
              title: 'ذخیره مقدار آپشن جنس با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalRef?.close();
            this.myForm.reset();

          } else {

            Swal.fire({
              title: 'ذخیره مقدار آپشن جنس با خطا مواجه شد',
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
          this.service.getAllProductOptionMaterialsByMaterialId(this.optionMaterial.id);
          Swal.fire({
            title: 'ذخیره مقدار آپشن جنس با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.formModalRef?.close();
          this.myForm.reset();

        } else {
          Swal.fire({
            title: 'ذخیره مقدار آپشن جنس با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.formModalRef?.close();
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
    Object.keys(this.myForm.controls).forEach(controlName => {
      const control = this.myForm.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
  }

  editMenu(item: any) {


    this.selectedId = item.id;
    

    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['productOptionMaterialId'].setValue(item.productOptionMaterialId ?? null);
    this.myForm.controls['name'].setValue(item.name ?? null);

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
      if (result == null) {
        Swal.fire({
          title: 'حذف مقدار آپشن جنس با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.getAllProductOptionMaterialsByMaterialId(this.optionMaterial.id);

      } else {

        Swal.fire({
          title: 'حذف مقدار آپشن جنس با خطا مواجه شد',
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


  public getAllProductOptionValueByMaterialId(id : number) {
   
    this.clientValue.getAllProductOptionValueByMaterialId(id).subscribe(result => {
      this.optionMaterialValuesList = result;
    }, error => console.error(error));

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



