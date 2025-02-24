

import { Component, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import { ProductAttributeTypesClient, UpdateProductAttributeTypeCommand } from "src/app/web-api-client";
import Swal from "sweetalert2";
import { ProductAttributeTypesModel } from "./product-attribute-types.model";
import { NgbdProductAttributeTypesSortableHeader, SortEvent } from "./product-attribute-types-sortable.directive";
import { ProductAttributeTypesGridService } from "./product-attribute-types-grid.service";
import { DecimalPipe } from "@angular/common";
import { ProductAttributeTypeValidators } from "./product-attribute-types-validators";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-product-attribute-types",
  templateUrl: "./product-attribute-types.component.html",
  styleUrls: ["./product-attribute-types.component.scss"],
  providers: [ProductAttributeTypesGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductAttributeTypesComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  selectedProductAttributeType?: any;
  submit!: boolean;
  selectedItem?: ProductAttributeTypesModel;
  gridjsList$!: Observable<ProductAttributeTypesModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdProductAttributeTypesSortableHeader)
  productAttributeTypes!: QueryList<NgbdProductAttributeTypesSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("productAttributeGroupModel") productAttributeGroupModel: any = [];
  @ViewChild("productAttributeTypeTabModel") productAttributeTypeTabModel: any =
    [];
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductAttributeTypesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService : ExportFileService,
    public service: ProductAttributeTypesGridService
  ) {
    this.gridjsList$ = service.productAttributeType$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      name: ["", [Validators.required]],
    });

    this.form.name.addAsyncValidators(
      ProductAttributeTypeValidators.validProductAttributeTypeName(
        this.client,
        0
      )
    );

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        ProductAttributeTypeValidators.validProductAttributeTypeName(
          this.client,
          id != null ? id : 0
        )
      );
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "محصول" },
      { label: "نوع ویژگی محصول", active: true },
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
    this.productAttributeTypes.forEach((bt) => {
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

  openMoreDetailProductType(item: any) {
    this.selectedProductAttributeType = item;
    this.modalService.open(this.productAttributeTypeTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }
  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);

        let cmd = new UpdateProductAttributeTypeCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.name = this.myForm.controls["name"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllProductAttributeType();
              Swal.fire({
                title: "ذخیره نوع ویژگی محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.handleCloseFormModal();
              this.modalService.dismissAll();
              this.myForm.reset();
            } else {
              Swal.fire({
                title: "ذخیره نوع ویژگی محصول با خطا مواجه شد",
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
              this.service.getAllProductAttributeType();
              Swal.fire({
                title: "ذخیره نوع ویژگی محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.myForm.reset();
            } else {
              Swal.fire({
                title: "ذخیره نوع ویژگی محصول با خطا مواجه شد",
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
    this.myForm.controls["name"].setValue(item.name ?? null);

    this.myForm.controls["name"].disable();

    this.modalService.open(this.formModal);
  }

  openInsertModal() {
    this.modalService.open(this.formModal);
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    this.client.deleteRange([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف نوع ویژگی محصول با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllProductAttributeType();
        } else {
          Swal.fire({
            title: "حذف نوع ویژگی محصول با خطا مواجه شد",
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
    this.myForm.controls["name"].enable();
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



