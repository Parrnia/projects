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
  ProductBrandsClient,
  CountriesClient,
  CountryDto,
  FileParameter,
  FileUploadMetadataDto,
  UpdateProductBrandCommand,
  CreateProductBrandCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { ProductBrandGridService } from "./product-brand-grid.service";
import { ProductBrandModel } from "./product-brand.model";
import {
  NgbdProductBrandSortableHeader,
  SortEvent,
} from "./product-brand-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { NonNullAssert } from "@angular/compiler";
import { ProductBrandValidators } from "./product-brand-validators";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-product-brand",
  templateUrl: "./product-brand.component.html",
  styleUrls: ["./product-brand.component.scss"],
  providers: [ProductBrandGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductBrandComponent {
  selectedFileUrl: string | undefined = undefined;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductBrandModel;
  gridjsList$!: Observable<ProductBrandModel[]>;
  total$: Observable<number>;
  countryList?: CountryDto[];
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdProductBrandSortableHeader)
  productBrands!: QueryList<NgbdProductBrandSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  countryId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductBrandsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ProductBrandGridService,
    private exportService: ExportFileService,
    public countryClient: CountriesClient,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.productBrands$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      brandLogo: ["", [Validators.required]],
      name: ["", [Validators.required, Validators.maxLength(50)]],
      localizedName: ["", [Validators.required, Validators.maxLength(50)]],
      code: ["", [Validators.required]],
      countryId: 0,
      isActive: false,
    });

    this.form.name.addAsyncValidators(
      ProductBrandValidators.validProductBrandName(this.client, 0)
    );
    this.form.localizedName.addAsyncValidators(
      ProductBrandValidators.validProductBrandLocalizedName(this.client, 0)
    );

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        ProductBrandValidators.validProductBrandName(
          this.client,
          id != null ? id : 0
        )
      );
      this.form.localizedName.setAsyncValidators(
        ProductBrandValidators.validProductBrandLocalizedName(
          this.client,
          id != null ? id : 0
        )
      );
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.getAllCountries();

    this.breadCrumbItems = [
      { label: "خوشه برند" },
      { label: "برند محصول", active: true },
    ];
  }

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["brandLogo"].setValue(event.fileId);
    } else {
      this.myForm.controls["brandLogo"].setValue(null);
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

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other productBrands
    this.productBrands.forEach((productBrand) => {
      if (productBrand.sortable !== column) {
        productBrand.direction = "";
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
    this.myForm.value.countryId = parseInt(this.countryId);
    debugger;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateProductBrandCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.brandLogo = this.myForm.controls["brandLogo"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.code = this.myForm.controls["code"].value;
        cmd.countryId = this.myForm.controls["countryId"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllProductBrand();
              Swal.fire({
                title: "ذخیره برند با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره برند با خطا مواجه شد",
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
        let cmd = new CreateProductBrandCommand();
        cmd.brandLogo = this.myForm.controls["brandLogo"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.code = this.myForm.controls["code"].value;
        cmd.countryId = this.myForm.controls["countryId"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllProductBrand();
              Swal.fire({
                title: "ذخیره برند با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره برند با خطا مواجه شد",
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

  edit(item: ProductBrandModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);
    this.myForm.controls["localizedName"].setValue(item.localizedName ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);
    this.myForm.controls["brandLogo"].setValue(item.brandLogo ?? null);
    if (this.fileInput) {
      this.fileInput.nativeElement.click();
    }
    this.myForm.controls["code"].setValue(item.code ?? null);
    this.myForm.controls["countryId"].setValue(item.countryId ?? null);
    this.selectedFileUrl = item.brandLogoSrc;

    this.countryId = item.countryId ?? null;

    this.myForm.controls["name"].disable();
    this.myForm.controls["localizedName"].disable();
    this.myForm.controls["code"].disable();
    this.myForm.controls["countryId"].disable();
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
    this.client.deleteRangeProductBrand([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف برند با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllProductBrand();
        } else {
          Swal.fire({
            title: "حذف برند با خطا مواجه شد",
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

  public getAllCountries() {
    this.countryClient.getAllCountries().subscribe(
      (result) => {
        this.countryList = result;
      },
      (error) => console.error(error)
    );
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.myForm.controls["name"].enable();
    this.myForm.controls["localizedName"].enable();
    this.myForm.controls["code"].enable();
    this.myForm.controls["countryId"].enable();
    this.selectedId = 0;
    this.selectedFileUrl = undefined;
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
