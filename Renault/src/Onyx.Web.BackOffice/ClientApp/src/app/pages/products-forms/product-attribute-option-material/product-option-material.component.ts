import { DecimalPipe } from "@angular/common";
import { ProductOptionMaterialsGridService } from "./product-option-material-grid.service";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { ProductOptionMaterialModel } from "./product-option-material.model";
import { Observable } from "rxjs";
import { Component, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { NgbdProductOptionMaterialSortableHeader, SortEvent } from "./product-option-material-sortable.directive";
import { ProductOptionMaterialsClient, UpdateProductOptionMaterialCommand } from "src/app/web-api-client";
import Swal from "sweetalert2";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-product-option-material",
  templateUrl: "./product-option-material.component.html",
  styleUrls: ["./product-option-material.component.scss"],
  providers: [ProductOptionMaterialsGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductAttributeOptionMaterialComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedOptionMaterial?: any;
  selectedItem?: ProductOptionMaterialModel;
  gridjsList$!: Observable<ProductOptionMaterialModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdProductOptionMaterialSortableHeader)
  brandTypes!: QueryList<NgbdProductOptionMaterialSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild("formModal") formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  @ViewChild("optionMaterialModal") optionMaterialModal: any = [];
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductOptionMaterialsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ProductOptionMaterialsGridService,
    private exportService: ExportFileService,
    private formBuilder: UntypedFormBuilder
  ) {
    this.gridjsList$ = service.optionMaterial$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      name: ["", [Validators.required]],
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "محصولات" },
      { label: "آپشن جنس", active: true },
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
    this.brandTypes.forEach((bt) => {
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
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);

        let cmd = new UpdateProductOptionMaterialCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.name = this.myForm.controls["name"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllOptionMaterials();
              Swal.fire({
                title: "ذخیره آپشن جنس با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.myForm.reset();
            } else {
              Swal.fire({
                title: "ذخیره آپشن جنس با خطا مواجه شد",
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
              this.service.getAllOptionMaterials();
              Swal.fire({
                title: "ذخیره آپشن جنس با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.myForm.reset();
            } else {
              Swal.fire({
                title: "ذخیره آپشن جنس با خطا مواجه شد",
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

    this.modalService.open(this.formModal);
  }

  openInsertModal() {
    this.modalService.open(this.formModal);
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    console.log("    this.selectedId = id;", this.selectedId);
    this.modalService.open(this.confirmationModal);
  }
  openMoreDetail(item: any) {
    this.selectedOptionMaterial = item;
    this.modalService.open(this.optionMaterialModal, {
      size: "lg",
      backdrop: "static",
    });
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
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
  deleteItems() {
    this.inProgress = true;
    this.client.deleteRange([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف آپشن جنس با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllOptionMaterials();
        } else {
          Swal.fire({
            title: "حذف آپشن جنس با خطا مواجه شد",
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
}



