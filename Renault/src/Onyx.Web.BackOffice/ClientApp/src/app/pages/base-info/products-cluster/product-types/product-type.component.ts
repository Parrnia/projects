


import { Component, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { ProductsTypeGridService } from "./product-type-grid.service";
import { ProductTypeModel } from "./product-type.model";
import { NgbdProductTypeSortableHeader, SortEvent } from "./product-type-sortable.directive";
import { ProductTypesClient, UpdateProductTypeCommand } from "src/app/web-api-client";
import { ProductTypeValidators } from "./product-type-validators";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-product-type",
  templateUrl: "./product-type.component.html",
  styleUrls: ["./product-type.component.scss"],
  providers: [ProductsTypeGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductTypeComponent {
  // bread crumb items
  inProgress = false;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductTypeModel;
  gridjsList$!: Observable<ProductTypeModel[]>;
  total$: Observable<number>;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdProductTypeSortableHeader)
  productsType!: QueryList<NgbdProductTypeSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];

  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductTypesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ProductsTypeGridService,
    private exportService : ExportFileService,
    private formBuilder: UntypedFormBuilder
  ) {
    this.gridjsList$ = service.productsType$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      name: ["", [Validators.required, Validators.maxLength(50)]],
      localizedName: ["", [Validators.required, Validators.maxLength(50)]],
      code: ["", [Validators.required]],
    });

    this.form.name.addAsyncValidators(
      ProductTypeValidators.validProductTypeName(
        this.client,
        this.form.name.value
      )
    );
    this.form.localizedName.addAsyncValidators(
      ProductTypeValidators.validProductTypeLocalizedName(
        this.client,
        this.form.localizedName.value
      )
    );

    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        ProductTypeValidators.validProductTypeName(
          this.client,
          id != null ? id : 0
        )
      );
      this.form.localizedName.setAsyncValidators(
        ProductTypeValidators.validProductTypeLocalizedName(
          this.client,
          id != null ? id : 0
        )
      );
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه ویژگی های محصول" },
      { label: "نوع محصول", active: true },
    ];
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
    // resetting other headers
    this.productsType.forEach((bt) => {
      if (bt.sortable !== column) {
        bt.direction = "";
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

    console.log("form value", this.myForm.value);

    this.submit = true;
    if (this.myForm.valid) {
      // Form submission logic goes here
      console.log("form value", this.myForm.value);

      if (this.selectedId > 0) {
        let cmd = new UpdateProductTypeCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.code = this.myForm.controls["code"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllproductsType();
              Swal.fire({
                title: "ذخیره نوع محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع محصول با خطا مواجه شد",
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
        this.client.create(this.myForm.value).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllproductsType();
              Swal.fire({
                title: "ذخیره نوع محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع محصول با خطا مواجه شد",
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

  editMenu(item: any) {
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["code"].setValue(item.code ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);
    this.myForm.controls["localizedName"].setValue(item.localizedName ?? null);

    this.myForm.controls["code"].disable();
    this.myForm.controls["name"].disable();
    this.myForm.controls["localizedName"].disable();

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
    this.client.deleteRange([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف نوع محصول با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllproductsType();
        } else {
          Swal.fire({
            title: "حذف نوع محصول با خطا مواجه شد",
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

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.myForm.controls["code"].enable();
    this.myForm.controls["name"].enable();
    this.myForm.controls["localizedName"].enable();
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



