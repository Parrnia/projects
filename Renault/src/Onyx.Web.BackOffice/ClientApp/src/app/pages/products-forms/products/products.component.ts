import { AllProductBrandDropDownDto, AllProductOptionColorDropDownDto, AllProductOptionMaterialDropDownDto, UpdateProductCommand } from './../../../web-api-client';

export interface ITab {
  id: number;
  title: string;
}

import { Component, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, forkJoin } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AllCountingUnitDropDownDto, AllCountryDropDownDto, AllProductAttributeTypeDropDownDto, AllProductCategoryDropDownDto, AllProductStatusDropDownDto, AllProductTypeDropDownDto, AllProviderDropDownDto, ProductBrandsClient, CountingUnitsClient, CountriesClient, CountryDto, FileParameter, ProductAttributeTypeDto, ProductAttributeTypesClient, ProductCategoriesClient, ProductCategoryDto, ProductOptionColorsClient, ProductOptionMaterialsClient, ProductStatusDto, ProductStatusesClient, ProductTypeDto, ProductTypesClient, ProductsClient, ProviderDto, ProvidersClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ProductsGridService } from './products-grid.service';
import { ProductModel } from './products.model';
import { NgbdProductsSortableHeader, SortEvent } from './products-sortable.directive';
import { ProductValidators } from './products-validators';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-products",
  templateUrl: "./products.component.html",
  styleUrls: ["./products.component.scss"],
  providers: [ProductsGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductsComponent {
  selectedFile?: FileParameter;
  selectedFileName?: any;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductModel;
  gridjsList$!: Observable<ProductModel[]>;
  total$: Observable<number>;
  inProgress = false;
  providerList?: AllProviderDropDownDto[];
  productTypeList?: AllProductTypeDropDownDto[];
  countryList?: AllCountryDropDownDto[];
  compatibilityList?: any[];
  brandList?: AllProductBrandDropDownDto[];
  productCategoryList?: AllProductCategoryDropDownDto[];
  productStatusList?: AllProductStatusDropDownDto[];
  poductAttributeTypeList?: AllProductAttributeTypeDropDownDto[];
  countingUnitsList?: AllCountingUnitDropDownDto[];
  productOptionMaterialsList?: AllProductOptionMaterialDropDownDto[];
  productOptionColorsList?: AllProductOptionColorDropDownDto[];
  selectedProduct?: any;
  @ViewChildren(NgbdProductsSortableHeader)
  products!: QueryList<NgbdProductsSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("productTabModel") productTabModel: any = [];

  providerId?: any;
  countryId?: any;
  productTypeId?: any;
  productStatusId?: any;
  brandId?: any;
  productCategoryId?: any;
  commonCountingUnitId?: any;
  mainCountingUnitId?: any;
  availabilityId?: any;
  productAttributeTypeId?: any;
  productOptionColorId?: any;
  productOptionMaterialId?: any;
  compatibility?: any;
  formData = new FormData();
  activeTab = 1;
  inProgressAllExportbtn = false;

  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ProductsGridService,
    private exportService: ExportFileService,
    public providerClient: ProvidersClient,
    public productTypesClient: ProductTypesClient,
    public productStatusesClient: ProductStatusesClient,
    public countryClient: CountriesClient,
    public brandsClient: ProductBrandsClient,
    public productCategoryClient: ProductCategoriesClient,
    public productAttributeTypeClient: ProductAttributeTypesClient,
    private countingUnitsClient: CountingUnitsClient,
    private productOptionColorsClient: ProductOptionColorsClient,
    private productOptionMaterialsClient: ProductOptionMaterialsClient,
    private formBuilder: UntypedFormBuilder
  ) {
    this.gridjsList$ = service.products$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      name: ["", [Validators.required, Validators.maxLength(100)]],
      localizedName: ["", [Validators.required, Validators.maxLength(100)]],
      code: [null, [Validators.required]],
      productNo: ["", [Validators.required, Validators.maxLength(70)]],
      oldProductNo: ["", [Validators.required, Validators.maxLength(70)]],
      productCatalog: "",
      orderRate: [null, [Validators.required]],
      mileage: 0,
      duration: 0,
      excerpt: ["", [Validators.required]],
      description: ["", [Validators.required]],
      sku: "",
      providerId: 0,
      countryId: 0,
      productTypeId: 0,
      productStatusId: 0,
      mainCountingUnitId: 0,
      commonCountingUnitId: 0,
      brandId: [0, [Validators.required]],
      productCategoryId: [0, [Validators.required]],
      productAttributeTypeId: [0, [Validators.required]],
      productOptionColorId: null,
      productOptionMaterialId: null,
      compatibility: [null, [Validators.required]],
      isActive: false,
    });

    this.form.name.addAsyncValidators(
      ProductValidators.validProductName(this.client, 0)
    );
    this.form.localizedName.addAsyncValidators(
      ProductValidators.validProductLocalizedName(this.client, 0)
    );

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        ProductValidators.validProductName(this.client, id != null ? id : 0)
      );
      this.form.localizedName.setAsyncValidators(
        ProductValidators.validProductLocalizedName(
          this.client,
          id != null ? id : 0
        )
      );
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه محصول" },
      { label: "محصولات ", active: true },
    ];
    this.compatibilityList = [
      {
        id: 0,
        name: "سازگار با همه",
      },
      {
        id: 1,
        name: "ناشناخته",
      },
      {
        id: 2,
        name: "سازگار با بعضی محصولات",
      },
    ];
  }

  onFileSelected(event: any): void {
    var file = event.target.files[0];
    this.selectedFile = { data: file, fileName: file.name };
    this.selectedFileName = file.name;
    // this.formData.append('brandLogo',   this.selectedFile);

    console.log(this.selectedFile);
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
    } else {
      Swal.fire({
        title: "حداقل یک مورد را انتخاب کنید!",
        icon: "question",
        iconHtml: "!",
        confirmButtonText: "تایید",
      });
    }
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelQuery(null, 1, null, null, null, null, null)
      .subscribe({
        next: (response) => {
          this.inProgressAllExportbtn = false;
          this.exportService.exportFile(response);
        },
        error: (error) => {
          this.inProgressAllExportbtn = false;
          console.error(error);
        },
      });
  }

  openGetDetailExportExcelModal() {
    this.modalService.open(this.getDetailExportTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other brands
    this.products.forEach((product) => {
      if (product.sortable !== column) {
        product.direction = "";
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
  get form() {
    return this.myForm.controls;
  }

  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    this.myForm.value.providerId = parseInt(this.providerId);
    this.myForm.value.countryId = parseInt(this.countryId);
    this.myForm.value.productTypeId = parseInt(this.productTypeId);
    this.myForm.value.productStatusId = parseInt(this.productStatusId);
    this.myForm.value.mainCountingUnitId = parseInt(this.mainCountingUnitId);
    this.myForm.value.commonCountingUnitId = parseInt(
      this.commonCountingUnitId
    );
    this.myForm.value.brandId = parseInt(this.brandId);
    this.myForm.value.productCategoryId = parseInt(this.productCategoryId);
    this.myForm.value.productAttributeTypeId = parseInt(
      this.productAttributeTypeId
    );
    this.myForm.value.productOptionMaterialId = parseInt(
      this.productOptionMaterialId
    );
    this.myForm.value.productOptionColorId = parseInt(
      this.productOptionColorId
    );
    this.myForm.value.compatibility = parseInt(this.compatibility);

    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);

        let cmd = new UpdateProductCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.code = this.myForm.controls["code"].value;
        cmd.productNo = this.myForm.controls["productNo"].value;
        cmd.oldProductNo = this.myForm.controls["oldProductNo"].value;
        cmd.productCatalog = this.myForm.controls["productCatalog"].value;
        cmd.orderRate = this.myForm.controls["orderRate"].value;
        cmd.mileage = this.myForm.controls["mileage"].value;
        cmd.duration = this.myForm.controls["duration"].value;
        cmd.excerpt = this.myForm.controls["excerpt"].value;
        cmd.description = this.myForm.controls["description"].value;
        cmd.sku = this.myForm.controls["sku"].value;
        cmd.providerId = this.myForm.controls["providerId"].value;
        cmd.countryId = this.myForm.controls["countryId"].value;
        cmd.productTypeId = this.myForm.controls["productTypeId"].value;
        cmd.productStatusId = this.myForm.controls["productStatusId"].value;
        cmd.mainCountingUnitId =
          this.myForm.controls["mainCountingUnitId"].value;
        cmd.commonCountingUnitId =
          this.myForm.controls["commonCountingUnitId"].value;
        cmd.productBrandId = this.myForm.controls["brandId"].value;
        cmd.productCategoryId = this.myForm.controls["productCategoryId"].value;
        cmd.productAttributeTypeId =
          this.myForm.controls["productAttributeTypeId"].value;
        cmd.productOptionColorId =
          this.myForm.controls["productOptionColorId"].value;
        cmd.productOptionMaterialId =
          this.myForm.controls["productOptionMaterialId"].value;
        cmd.compatibility = this.myForm.controls["compatibility"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.refreshOrders();
              Swal.fire({
                title: "ذخیره محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.myForm.reset();
              this.selectedId = 0;
            } else {
              Swal.fire({
                title: "ذخیره محصول با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.selectedId = 0;
            }
          },
          (error) => {
            this.inProgress = false;
            console.error(error);
          }
        );
      } else {
        this.client.create(this.myForm.value).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.refreshOrders();
              Swal.fire({
                title: "ذخیره محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.myForm.reset();
              this.selectedId = 0;
            } else {
              Swal.fire({
                title: "ذخیره محصول با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.selectedId = 0;
            }
          },
          (error) => {
            this.inProgress = false;
            console.error(error);
          }
        );
      }
    } else {
      this.inProgress = false;
    }
  }

  resetForm(): void {
    Object.keys(this.myForm.controls).forEach((controlName) => {
      const control = this.myForm.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
  }

  edit(item: any) {
    debugger;
    this.selectedId = item.id;

    this.myForm.setValue({
      id: this.selectedId ?? null,
      name: item.name ?? null,
      localizedName: item.localizedName ?? null,
      code: item.code ?? null,
      productNo: item.productNo ?? null,
      oldProductNo: item.oldProductNo ?? null,
      productCatalog: item.productCatalog ?? null,
      orderRate: item.orderRate ?? null,
      mileage: item.mileage ?? null,
      duration: item.duration ?? null,
      excerpt: item.excerpt ?? null,
      description: item.description ?? null,
      sku: item.sku ?? null,
      providerId: item.providerId ?? null,
      countryId: item.countryId ?? null,
      productTypeId: item.productTypeId ?? null,
      productStatusId: item.productStatusId ?? null,
      mainCountingUnitId: item.mainCountingUnitId ?? null,
      commonCountingUnitId: item.commonCountingUnitId ?? null,
      brandId: item.brandId ?? null,
      productCategoryId: item.productCategoryId ?? null,
      productAttributeTypeId: item.productAttributeTypeId ?? null,
      productOptionColorId: item.colorOptionId,
      productOptionMaterialId: item.materialOptionId,
      compatibility: item.compatibility ?? null,
      isActive: item.isActive ?? null,
    });
    this.providerId = item.providerId;
    this.countryId = item.countryId;
    this.productTypeId = item.productTypeId;
    this.productStatusId = item.productStatusId;
    this.brandId = item.brandId;
    this.productCategoryId = item.productCategoryId;
    this.commonCountingUnitId = item.commonCountingUnitId;
    this.mainCountingUnitId = item.mainCountingUnitId;
    this.availabilityId = item.availabilityId;
    this.productAttributeTypeId = item.productAttributeTypeId;
    this.productOptionMaterialId = item.materialOptionId;
    this.productOptionColorId = item.colorOptionId;
    this.compatibility = item.compatibility;

    this.myForm.controls["name"].disable();
    this.myForm.controls["localizedName"].disable();
    this.myForm.controls["code"].disable();
    this.myForm.controls["productNo"].disable();
    this.myForm.controls["oldProductNo"].disable();
    this.myForm.controls["productCatalog"].disable();
    this.myForm.controls["orderRate"].disable();
    this.myForm.controls["mileage"].disable();
    this.myForm.controls["duration"].disable();
    this.myForm.controls["excerpt"].disable();
    this.myForm.controls["description"].disable();
    this.myForm.controls["sku"].disable();
    this.myForm.controls["providerId"].disable();
    this.myForm.controls["countryId"].disable();
    this.myForm.controls["productTypeId"].disable();
    this.myForm.controls["productStatusId"].disable();
    this.myForm.controls["mainCountingUnitId"].disable();
    this.myForm.controls["commonCountingUnitId"].disable();
    this.myForm.controls["brandId"].disable();
    this.myForm.controls["productCategoryId"].disable();
    this.myForm.controls["productAttributeTypeId"].disable();
    this.myForm.controls["productOptionColorId"].disable();
    this.myForm.controls["productOptionMaterialId"].disable();
    this.myForm.controls["compatibility"].disable();

    forkJoin([
      this.getAllProductCategory(),
      this.getAllBrands(),
      this.getAllProductTypes(),
      this.getAllProviders(),
      this.getAllCountries(),
      this.getAllProductStatus(),
      this.getAllProductAttributeType(),
      this.getAllCountingUnits(),
      this.getAllProductOptionMaterials(),
      this.getAllProductOptionColors(),
    ]).subscribe(() => {
      this.modalService.open(this.formModal, {
        size: "lg",
        backdrop: "static",
      });
    });
  }

  openInsertModal() {
    forkJoin([
      this.getAllProductCategory(),
      this.getAllBrands(),
      this.getAllProductTypes(),
      this.getAllProviders(),
      this.getAllCountries(),
      this.getAllProductStatus(),
      this.getAllProductAttributeType(),
      this.getAllCountingUnits(),
      this.getAllProductOptionMaterials(),
      this.getAllProductOptionColors(),
    ]).subscribe(() => {
      this.modalService.open(this.formModal, {
        size: "lg",
        backdrop: "static",
      });
    });
  }

  openMoreDetailProduct(item: any) {
    this.selectedProduct = item;
    this.modalService.open(this.productTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }
  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    console.log("    this.selectedId = id;", this.selectedId);
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    this.client.deleteRange([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف محصول با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.refreshOrders();
        } else {
          Swal.fire({
            title: "حذف محصول با خطا مواجه شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
        }
      },
      (error) => {
        this.inProgress = false;
        console.error(error);
      }
    );

    this.checkedItems = new Set<number>();
  }

  public getAllProductCategory() {
    let result = this.productCategoryClient.getAllProductCategoriesDropDown();
    result.subscribe(
      (result) => {
        this.productCategoryList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }

  public getAllBrands() {
    let result = this.brandsClient.getAllProductBrandsDropDown();
    result.subscribe(
      (result) => {
        this.brandList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }
  public getAllProductStatus() {
    let result = this.productStatusesClient.getAllProductStatusesDropDown();
    result.subscribe(
      (result) => {
        this.productStatusList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }

  public getAllProductTypes() {
    let result = this.productTypesClient.getAllProductTypesDropDown();
    result.subscribe(
      (result) => {
        this.productTypeList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }

  public getAllProviders() {
    let result = this.providerClient.getAllProvidersDropDown();
    result.subscribe(
      (result) => {
        this.providerList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }

  public getAllProductAttributeType() {
    let result =
      this.productAttributeTypeClient.getAllProductAttributeTypesDropDown();
    result.subscribe(
      (result) => {
        this.poductAttributeTypeList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }
  public getAllCountries() {
    let result = this.countryClient.getAllCountriesDropDown();
    result.subscribe(
      (result) => {
        this.countryList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }

  public getAllCountingUnits() {
    let result = this.countingUnitsClient.getAllCountingUnitsDropDown();
    result.subscribe(
      (result) => {
        this.countingUnitsList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }

  public getAllProductOptionColors() {
    let result =
      this.productOptionColorsClient.getAllProductOptionColorsDropDown();
    result.subscribe(
      (result) => {
        this.productOptionColorsList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }
  public getAllProductOptionMaterials() {
    let result =
      this.productOptionMaterialsClient.getAllProductOptionMaterialsDropDown();
    result.subscribe(
      (result) => {
        this.productOptionMaterialsList = result;
      },
      (error) => console.error(error)
    );
    return result;
  }
  public closeFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }
  public handleCloseFormModal() {
    this.closeFormModal(this.myForm);
    this.myForm.controls["name"].enable();
    this.myForm.controls["localizedName"].enable();
    this.myForm.controls["code"].enable();
    this.myForm.controls["productNo"].enable();
    this.myForm.controls["oldProductNo"].enable();
    this.myForm.controls["productCatalog"].enable();
    this.myForm.controls["orderRate"].enable();
    this.myForm.controls["mileage"].enable();
    this.myForm.controls["duration"].enable();
    this.myForm.controls["excerpt"].enable();
    this.myForm.controls["description"].enable();
    this.myForm.controls["sku"].enable();
    this.myForm.controls["providerId"].enable();
    this.myForm.controls["countryId"].enable();
    this.myForm.controls["productTypeId"].enable();
    this.myForm.controls["productStatusId"].enable();
    this.myForm.controls["mainCountingUnitId"].enable();
    this.myForm.controls["commonCountingUnitId"].enable();
    this.myForm.controls["brandId"].enable();
    this.myForm.controls["productCategoryId"].enable();
    this.myForm.controls["productAttributeTypeId"].enable();
    this.myForm.controls["productOptionColorId"].enable();
    this.myForm.controls["productOptionMaterialId"].enable();
    this.myForm.controls["compatibility"].enable();
    this.selectedId = 0;
  }
  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control) => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }
}

