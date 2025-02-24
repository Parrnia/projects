

import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { FileParameter, ProductAttributeOptionRolesClient, ProductAttributeOptionsClient, ProductImagesClient, ProductOptionColorsClient, ProductOptionMaterialsClient, ProductTypesClient, UpdateProductAttributeOptionRoleCommand } from "src/app/web-api-client";
import { ProductOptionRoleGridService } from "./product-option-role-grid.service";
import { ProductOptionRoleModel } from "./product-option-role.model";



@Component({
  selector: 'app-product-option-role',
  templateUrl: './product-option-role.component.html',
  styleUrls: ['./product-option-role.component.scss'],
  providers: [ProductOptionRoleGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductOptionRoleComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductOptionRoleModel;
  gridjsList$!: Observable<ProductOptionRoleModel[]>;
  total$: Observable<number>;
  inProgress = false;
  imageBaseUrl?: string;
  optionRoleId: any;
  optionRoleList?: any[];
  customerTypeEnumId?: any;
  customerTypeList = [1,2,3,4];
  isShownonDefaultOption: boolean = true;
  isShowDefaultOption: boolean = true;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  selectedFile?: FileParameter;
  selectedFileName?: any;
  productAttributeOptionValueModels: any[] = [];
  @Input() productOption?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig,
    public optionRolesClient: ProductAttributeOptionRolesClient,
    private fb: FormBuilder, private modalService: NgbModal, public service: ProductOptionRoleGridService,
    private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.ProductOptionRole$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      MinimumStockToDisplayProductForThisCustomerTypeEnum: [0, [Validators.required]],
      mainMaxOrderQty: 0,
      mainMinOrderQty: 0,
      customerTypeEnumId: 0,
      discountPercent: 0,
      productAttributeOptionId: 0,
    });


  }

  ngOnInit(): void {

    this.service.getAllProductAttributeOptionRoles(this.productOption.id);
  }

  public getCustomerTypeNameByEnglishName(name: string) {
    switch (name) {
      case 'Personal':
        return 'شخصی';
      case 'Store':
        return 'فروشگاهی';
      case 'Agency':
        return 'نمایندگی';
      case 'CentralRepairShop':
        return 'تعمیرگاه مرکزی';
      default:
        return '';
    }
  }
  public getCustomerTypeName(id: number) {
    switch (id) {
      case 1:
        return 'شخصی';
      case 2:
        return 'فروشگاهی';
      case 3:
        return 'نمایندگی';
      case 4:
        return 'تعمیرگاه مرکزی';
      default:
        return '';
    }
  }

  public getCustomerTypeId(name: string) {
    switch (name) {
      case 'Personal':
        return '1';
      case 'Store':
        return '2';
      case 'Agency':
        return '3';
      case 'CentralRepairShop':
        return '4';
      default:
        return '';
    }
  }

  public getCustomerTypeIdByPersianName(name: string) {
    switch (name) {
      case 'شخصی':
        return '1';
      case 'فروشگاهی':
        return '2';
      case 'نمایندگی':
        return '3';
      case 'تعمیرگاه مرکزی':
        return '4';
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


  get form() {
    return this.myForm.controls;
  }


  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.myForm.controls['productAttributeOptionId'].setValue(this.productOption.id ?? null);

    this.submit = true;
    if (this.myForm.valid) {
      console.log("productId", this.productOption);
      if (this.selectedId > 0) {
        debugger;
        let cmd = new UpdateProductAttributeOptionRoleCommand();
        cmd.id = this.selectedId;
        cmd.discountPercent = parseInt(this.myForm.controls['discountPercent'].value);
        cmd.mainMaxOrderQty = parseInt(this.myForm.controls['mainMaxOrderQty'].value);
        cmd.mainMinOrderQty = parseInt(this.myForm.controls['mainMinOrderQty'].value);
        cmd.minimumStockToDisplayProductForThisCustomerTypeEnum = parseInt(this.myForm.controls['MinimumStockToDisplayProductForThisCustomerTypeEnum'].value);
        cmd.customerTypeEnumId = parseInt(this.myForm.controls['customerTypeEnumId'].value);
        cmd.productAttributeOptionId = this.productOption?.id;
        this.optionRolesClient.update(
          this.selectedId, 
          cmd).subscribe(result => {
            this.inProgress = false;
          if (result == null) {
            this.service.getAllProductAttributeOptionRoles(this.productOption.id);
            Swal.fire({
              title: 'ذخیره داده آپشن محصول با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalRef?.close();
            this.handleCloseFormModal();

          } else {

            Swal.fire({
              title: 'ذخیره داده آپشن محصول با خطا مواجه شد',
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
        this.optionRolesClient.create(this.myForm.value).subscribe(result => {
          this.inProgress = false;
          if (result > 0) {
            this.service.getAllProductAttributeOptionRoles(this.productOption.id);
            Swal.fire({
              title: 'ذخیره داده آپشن محصول با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.formModal.nativeElement.click();

            this.formModalRef?.close();
            this.handleCloseFormModal();

          } else {
            Swal.fire({
              title: 'ذخیره داده آپشن محصول با خطا مواجه شد',
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


  openInsertModal() {
    this.formModalRef = this.modalService.open(this.formModal);
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }


  deleteItems() {
    this.inProgress = true;
    this.optionRolesClient.deleteRange([...this.checkedItems]).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف داده آپشن محصول با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.getAllProductAttributeOptionRoles(this.productOption.id);

      } else {

        Swal.fire({
          title: 'حذف داده آپشن محصول با خطا مواجه شد',
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



  editMenu(item: any) {
    this.selectedId = item.id;
    debugger;
    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['MinimumStockToDisplayProductForThisCustomerTypeEnum'].setValue(item.MinimumStockToDisplayProductForThisCustomerTypeEnum ?? 0);
    this.myForm.controls['mainMaxOrderQty'].setValue(item.mainMaxOrderQty ?? 0);
    this.myForm.controls['mainMinOrderQty'].setValue(item.mainMinOrderQty ?? 0);
    this.myForm.controls['customerTypeEnumId'].setValue(this.getCustomerTypeIdByPersianName(item.customerTypeEnumName ?? ''));
    this.myForm.controls['discountPercent'].setValue(item.discountPercent ?? 0);
    this.myForm.controls['productAttributeOptionId'].setValue(item.productAttributeOptionId ?? null);
   
    this.customerTypeEnumId = this.getCustomerTypeIdByPersianName(item.customerTypeEnumName ?? '');

    this.myForm.controls['mainMaxOrderQty'].disable();
    this.myForm.controls['mainMinOrderQty'].disable();
    this.myForm.controls['customerTypeEnumId'].disable();
    this.formModalRef = this.modalService.open(this.formModal);
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.myForm.controls['mainMaxOrderQty'].enable();
    this.myForm.controls['mainMinOrderQty'].enable();
    this.myForm.controls['customerTypeEnumId'].enable();
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



