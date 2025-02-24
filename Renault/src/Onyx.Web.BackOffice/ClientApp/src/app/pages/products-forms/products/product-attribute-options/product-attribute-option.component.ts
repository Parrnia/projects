

import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { CreateProductAttributeOptionCommand, CreateProductAttributeOptionValueCommand, FileParameter, ProductAttributeOptionValuesClient, ProductAttributeOptionsClient, ProductImagesClient, ProductOptionColorsClient, ProductOptionMaterialsClient, ProductOptionValueColorsClient, ProductOptionValueMaterialsClient, ProductTypesClient, UpdateProductAttributeOptionCommand, UpdateProductAttributeOptionValueCommand } from "src/app/web-api-client";
import { ProductAttributeOptionGridService } from "./product-attribute-option-grid.service";
import { ProductAttributeOptionModel } from "./product-attribute-option.model";



@Component({
  selector: 'app-product-attribute-option',
  templateUrl: './product-attribute-option.component.html',
  styleUrls: ['./product-attribute-option.component.scss'],
  providers: [ProductAttributeOptionGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductAttributeOptionComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductAttributeOptionModel;
  gridjsList$!: Observable<ProductAttributeOptionModel[]>;
  total$: Observable<number>;
  inProgress = false;
  imageBaseUrl?: string;
  optionColorList?: any[];
  optionColorName?: string;
  colorOptionValue?: any;
  optionMaterialList?: any[];
  optionMaterialName?: string;
  materialOptionValue?: any;
  selectedOption?: any;
  isShownonDefaultOption: boolean = true;
  isShowDefaultOption: boolean = true;

  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('optionValueModal') optionValueModal: any = [];
  @ViewChild('optionPriceModal') optionPriceModal: any = [];
  @ViewChild('productAttributeOptionTabModal') productAttributeOptionModal: any = [];
  confirmationModalRef: NgbModalRef | undefined;
  formModalRef: NgbModalRef | undefined;
  optionValueModalRef: NgbModalRef | undefined;
  optionPriceModalRef: NgbModalRef | undefined;
  productAttributeOptionTabModalRef: NgbModalRef | undefined;

  @Input() product?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig,
    public client: ProductAttributeOptionsClient,
    public productAttributeOptionValuesClient: ProductAttributeOptionValuesClient,
    public optionColorsClient: ProductOptionColorsClient,
    public optionValueColorClient: ProductOptionValueColorsClient,
    public optionMaterialsClient: ProductOptionMaterialsClient,
    public optionValueMaterialsClient: ProductOptionValueMaterialsClient,
    private fb: FormBuilder, private modalService: NgbModal, public service: ProductAttributeOptionGridService,
    private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.productAttributeOption$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      totalCount: [0, [Validators.required]],
      safetyStockQty: [0, [Validators.required]],
      minStockQty: [0, [Validators.required]],
      maxStockQty: [0, [Validators.required]],
      maxSalePriceNonCompanyProductPercent: null,
      isDefault: false,
      colorOptionValue: null,
      materialOptionValue: null,
      productId: [this.product?.id, [Validators.required]],
    });


  }

  ngOnInit(): void {

    this.service.getAllProductAttributeOption(this.product.id);
    this.getProductOptionColors();
    this.getProductOptionMaterials();
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
    this.myForm.controls['productId'].setValue(this.product?.id);

    this.submit = true;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        let command = new UpdateProductAttributeOptionCommand();
        command.id = this.selectedId;
        command.totalCount = parseInt(this.myForm.controls['totalCount'].value);
        command.safetyStockQty = parseInt(this.myForm.controls['safetyStockQty'].value);
        command.minStockQty = parseInt(this.myForm.controls['minStockQty'].value);
        command.maxStockQty = parseInt(this.myForm.controls['maxStockQty'].value);
        command.maxSalePriceNonCompanyProductPercent = parseInt(this.myForm.controls['maxSalePriceNonCompanyProductPercent'].value);
        command.isDefault = this.myForm.controls['isDefault'].value;
        command.productId = this.product?.id;
        let values: UpdateProductAttributeOptionValueCommand[] = [];

        if (this.product.colorOptionId != null) {
          let cmd = new UpdateProductAttributeOptionValueCommand();
          cmd.name = this.optionColorName;
          cmd.value = this.colorOptionValue;
          values.push(cmd);
        }

        if (this.product.materialOptionId != null) {
          let cmd = new UpdateProductAttributeOptionValueCommand();
          cmd.name = this.optionMaterialName;
          cmd.value = this.materialOptionValue;
          values.push(cmd);
        }
        command.productAttributeOptionValues = values;
        debugger;
        this.client.update(this.selectedId, command).subscribe(result => {
          this.inProgress = false;
          if (result == null) {

            this.service.getAllProductAttributeOption(this.product.id);
            Swal.fire({
              title: 'ذخیره ویژگی آپشن با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalRef?.close();
            this.handleCloseFormModal();

          } else {

            Swal.fire({
              title: 'ذخیره ویژگی آپشن با خطا مواجه شد',
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
        let command = new CreateProductAttributeOptionCommand();
        command.totalCount = parseInt(this.myForm.controls['totalCount'].value);
        command.safetyStockQty = parseInt(this.myForm.controls['safetyStockQty'].value);
        command.minStockQty = parseInt(this.myForm.controls['minStockQty'].value);
        command.maxStockQty = parseInt(this.myForm.controls['maxStockQty'].value);
        command.maxSalePriceNonCompanyProductPercent = parseInt(this.myForm.controls['maxSalePriceNonCompanyProductPercent'].value);
        command.isDefault = this.myForm.controls['isDefault'].value;
        command.productId = this.product?.id;
        let values: CreateProductAttributeOptionValueCommand[] = [];

        if (this.product.colorOptionId != null) {
          let cmd = new CreateProductAttributeOptionValueCommand();
          cmd.name = this.optionColorName;
          cmd.value = this.colorOptionValue;
          values.push(cmd);
        }

        if (this.product.materialOptionId != null) {
          let cmd = new CreateProductAttributeOptionValueCommand();
          cmd.name = this.optionMaterialName;
          cmd.value = this.materialOptionValue;
          values.push(cmd);
        }
        command.productAttributeOptionValues = values;

        this.client.create(command).subscribe(result => {
          this.inProgress = false;
          if (result > 0) {
            this.service.getAllProductAttributeOption(this.product.id);
            Swal.fire({
              title: 'ذخیره ویژگی آپشن محصول با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.formModalRef?.close();
            this.handleCloseFormModal();

          } else {
            Swal.fire({
              title: 'ذخیره ویژگی آپشن محصول با خطا مواجه شد',
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

  onSelectOptionValue(data: any) {

    this.selectedOption = data;
    this.modalService.open(this.optionValueModal);
  }

  onSelectOptionPrice(data: any) {

    this.selectedOption = data;
    this.modalService.open(this.optionPriceModal);
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }

  editMenu(item: any) {
    this.selectedId = item.id;
    this.getProductAttributeOptionValues();
    debugger;
    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['totalCount'].setValue(item.totalCount ?? null);
    this.myForm.controls['safetyStockQty'].setValue(item.safetyStockQty ?? null);
    this.myForm.controls['minStockQty'].setValue(item.minStockQty ?? null);
    this.myForm.controls['maxStockQty'].setValue(item.maxStockQty ?? null);
    this.myForm.controls['maxSalePriceNonCompanyProductPercent'].setValue(item.maxSalePriceNonCompanyProductPercent ?? null);
    this.myForm.controls['isDefault'].setValue(item.isDefault ?? false);
    this.myForm.controls['colorOptionValue'].setValue(item.colorOptionValue ?? 'ندارد');
    this.myForm.controls['materialOptionValue'].setValue(item.materialOptionValue ?? 'ندارد');
    this.myForm.controls['productId'].setValue(this.product?.id ?? null);

    this.materialOptionValue = item.materialOptionValue ?? 'ندارد';
    this.colorOptionValue = item.colorOptionValue ?? 'ندارد';

    this.myForm.controls['totalCount'].disable();
    this.myForm.controls['safetyStockQty'].disable();
    this.myForm.controls['minStockQty'].disable();
    this.myForm.controls['maxStockQty'].disable();
    this.myForm.controls['maxSalePriceNonCompanyProductPercent'].disable();

    this.formModalRef = this.modalService.open(this.formModal);

  }

  openMoreDetail(item: any) {
    this.selectedOption = item;
    this.modalService.open(this.productAttributeOptionModal, { size: 'lg', backdrop: 'static' });
  }


  deleteItems() {
    this.inProgress = true;
    this.client.deleteRange([...this.checkedItems]).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف ویژگی آپشن محصول با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.getAllProductAttributeOption(this.product.id);

      } else {

        Swal.fire({
          title: 'حذف ویژگی آپشن محصول با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.modalService.dismissAll();


      }
    }, error => {
      this.inProgress = false;
      console.error(error)}
  );

    this.checkedItems = new Set<number>();
  }

  public getProductOptionColors() {
    if (this.product.colorOptionId != null) {
      this.optionColorsClient.getProductOptionColorById(this.product.colorOptionId).subscribe(result => {
        debugger;
        this.optionColorName = result.name;
        this.optionColorList = result.values;
      }, error => console.error(error));
    }
  }


  public getProductOptionMaterials() {
    if (this.product.materialOptionId != null) {
      this.optionMaterialsClient.getProductOptionMaterialById(this.product.materialOptionId).subscribe(result => {
        this.optionMaterialName = result.name;
        this.optionMaterialList = result.values;
      }, error => console.error(error));
    }
  }

  public getProductAttributeOptionValues() {
    this.productAttributeOptionValuesClient.getAllProductAttributeOptionValueByOptionId(this.selectedId!).subscribe(result => {
      this.materialOptionValue = result.find(c => c.name == this.optionMaterialName)?.value;
      this.colorOptionValue = result.find(c => c.name == this.optionColorName)?.value;
    }, error => console.error(error));
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.myForm.controls['totalCount'].enable();
    this.myForm.controls['safetyStockQty'].enable();
    this.myForm.controls['minStockQty'].enable();
    this.myForm.controls['maxStockQty'].enable();
    this.myForm.controls['maxSalePriceNonCompanyProductPercent'].enable();
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



