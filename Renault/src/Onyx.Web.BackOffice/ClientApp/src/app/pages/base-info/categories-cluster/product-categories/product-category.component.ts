import {
  Component,
  ElementRef,
  QueryList,
  Renderer2,
  ViewChild,
  ViewChildren,
} from "@angular/core";

import { Observable, of } from "rxjs";
import {
  FormBuilder,
  FormGroup,
  UntypedFormBuilder,
  Validators,
} from "@angular/forms";

import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
  ProductCategoriesClient,
  CountriesClient,
  CountryDto,
  FileParameter,
  FamiliesClient,
  ModelDto,
  ModelsClient,
  ProductCategoryDto,
  FileUploadMetadataDto,
  UpdateProductCategoryCommand,
  CreateProductCategoryCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { ProductCategoryGridService } from "./product-category-grid.service";
import { ProductCategoryModel } from "./product-category.model";
import {
  NgbdProductCategorySortableHeader,
  SortEvent,
} from "./product-category-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { NonNullAssert } from "@angular/compiler";
import { ProductCategoryValidators } from "./product-category-validators";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-product-category",
  templateUrl: "./product-category.component.html",
  styleUrls: ["./product-category.component.scss"],
  providers: [ProductCategoryGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductCategoryComponent {
  selectedFileUrl: string | undefined = undefined;
  selectedFileUrlMenu: string | undefined = undefined;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductCategoryModel;
  gridjsList$!: Observable<ProductCategoryModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  productCategoriesList?: ProductCategoryDto[];
  @ViewChildren(NgbdProductCategorySortableHeader)
  productCategories!: QueryList<NgbdProductCategorySortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  productParentCategoryId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductCategoriesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ProductCategoryGridService,
    private exportService: ExportFileService,
    private productCategoriesClient: ProductCategoriesClient,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.productCategories$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      code: [0, [Validators.required]],
      name: ["", [Validators.required, Validators.maxLength(100)]],
      localizedName: ["", [Validators.required, Validators.maxLength(100)]],
      productCategoryNo: ["", [Validators.required, Validators.maxLength(70)]],
      image: ["", [Validators.required]],
      menuImage: "",
      productParentCategoryId: 0,
      isPopular: false,
      isFeatured: false,
      isActive: false,
    });

    this.form.name.addAsyncValidators(
      ProductCategoryValidators.validProductCategoryName(
        this.client,
        this.form.name.value
      )
    );
    this.form.localizedName.addAsyncValidators(
      ProductCategoryValidators.validProductCategoryLocalizedName(
        this.client,
        this.form.localizedName.value
      )
    );
    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        ProductCategoryValidators.validProductCategoryName(
          this.client,
          id != null ? id : 0
        )
      );
      this.form.localizedName.setAsyncValidators(
        ProductCategoryValidators.validProductCategoryLocalizedName(
          this.client,
          id != null ? id : 0
        )
      );
    });
  }

  ngOnInit(): void {
    this.getAllProductCategories();

    this.breadCrumbItems = [
      { label: "خوشه دسته بندی" },
      { label: "دسته بندی های محصول", active: true },
    ];
  }
  onFileSelected(event: FileUploadMetadataDto | null): void {
    debugger;
    if (event) {
      this.myForm.controls["image"].setValue(event?.fileId);
    } else {
      this.myForm.controls["image"].setValue(null);
    }
  }

  onFileSelectedMenu(event: FileUploadMetadataDto | null): void {
    debugger;
    if (event) {
      this.myForm.controls["menuImage"].setValue(event.fileId);
    } else {
      this.myForm.controls["menuImage"].setValue(null);
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
    // resetting other productCategories
    this.productCategories.forEach((productCategory) => {
      if (productCategory.sortable !== column) {
        productCategory.direction = "";
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
    this.myForm.value.modelId = parseInt(this.productParentCategoryId);
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateProductCategoryCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.code = this.myForm.controls["code"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.productCategoryNo = this.myForm.controls["productCategoryNo"].value;
        cmd.productParentCategoryId = this.myForm.controls["productParentCategoryId"].value;
        cmd.isPopular = this.myForm.controls["isPopular"].value;
        cmd.isFeatured = this.myForm.controls["isFeatured"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        cmd.image = this.myForm.controls["image"].value;
        cmd.menuImage = this.myForm.controls["menuImage"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllProductCategory();
              Swal.fire({
                title: "ذخیره نوع با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع با خطا مواجه شد",
                icon: "error",
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
      } else {
        let cmd = new CreateProductCategoryCommand();
        cmd.code = this.myForm.controls["code"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.productCategoryNo = this.myForm.controls["productCategoryNo"].value;
        cmd.productParentCategoryId = this.myForm.controls["productParentCategoryId"].value;
        cmd.isPopular = this.myForm.controls["isPopular"].value;
        cmd.isFeatured = this.myForm.controls["isFeatured"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        cmd.image = this.myForm.controls["image"].value;
        cmd.menuImage = this.myForm.controls["menuImage"].value;
        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllProductCategory();
              Swal.fire({
                title: "ذخیره نوع با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع با خطا مواجه شد",
                icon: "error",
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

  edit(item: ProductCategoryModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["code"].setValue(item.code ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);
    this.myForm.controls["localizedName"].setValue(item.localizedName ?? null);
    this.myForm.controls["isPopular"].setValue(item.isPopular ?? null);
    this.myForm.controls["isFeatured"].setValue(item.isFeatured ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);
    this.myForm.controls["productCategoryNo"].setValue(
      item.productCategoryNo ?? null
    );
    this.myForm.controls["productParentCategoryId"].setValue(
      item.productParentCategoryId ?? null
    );
    this.myForm.controls["image"].setValue(item.image ?? null);
    this.myForm.controls["menuImage"].setValue(item.menuImage ?? null);
    this.selectedFileUrl = item.imageSrc;
    this.selectedFileUrlMenu = item.menuImageSrc;
    this.productParentCategoryId = item.productParentCategoryId ?? null;

    this.myForm.controls["code"].disable();
    this.myForm.controls["name"].disable();
    this.myForm.controls["localizedName"].disable();
    this.myForm.controls["productCategoryNo"].disable();
    this.myForm.controls["productParentCategoryId"].disable();

    this.modalService.open(this.formModal, { size: "lg", backdrop: "static" });
  }

  openInsertModal() {
    this.modalService.open(this.formModal, { size: "lg", backdrop: "static" });
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    console.log("    this.selectedId = id;", this.selectedId);
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    this.client.deleteRangeProductCategory([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف نوع با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllProductCategory();
        } else {
          Swal.fire({
            title: "حذف نوع با خطا مواجه شد",
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

  public getAllProductCategories() {
    this.productCategoriesClient.getAllProductCategories().subscribe(
      (result) => {
        this.productCategoriesList = result;
      },
      (error) => console.error(error)
    );
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.myForm.controls["code"].enable();
    this.myForm.controls["name"].enable();
    this.myForm.controls["localizedName"].enable();
    this.myForm.controls["productCategoryNo"].enable();
    this.myForm.controls["productParentCategoryId"].enable();
    this.selectedId = 0;
    this.selectedFileUrl = undefined;
    this.selectedFileUrlMenu = undefined;
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
